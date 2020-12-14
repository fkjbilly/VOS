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
    public partial class CategoryTemplateVM : BaseTemplateVM
    {
        [Display(Name = "类目名称")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<Category>(x => x.Name);
        [Display(Name = "父级类目")]
        public ExcelPropety Parent_Excel = ExcelPropety.CreateProperty<Category>(x => x.ParentId);
        [Display(Name = "周期单量")]
        public ExcelPropety CycleNum_Excel = ExcelPropety.CreateProperty<Category>(x => x.CycleNum);

	    protected override void InitVM()
        {
            Parent_Excel.DataType = ColumnDataType.ComboBox;
            Parent_Excel.ListItems = DC.Set<Category>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }

    public class CategoryImportVM : BaseImportVM<CategoryTemplateVM, Category>
    {

    }

}
