using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PlanVMs
{
    public partial class VOS_PlanTemplateVM : BaseTemplateVM
    {
        [Display(Name = "计划编号")]
        public ExcelPropety Plan_no_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.Plan_no);
        [Display(Name = "店铺名称")]
        public ExcelPropety Shopname_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.ShopnameId);
        [Display(Name = "开始时间")]
        public ExcelPropety PlanSatrtTime_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.PlanSatrtTime);
        [Display(Name = "结束时间")]
        public ExcelPropety PlanEndTime_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.PlanEndTime);
        [Display(Name = "计划金额")]
        public ExcelPropety PlanFee_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.PlanFee);
        [Display(Name = "备注")]
        public ExcelPropety Remark_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.Remark);
        [Display(Name = "组织机构")]
        public ExcelPropety Organization_Excel = ExcelPropety.CreateProperty<VOS_Plan>(x => x.OrganizationID);
        protected override void InitVM()
        {
            Shopname_Excel.DataType = ColumnDataType.ComboBox;
            Shopname_Excel.ListItems = DC.Set<VOS_Shop>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
            Organization_Excel.DataType = ColumnDataType.ComboBox;
            Organization_Excel.ListItems = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }

    public class VOS_PlanImportVM : BaseImportVM<VOS_PlanTemplateVM, VOS_Plan>
    {

    }

}
