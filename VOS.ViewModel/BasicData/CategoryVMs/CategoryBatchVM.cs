using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CategoryVMs
{
    public partial class CategoryBatchVM : BaseBatchVM<Category, Category_BatchEdit>
    {
        public CategoryBatchVM()
        {
            ListVM = new CategoryListVM();
            LinkedVM = new Category_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class Category_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
