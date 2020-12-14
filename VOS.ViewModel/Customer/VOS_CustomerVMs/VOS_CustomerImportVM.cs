using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_CustomerVMs
{
    public partial class VOS_CustomerTemplateVM : BaseTemplateVM
    {
        [Display(Name = "客户编号")]
        public ExcelPropety cust_no_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_no);
        [Display(Name = "客户名称")]
        public ExcelPropety cust_name_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_name);
        [Display(Name = "客户地区")]
        public ExcelPropety cust_region_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_regionId);
        [Display(Name = "客户地址")]
        public ExcelPropety cust_address_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_address);
        [Display(Name = "客户等级")]
        public ExcelPropety cust_level_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_level);
        [Display(Name = "客户电话")]
        public ExcelPropety cust_telephone_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_telephone);
        [Display(Name = "客户标识")]
        public ExcelPropety cust_flag_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.cust_flag);
        [Display(Name = "联系人")]
        public ExcelPropety link_name_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.link_name);
        [Display(Name = "性别")]
        public ExcelPropety link_sex_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.link_sex);
        [Display(Name = "职位")]
        public ExcelPropety link_position_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.link_position);
        [Display(Name = "手机")]
        public ExcelPropety link_mobile_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.link_mobile);
        [Display(Name = "备注")]
        public ExcelPropety Remark_Excel = ExcelPropety.CreateProperty<VOS_Customer>(x => x.Remark);

	    protected override void InitVM()
        {
            cust_region_Excel.DataType = ColumnDataType.ComboBox;
            cust_region_Excel.ListItems = DC.Set<City>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }

    public class VOS_CustomerImportVM : BaseImportVM<VOS_CustomerTemplateVM, VOS_Customer>
    {

    }

}
