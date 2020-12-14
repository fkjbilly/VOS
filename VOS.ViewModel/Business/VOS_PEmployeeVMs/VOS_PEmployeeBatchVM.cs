using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeBatchVM : BaseBatchVM<VOS_PEmployee, VOS_PEmployee_BatchEdit>
    {
        public VOS_PEmployeeBatchVM()
        {
            ListVM = new VOS_PEmployeeListVM();
            LinkedVM = new VOS_PEmployee_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_PEmployee_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
