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
    public partial class VOS_StatisticsBatchVM : BaseBatchVM<VOS_Statistics, VOS_Statistics_BatchEdit>
    {
        public VOS_StatisticsBatchVM()
        {
            ListVM = new VOS_StatisticsListVM();
            LinkedVM = new VOS_Statistics_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Statistics_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
