using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;
using System.Threading;

namespace VOS.ViewModel.Finance.VOS_StatisticsVMs
{
    public partial class VOS_StatisticsListVM : BasePagedListVM<VOS_Statistics_View, VOS_StatisticsSearcher>
    {

        public CalculationModel CalculationModel { get; set; }

        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.Create, Localizer["Create"],"Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.Details, Localizer["Details"], "Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Finance", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.Import, Localizer["Import"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Statistics", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Finance"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Statistics_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Statistics_View>>{
                    this.MakeGridHeader(x => x.TaskType).SetWidth(80),
                    this.MakeGridHeader(x => x.ShopName),
                    this.MakeGridHeader(x => x.Plan),
                    this.MakeGridHeader(x => x.ExecutorTime),
                    this.MakeGridHeader(x => x.Peice).SetWidth(80).SetShowTotal(true),
                    this.MakeGridHeader(x => x.MemberName).SetWidth(120),
                    this.MakeGridHeader(x => x.Headquarters).SetFormat(CalculationtHeadquarters).SetWidth(120).SetShowTotal(true),
                    this.MakeGridHeader(x => x.proxy).SetFormat(CalculationtProxy).SetWidth(120).SetShowTotal(true),
                    this.MakeGridHeader(x => x.member).SetFormat(CalculationtMember).SetWidth(120).SetShowTotal(true),
            };
        }

        #region 计算相关

        /// <summary>
        /// 计算总部佣金
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private string CalculationtHeadquarters(VOS_Statistics_View entity, object val)
        {
            var CommissionModel = GetDiscount(entity.Peice);
            if (CommissionModel == null)
            {
                return "0";
            }
            double Sum = entity.TaskType == TaskType.隔天 ? (CommissionModel.HeadquartersPrice + CommissionModel.HeadquartersSeparate) : CommissionModel.HeadquartersPrice * entity.discount;
            return Sum.ToString();
        }

        /// <summary>
        /// 计算代理佣金
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private string CalculationtProxy(VOS_Statistics_View entity, object val)
        {
            var CommissionModel = GetDiscount(entity.Peice);
            if (CommissionModel == null)
            {
                return "0";
            }
            double Sum = entity.TaskType == TaskType.隔天 ? CommissionModel.proxyCommission + CommissionModel.proxySeparate : CommissionModel.proxyCommission;
            return Sum.ToString();
        }

        /// <summary>
        /// 计算会员佣金
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private string CalculationtMember(VOS_Statistics_View entity, object val)
        {
            var CommissionModel = GetDiscount(entity.Peice);
            if (CommissionModel == null)
            {
                return "0";
            }
            return CommissionModel.memberCommission.ToString();
        }

        public VOS_Commission GetDiscount(double CommodityPrice)
        {
            try
            {
                var RangeModel = DC.Set<VOS_Range>().Where(x => CommodityPrice >= x.MinNumber && CommodityPrice <= x.MaxNumber).OrderByDescending(x => x.CreateTime).FirstOrDefault();
                if (RangeModel != null)
                {
                    return DC.Set<VOS_Commission>().Where(x => x.VOS_Range.PriceRangeGroup == RangeModel.PriceRangeGroup).OrderByDescending(x => x.CreateBy).FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public override IOrderedQueryable<VOS_Statistics_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Task>()
                .CheckBetween(Searcher.datetime?.GetStartTime(), Searcher.datetime?.GetEndTime(), x => x.ImplementStartTime, includeMax: false)
                .CheckContain(Searcher.ShopName, x => x.Plan.Shopname.ShopName)
                .CheckContain(Searcher.Executor, x => x.Executor.Name)
                .CheckContain(Searcher.MemberName, x => x.Employee.FullName)
                .CheckContain(Searcher.Plan_no, x => x.Plan.Plan_no)
                .CheckEqual(Searcher.OrganizationID, x => x.Plan.OrganizationID)
                .CheckEqual(Searcher.TaskType, x => x.TaskType)
                .Where(x => x.IsValid)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.Plan.OrganizationID)
                .Select(x => new VOS_Statistics_View
                {
                    ID = x.ID,
                    TaskType = x.TaskType,
                    ShopName = x.Plan.Shopname.ShopName,
                    discount = x.Plan.Shopname.Customer.discount,
                    Plan = x.Plan.Plan_no,
                    ExecutorTime = x.DistributionTime,
                    Executor = x.Executor.Name,
                    Peice =Convert.ToDouble(x.CommodityPrice),
                    MemberName = x.Employee.FullName,
                }).Take(600)
                .OrderBy(x => x.ID);
            return query;
        }


        public async Task GetCommissionSum()
        {
            var query = await GetSearchQuery().ToListAsync();
            this.CalculationModel = new CalculationModel();
            double SumHeadquarters =0, SumProxy = 0,  SumMember = 0, SumPeice = 0;
            foreach (var item in query)
            {
                var CommissionModel = GetDiscount(item.Peice);
                if (CommissionModel != null)
                {
                    //总部
                    SumHeadquarters += item.TaskType == TaskType.隔天 ? (CommissionModel.HeadquartersPrice + CommissionModel.HeadquartersSeparate) : CommissionModel.HeadquartersPrice * item.discount;
                    //代理
                    SumProxy += item.TaskType == TaskType.隔天 ? CommissionModel.proxyCommission + CommissionModel.proxySeparate : CommissionModel.proxyCommission;
                    //会员(刷手)
                    SumMember += CommissionModel.memberCommission;
                }
                //价格
                SumPeice += item.Peice;
            }
            CalculationModel.SumHeadquarters = SumHeadquarters.ToString("0.00");
            CalculationModel.SumProxy = SumProxy.ToString("0.00");
            CalculationModel.SumMember = SumMember.ToString("0.00");
            CalculationModel.SumPeice = SumPeice.ToString("0.00");
        }
    }

    public class VOS_Statistics_View : VOS_Statistics
    {
        [Display(Name = "方法")]
        public TaskType TaskType { get; set; }
        [Display(Name = "店铺")]
        public string ShopName { get; set; }

        [Display(Name = "执行人")]
        public string Executor { get; set; }

        [Display(Name = "价格")]
        public double Peice { get; set; }

        [Display(Name = "计划编号")]
        public string Plan { get; set; }

        [Display(Name = "执行时间")]
        public DateTime? ExecutorTime { get; set; }

        [Display(Name = "会员")]
        public string MemberName { get; set; }

        [Display(Name = "客户则扣")]
        public double discount { get; set; }
    }

    public class CalculationModel
    {
        [Display(Name = "总部佣金共")]
        public string SumHeadquarters { get; set; }

        [Display(Name = "代理佣金共")]
        public string SumProxy { get; set; }

        [Display(Name = "会员佣金共")]
        public string SumMember { get; set; }

        [Display(Name ="价格")]
        public string SumPeice { get; set; }
    }
}
