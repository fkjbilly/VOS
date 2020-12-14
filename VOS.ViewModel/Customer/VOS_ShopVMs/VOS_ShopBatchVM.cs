using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_ShopVMs
{
    public partial class VOS_ShopBatchVM : BaseBatchVM<VOS_Shop, VOS_Shop_BatchEdit>
    {
        public VOS_ShopBatchVM()
        {
            ListVM = new VOS_ShopListVM();
            LinkedVM = new VOS_Shop_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Shop_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
