using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskTemplateVM : BaseTemplateVM
    {
        [Display(Name = "任务编号")]
        public ExcelPropety Task_no_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Task_no);
        [Display(Name = "方法")]
        public ExcelPropety TaskType_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TaskType);
        [Display(Name = "计划编号")]
        public ExcelPropety Plan_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.PlanId);
        [Display(Name = "佣金折扣")]
        public ExcelPropety ComDis_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ComDis);
        [Display(Name = "运营负责人")]
        public ExcelPropety ShopCharge_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ShopCharge);
        [Display(Name = "执行开始")]
        public ExcelPropety ImplementStartTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ImplementStartTime);
        [Display(Name = "商品类目")]
        public ExcelPropety TaskCate_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TaskCateId);
        [Display(Name = "商品名称")]
        public ExcelPropety CommodityName_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityName);
        [Display(Name = "商品链接")]
        public ExcelPropety CommodityLink_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityLink);
        [Display(Name = "商品价格")]
        public ExcelPropety CommodityPrice_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityPrice);
        [Display(Name = "公司佣金")]
        public ExcelPropety Commission_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Commission);
        [Display(Name = "其他费用")]
        public ExcelPropety OtherExpenses_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.OtherExpenses);
        [Display(Name = "方法要求")]
        public ExcelPropety TRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TRequirement);
        [Display(Name = "区域要求")]
        public ExcelPropety AreaRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.AreaRequirement);
        [Display(Name = "搜索关键字")]
        public ExcelPropety SearchKeyword_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SearchKeyword);
        [Display(Name = "SKU")]
        public ExcelPropety SKU_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SKU);
        [Display(Name = "会员佣金")]
        public ExcelPropety EmployeeCommission_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.EmployeeCommission);
        [Display(Name = "单量")]
        public ExcelPropety VOS_Number_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.VOS_Number);

        protected override void InitVM()
        {
            Plan_Excel.DataType = ColumnDataType.ComboBox;
            Plan_Excel.ListItems = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            TaskCate_Excel.DataType = ColumnDataType.ComboBox;
            TaskCate_Excel.ListItems = DC.Set<Category>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }
    }

    public class VOS_TaskImportVM : BaseImportVM<VOS_TaskTemplateVM, VOS_Task>
    {
        public override bool BatchSaveData()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    this.SetEntityList();
                    List<VOS_Task> newList = new List<VOS_Task>();
                    foreach (var item in EntityList)
                    {
                        //单量是否大于1
                        if (item.VOS_Number > 1)
                        {
                            for (int i = 1; i < item.VOS_Number; i++)
                            {
                                newList.Add(new VOS_Task()
                                {
                                    IsValid = true,
                                    Task_no = item.Task_no + "-" + i,
                                    TaskType = item.TaskType,
                                    PlanId = item.PlanId,
                                    ComDis = item.ComDis,
                                    ShopCharge = item.ShopCharge,
                                    ImplementStartTime = item.ImplementStartTime,
                                    TaskCateId = item.TaskCateId,
                                    CommodityName = item.CommodityName,
                                    CommodityLink = item.CommodityLink,
                                    CommodityPrice = item.CommodityPrice,
                                    Commission = item.Commission,
                                    OtherExpenses = item.OtherExpenses,
                                    TRequirement = item.TRequirement,
                                    AreaRequirement = item.AreaRequirement,
                                    SearchKeyword = item.SearchKeyword,
                                    SKU = item.SKU,
                                    EmployeeCommission = item.EmployeeCommission,
                                    IsLock = false,
                                    IsTP = false,

                                });
                            }
                        }
                        VOS_Task vOS_Task = new VOS_Task()
                        {
                            IsValid = true,
                            Task_no = item.Task_no,
                            TaskType = item.TaskType,
                            PlanId = item.PlanId,
                            ComDis = item.ComDis,
                            ShopCharge = item.ShopCharge,
                            ImplementStartTime = item.ImplementStartTime,
                            TaskCateId = item.TaskCateId,
                            CommodityName = item.CommodityName,
                            CommodityLink = item.CommodityLink,
                            CommodityPrice = item.CommodityPrice,
                            Commission = item.Commission,
                            OtherExpenses = item.OtherExpenses,
                            TRequirement = item.TRequirement,
                            AreaRequirement = item.AreaRequirement,
                            SearchKeyword = item.SearchKeyword,
                            SKU = item.SKU,
                            EmployeeCommission = item.EmployeeCommission,
                            IsLock = false,
                            IsTP = false,
                        };
                        newList.Add(vOS_Task);
                    }
                    this.EntityList = newList;
                    transaction.Commit();
                    return base.BatchSaveData();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
    }

}
