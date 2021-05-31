using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_StatisticsVMs
{
    public partial class VOS_StatisticsTemplateVM : BaseTemplateVM
    {

	    protected override void InitVM()
        {
        }

    }

    public class VOS_StatisticsImportVM : BaseImportVM<VOS_StatisticsTemplateVM, VOS_Statistics>
    {

    }

}
