using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_DistributionVMs
{
    public partial class VOS_DistributionTemplateVM : BaseTemplateVM
    {
        [Display(Name = "组织机构")]
        public ExcelPropety DistributionName_Excel = ExcelPropety.CreateProperty<VOS_Distribution>(x => x.DistributionName);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_DistributionImportVM : BaseImportVM<VOS_DistributionTemplateVM, VOS_Distribution>
    {

    }

}
