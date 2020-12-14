using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CategoryVMs
{
    public partial class CategoryListVM : BasePagedListVM<Category_View, CategorySearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Category", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<Category_View>> InitGridHeader()
        {
            return new List<GridColumn<Category_View>>{
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CycleNum),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Category_View> GetSearchQuery()
        {
            var query = DC.Set<Category>()
                .CheckContain(Searcher.Name, x=>x.Name)
                .CheckEqual(Searcher.ParentId, x=>x.ParentId)
                .Select(x => new Category_View
                {
				    ID = x.ID,
                    Name = x.Name,
                    Name_view = x.Parent.Name,
                    CycleNum = x.CycleNum,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class Category_View : Category{
        [Display(Name = "类目名称")]
        public String Name_view { get; set; }

    }
}
