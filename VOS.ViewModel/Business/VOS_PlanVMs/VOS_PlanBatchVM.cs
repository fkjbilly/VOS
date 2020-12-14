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
    public partial class VOS_PlanBatchVM : BaseBatchVM<VOS_Plan, VOS_Plan_BatchEdit>
    {
        public VOS_PlanBatchVM()
        {
            ListVM = new VOS_PlanListVM();
            LinkedVM = new VOS_Plan_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Plan_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
