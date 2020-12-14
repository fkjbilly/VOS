using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CityVMs
{
    public partial class CityListVM : BasePagedListVM<City_View, CitySearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("City", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<City_View>> InitGridHeader()
        {
            return new List<GridColumn<City_View>>{
                this.MakeGridHeader(x => x.Name),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<City_View> GetSearchQuery()
        {
            var query = DC.Set<City>()
                .CheckContain(Searcher.Name, x=>x.Name)
                .CheckEqual(Searcher.ParentId, x=>x.ParentId)
                .Select(x => new City_View
                {
				    ID = x.ID,
                    Name = x.Name,
                    Name_view = x.Parent.Name,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class City_View : City{
        [Display(Name = "名称")]
        public String Name_view { get; set; }

    }
}
