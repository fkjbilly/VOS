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
    public partial class VOS_OrganizationBatchVM : BaseBatchVM<VOS_Organization, VOS_Organization_BatchEdit>
    {
        public VOS_OrganizationBatchVM()
        {
            ListVM = new VOS_OrganizationListVM();
            LinkedVM = new VOS_Organization_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Organization_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
