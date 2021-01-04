using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_OrganizationVMs
{
    public partial class VOS_OrganizationTemplateVM : BaseTemplateVM
    {
        [Display(Name = "组织机构")]
        public ExcelPropety OrganizationName_Excel = ExcelPropety.CreateProperty<VOS_Organization>(x => x.OrganizationName);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_OrganizationImportVM : BaseImportVM<VOS_OrganizationTemplateVM, VOS_Organization>
    {

    }

}
