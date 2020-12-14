using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_ShopVMs
{
    public partial class VOS_ShopTemplateVM : BaseTemplateVM
    {
        [Display(Name = "客户")]
        public ExcelPropety Customer_Excel = ExcelPropety.CreateProperty<VOS_Shop>(x => x.CustomerId);
        [Display(Name = "店铺名称")]
        public ExcelPropety ShopName_Excel = ExcelPropety.CreateProperty<VOS_Shop>(x => x.ShopName);
        [Display(Name = "所属平台")]
        public ExcelPropety ShopPlat_Excel = ExcelPropety.CreateProperty<VOS_Shop>(x => x.ShopPlat);
        [Display(Name = "开店时间")]
        public ExcelPropety OpenTime_Excel = ExcelPropety.CreateProperty<VOS_Shop>(x => x.OpenTime);

	    protected override void InitVM()
        {
            Customer_Excel.DataType = ColumnDataType.ComboBox;
            Customer_Excel.ListItems = DC.Set<VOS_Customer>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.cust_name);
        }

    }

    public class VOS_ShopImportVM : BaseImportVM<VOS_ShopTemplateVM, VOS_Shop>
    {

    }

}
