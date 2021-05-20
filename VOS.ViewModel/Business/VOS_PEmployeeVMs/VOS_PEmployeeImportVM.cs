using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeTemplateVM : BaseTemplateVM
    {
        [Display(Name = "推荐人")]
        public ExcelPropety Recom_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.RecomId);
        [Display(Name = "区域")]
        public ExcelPropety Area_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.AreaId);
        [Display(Name = "地址")]
        public ExcelPropety Address_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.Address);
        [Display(Name = "姓名")]
        public ExcelPropety FullName_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.FullName);
        [Display(Name = "性别")]
        public ExcelPropety Sex_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.Sex);
        [Display(Name = "联系电话")]
        public ExcelPropety Mobile_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.Mobile);
        [Display(Name = "微信账号")]
        public ExcelPropety WeChat_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.WeChat);
        [Display(Name = "淘宝账号")]
        public ExcelPropety TaobaAccount_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.TaobaAccount);
        [Display(Name = "京东账号")]
        public ExcelPropety JDAccount_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.JDAccount);
        [Display(Name = "备注")]
        public ExcelPropety Remark_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.Remark);
        [Display(Name = "会员状态")]
        public ExcelPropety PEstate_Excel = ExcelPropety.CreateProperty<VOS_PEmployee>(x => x.PEstate);

	    protected override void InitVM()
        {
            Recom_Excel.DataType = ColumnDataType.ComboBox;
            Recom_Excel.ListItems = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            Area_Excel.DataType = ColumnDataType.ComboBox;
            Area_Excel.ListItems = DC.Set<City>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }

    public class VOS_PEmployeeImportVM : BaseImportVM<VOS_PEmployeeTemplateVM, VOS_PEmployee>
    {

    }

}
