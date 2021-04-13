using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserGroupVMs
{
    public partial class VOS_UserGroupBatchVM : BaseBatchVM<VOS_UserGroup, VOS_UserGroup_BatchEdit>
    {
        public VOS_UserGroupBatchVM()
        {
            ListVM = new VOS_UserGroupListVM();
            LinkedVM = new VOS_UserGroup_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_UserGroup_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
