using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using VOS.ViewModel.Business.VOS_PEmployeeVMs;

namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskBatchVM : BaseBatchVM<VOS_Task, VOS_Task_BatchEdit>
    {
        public VOS_PEmployeeListVM VOS_PEmployeeListVM { get; set; }
        public VOS_TaskBatchVM()
        {
            ListVM = new VOS_TaskListVM();
            LinkedVM = new VOS_Task_BatchEdit();
            VOS_PEmployeeListVM = new VOS_PEmployeeListVM();
        }
    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Task_BatchEdit : BaseVM
    {


        [Display(Name = "商品图片")]
        [Required(ErrorMessage ="图片不能为空")]
        public Guid CommodityPicId { get; set; }

        protected override void InitVM()
        {

        }

    }

}
