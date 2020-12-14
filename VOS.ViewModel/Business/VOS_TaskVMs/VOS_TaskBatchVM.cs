using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskBatchVM : BaseBatchVM<VOS_Task, VOS_Task_BatchEdit>
    {
        public VOS_TaskBatchVM()
        {
            ListVM = new VOS_TaskListVM();
            LinkedVM = new VOS_Task_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Task_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
