using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_CommissionVMs
{
    public partial class VOS_CommissionBatchVM : BaseBatchVM<VOS_Commission, VOS_Commission_BatchEdit>
    {
        public VOS_CommissionBatchVM()
        {
            ListVM = new VOS_CommissionListVM();
            LinkedVM = new VOS_Commission_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Commission_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
