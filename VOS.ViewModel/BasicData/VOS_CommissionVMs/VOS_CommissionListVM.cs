using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_CommissionVMs
{
    public partial class VOS_CommissionListVM : BasePagedListVM<VOS_Commission_View, VOS_CommissionSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Commission", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Commission_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Commission_View>>{
                this.MakeGridHeader(x => x.PriceRange),
                this.MakeGridHeader(x => x.HeadquartersPrice),
                this.MakeGridHeader(x => x.proxyCommission),
                this.MakeGridHeader(x => x.memberCommission),
                this.MakeGridHeader(x => x.HeadquartersSeparate),
                this.MakeGridHeader(x => x.proxySeparate),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Commission_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Commission>()
                .CheckContain(Searcher.PriceRange, x=>x.PriceRange)
                .Select(x => new VOS_Commission_View
                {
				    ID = x.ID,
                    HeadquartersPrice = x.HeadquartersPrice,
                    proxyCommission = x.proxyCommission,
                    memberCommission = x.memberCommission,
                    HeadquartersSeparate = x.HeadquartersSeparate,
                    proxySeparate = x.proxySeparate,
                    PriceRange = x.PriceRange,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Commission_View : VOS_Commission{

    }
}
