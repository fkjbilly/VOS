using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RangeVMs
{
    public partial class VOS_RangeListVM : BasePagedListVM<VOS_Range_View, VOS_RangeSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("VOS_Range", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Range_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Range_View>>{
                this.MakeGridHeader(x => x.MinNumber),
                this.MakeGridHeader(x => x.MaxNumber),
                this.MakeGridHeader(x => x.PriceRangeGroup),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Range_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Range>()
                .CheckContain(Searcher.PriceRangeGroup, x=>x.PriceRangeGroup)
                .Select(x => new VOS_Range_View
                {
				    ID = x.ID,
                    MinNumber = x.MinNumber,
                    MaxNumber = x.MaxNumber,
                    PriceRangeGroup = x.PriceRangeGroup,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Range_View : VOS_Range{

    }
}
