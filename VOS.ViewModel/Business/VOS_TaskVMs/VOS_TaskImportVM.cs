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
        [Display(Name = "做单方法")]
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
        [Display(Name = "做单要求")]
        public ExcelPropety TRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TRequirement);
        [Display(Name = "区域要求")]
        public ExcelPropety AreaRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.AreaRequirement);
        [Display(Name = "搜索关键字")]
        public ExcelPropety SearchKeyword_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SearchKeyword);
        [Display(Name = "SKU")]
        public ExcelPropety SKU_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SKU);
        [Display(Name = "刷手佣金")]
        public ExcelPropety EmployeeCommission_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.EmployeeCommission);
        [Display(Name = "数量")]
        public ExcelPropety quantity_Excel { get; set; }

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

    }

}
