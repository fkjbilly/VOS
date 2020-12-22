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
        [Display(Name = "Email")]
        public ExcelPropety Email_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Email);
        [Display(Name = "Name")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.Name);
        [Display(Name = "CellPhone")]
        public ExcelPropety CellPhone_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.CellPhone);
        [Display(Name = "IsValid")]
        public ExcelPropety IsValid_Excel = ExcelPropety.CreateProperty<VOS_User>(x => x.IsValid);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_UserImportVM : BaseImportVM<VOS_UserTemplateVM, VOS_User>
    {

    }

}
