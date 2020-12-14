using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_CollectionVMs
{
    public partial class VOS_CollectionBatchVM : BaseBatchVM<VOS_Collection, VOS_Collection_BatchEdit>
    {
        public VOS_CollectionBatchVM()
        {
            ListVM = new VOS_CollectionListVM();
            LinkedVM = new VOS_Collection_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Collection_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
