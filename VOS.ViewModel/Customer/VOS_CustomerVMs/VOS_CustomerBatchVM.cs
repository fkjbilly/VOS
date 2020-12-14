using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_CustomerVMs
{
    public partial class VOS_CustomerBatchVM : BaseBatchVM<VOS_Customer, VOS_Customer_BatchEdit>
    {
        public VOS_CustomerBatchVM()
        {
            ListVM = new VOS_CustomerListVM();
            LinkedVM = new VOS_Customer_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Customer_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
