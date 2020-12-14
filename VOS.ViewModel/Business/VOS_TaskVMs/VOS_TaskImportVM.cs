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
        [Display(Name = "客服")]
        public ExcelPropety CustomerService_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CustomerService);
        [Display(Name = "联系方式")]
        public ExcelPropety Contact_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Contact);
        [Display(Name = "运营负责人")]
        public ExcelPropety ShopCharge_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ShopCharge);
        [Display(Name = "运营电话")]
        public ExcelPropety ShopChargeContact_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ShopChargeContact);
        [Display(Name = "执行开始")]
        public ExcelPropety ImplementStartTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ImplementStartTime);
        [Display(Name = "执行结束")]
        public ExcelPropety ImplementEndTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ImplementEndTime);
        [Display(Name = "商品类目")]
        public ExcelPropety TaskCate_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TaskCateId);
        [Display(Name = "商品名称")]
        public ExcelPropety CommodityName_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityName);
        [Display(Name = "商品链接")]
        public ExcelPropety CommodityLink_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityLink);
        [Display(Name = "商品权重")]
        public ExcelPropety Eweight_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Eweight);
        [Display(Name = "任务分")]
        public ExcelPropety TaskFen_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TaskFen);
        [Display(Name = "商品价格")]
        public ExcelPropety CommodityPrice_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CommodityPrice);
        [Display(Name = "公司佣金")]
        public ExcelPropety Commission_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Commission);
        [Display(Name = "其他费用")]
        public ExcelPropety OtherExpenses_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.OtherExpenses);
        [Display(Name = "其他要求")]
        public ExcelPropety ORequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ORequirement);
        [Display(Name = "做单要求")]
        public ExcelPropety TRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TRequirement);
        [Display(Name = "客服备注")]
        public ExcelPropety CRemarks_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CRemarks);
        [Display(Name = "区域要求")]
        public ExcelPropety AreaRequirement_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.AreaRequirement);
        [Display(Name = "是否TP")]
        public ExcelPropety IsTP_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.IsTP);
        [Display(Name = "搜索关键字")]
        public ExcelPropety SearchKeyword_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SearchKeyword);
        [Display(Name = "成交关键字")]
        public ExcelPropety DealKeyword_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.DealKeyword);
        [Display(Name = "SKU")]
        public ExcelPropety SKU_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.SKU);
        [Display(Name = "是否解锁")]
        public ExcelPropety IsLock_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.IsLock);
        [Display(Name = "解锁人")]
        public ExcelPropety Unlocker_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.UnlockerId);
        [Display(Name = "解锁时间")]
        public ExcelPropety UnlockTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.UnlockTime);
        [Display(Name = "执行人")]
        public ExcelPropety Executor_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.ExecutorId);
        [Display(Name = "分配人")]
        public ExcelPropety Distributor_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.DistributorId);
        [Display(Name = "分配时间")]
        public ExcelPropety DistributionTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.DistributionTime);
        [Display(Name = "刷手")]
        public ExcelPropety Employee_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.EmployeeId);
        [Display(Name = "刷手佣金")]
        public ExcelPropety EmployeeCommission_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.EmployeeCommission);
        [Display(Name = "刷单单号")]
        public ExcelPropety VOrderCode_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.VOrderCode);
        [Display(Name = "手机")]
        public ExcelPropety Phone_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.Phone);
        [Display(Name = "淘宝账号")]
        public ExcelPropety TBAccount_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.TBAccount);
        [Display(Name = "微信账号")]
        public ExcelPropety WXAccount_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.WXAccount);
        [Display(Name = "完成人")]
        public ExcelPropety Completer_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CompleterId);
        [Display(Name = "完成时间")]
        public ExcelPropety CompleteTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.CompleteTime);
        [Display(Name = "返款人")]
        public ExcelPropety Refunder_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.RefunderId);
        [Display(Name = "返款时间")]
        public ExcelPropety RefundTime_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.RefundTime);
        [Display(Name = "任务状态")]
        public ExcelPropety OrderState_Excel = ExcelPropety.CreateProperty<VOS_Task>(x => x.OrderState);

	    protected override void InitVM()
        {
            Plan_Excel.DataType = ColumnDataType.ComboBox;
            Plan_Excel.ListItems = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            TaskCate_Excel.DataType = ColumnDataType.ComboBox;
            TaskCate_Excel.ListItems = DC.Set<Category>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
            Unlocker_Excel.DataType = ColumnDataType.ComboBox;
            Unlocker_Excel.ListItems = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            Executor_Excel.DataType = ColumnDataType.ComboBox;
            Executor_Excel.ListItems = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            Distributor_Excel.DataType = ColumnDataType.ComboBox;
            Distributor_Excel.ListItems = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            Employee_Excel.DataType = ColumnDataType.ComboBox;
            Employee_Excel.ListItems = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            Completer_Excel.DataType = ColumnDataType.ComboBox;
            Completer_Excel.ListItems = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            Refunder_Excel.DataType = ColumnDataType.ComboBox;
            Refunder_Excel.ListItems = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
        }

    }

    public class VOS_TaskImportVM : BaseImportVM<VOS_TaskTemplateVM, VOS_Task>
    {

    }

}
