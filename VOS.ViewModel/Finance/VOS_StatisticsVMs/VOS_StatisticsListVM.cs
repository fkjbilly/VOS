﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VOS.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

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
            var data = new List<GridColumn<VOS_Statistics_View>>{
                    this.MakeGridHeader(x => x.TaskType).SetWidth(80),
                    this.MakeGridHeader(x => x.ShopName),
                    this.MakeGridHeader(x => x.Plan).SetSort(true),
                    this.MakeGridHeader(x => x.ExecutorTime),
                    this.MakeGridHeader(x => x.Peice).SetWidth(80).SetShowTotal(true),
                    this.MakeGridHeader(x => x.MemberName).SetWidth(120),
                    this.MakeGridHeader(x => x.Headquarters).SetFormat(CalculationtHeadquarters).SetWidth(120).SetShowTotal(true),
                    this.MakeGridHeader(x => x.proxy).SetFormat(CalculationtProxy).SetWidth(120).SetShowTotal(true),
                    this.MakeGridHeader(x => x.member).SetFormat(CalculationtMember).SetWidth(120).SetShowTotal(true),
                    this.MakeGridHeader(x => x.OrderState).SetBackGroundFunc((x)=>{
                        return  x.OrderState  switch
                            {
                                 OrderState.未分配=>"#009688",
                                 OrderState.已分配=>"#5FB878",
                                 OrderState.进行中=>"#FF5722",
                                 OrderState.已完成=>"#1E9FFF",
                                 OrderState.已返款=>"#CCFF99",
                                                 _=>""
                            };
                    }).SetForeGroundFunc((x)=>{
                        return "#000000";
                    }).SetWidth(102).SetSort(true),

            };
            if (ExpandVM.IsSuperAdministrator(this, LoginUserInfo.Id))
            {
                data.Insert(data.Count(), this.MakeGridHeader(x => x.OrganizationName).SetSort(true));
            }
            return data;
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
            double Sum;
            if (entity.TaskType == TaskType.隔天)
            {
                Sum = (CommissionModel.HeadquartersPrice * entity.discount) + CommissionModel.HeadquartersSeparate;
            }
            else
            {
                Sum = CommissionModel.HeadquartersPrice * entity.discount;
            }
            return Math.Round(Sum, 2, MidpointRounding.AwayFromZero).ToString();
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
            return Math.Round(Sum, 2, MidpointRounding.AwayFromZero).ToString();
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
            
            return Math.Round(CommissionModel.memberCommission, 2, MidpointRounding.AwayFromZero).ToString();
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
            catch (Exception)
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
                .CheckEqual(Searcher.PlanId, x => x.Plan.ID)
                .CheckEqual(Searcher.OrganizationID, x => x.Plan.OrganizationID)
                .CheckEqual(Searcher.OrderState, x => x.OrderState)
                .CheckEqual(Searcher.TaskType, x => x.TaskType)
                .Where(x => x.IsValid)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.Plan.OrganizationID)
                .Select(x => new VOS_Statistics_View
                {
                    ID = x.ID,
                    TaskType = x.TaskType,
                    ShopName = x.Plan.Shopname.ShopName,
                    OrderState = x.OrderState,
                    discount = x.Plan.Shopname.Customer.discount,
                    Plan = x.Plan.Plan_no,
                    OrganizationName = x.Plan.Organization.OrganizationName,
                    ExecutorTime = x.DistributionTime,
                    Executor = x.Executor.Name,
                    Peice = Convert.ToDouble(x.CommodityPrice),
                    MemberName = x.Employee.FullName,
                    CreateTime = x.CreateTime,
                })
                .OrderByDescending(x => x.Plan);
            return query;
        }


        public async Task GetCommissionSum()
        {
            var query = await GetSearchQuery().ToListAsync();
            this.CalculationModel = new CalculationModel();
            (double SumHeadquarters, double SumProxy, double SumMember, double SumPeice) = (0, 0, 0, 0);
            foreach (var item in query)
            {
                var CommissionModel = GetDiscount(item.Peice);
                if (CommissionModel != null)
                {
                    //总部
                    if (item.TaskType == TaskType.隔天)
                    {
                        SumHeadquarters += (CommissionModel.HeadquartersPrice * item.discount) + CommissionModel.HeadquartersSeparate;
                    }
                    else
                    {
                        SumHeadquarters += CommissionModel.HeadquartersPrice * item.discount;
                    }
                    //代理
                    SumProxy += item.TaskType == TaskType.隔天 ? CommissionModel.proxyCommission + CommissionModel.proxySeparate : CommissionModel.proxyCommission;
                    //会员(刷手)
                    SumMember += CommissionModel.memberCommission;
                }
                //价格
                SumPeice += item.Peice;
            }
            CalculationModel.SumHeadquarters = Math.Round(SumHeadquarters, 2, MidpointRounding.AwayFromZero).ToString();
            CalculationModel.SumProxy = Math.Round(SumProxy, 2, MidpointRounding.AwayFromZero).ToString();
            CalculationModel.SumMember = Math.Round(SumMember, 2, MidpointRounding.AwayFromZero).ToString();
            CalculationModel.SumPeice = Math.Round(SumPeice, 2, MidpointRounding.AwayFromZero).ToString();
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
        [Display(Name = "组织机构")]
        public string OrganizationName { get; set; }

        [Display(Name = "任务状态")]
        public OrderState OrderState { get; set; }
    }

    public class CalculationModel
    {
        [Display(Name = "总部佣金共")]
        public string SumHeadquarters { get; set; }

        [Display(Name = "代理佣金共")]
        public string SumProxy { get; set; }

        [Display(Name = "会员佣金共")]
        public string SumMember { get; set; }

        [Display(Name = "价格共")]
        public string SumPeice { get; set; }
    }
}
