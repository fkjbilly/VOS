using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RangeVMs
{
    public partial class VOS_RangeBatchVM : BaseBatchVM<VOS_Range, VOS_Range_BatchEdit>
    {
        public VOS_RangeBatchVM()
        {
            ListVM = new VOS_RangeListVM();
            LinkedVM = new VOS_Range_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Range_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
