using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserVMs
{
    public partial class VOS_UserTemplateVM : BaseTemplateVM
    {
        [Display(Name = "Account")]
        public ExcelPropety ITCode_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.ITCode);
        [Display(Name = "Password")]
        public ExcelPropety Password_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Password);
        [Display(Name = "Name")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Name);
        [Display(Name = "Sex")]
        public ExcelPropety Sex_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Sex);
        [Display(Name = "CellPhone")]
        public ExcelPropety CellPhone_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.CellPhone);
        [Display(Name = "HomePhone")]
        public ExcelPropety HomePhone_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.HomePhone);
        [Display(Name = "Address")]
        public ExcelPropety Address_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Address);
        [Display(Name = "ZipCode")]
        public ExcelPropety ZipCode_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.ZipCode);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_UserImportVM : BaseImportVM<VOS_UserTemplateVM, VOS_User>
    {

    }

}
