using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_OrganizationVMs
{
    public partial class VOS_OrganizationListVM : BasePagedListVM<VOS_Organization_View, VOS_OrganizationSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Organization", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Organization_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Organization_View>>{
                this.MakeGridHeader(x => x.OrganizationName),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Organization_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Organization>()
                .CheckContain(Searcher.OrganizationName, x=>x.OrganizationName)
                .Select(x => new VOS_Organization_View
                {
				    ID = x.ID,
                    OrganizationName = x.OrganizationName,
                    CreateTime=x.CreateTime
                })
                .OrderByDescending(x => x.CreateTime.Value);
            return query;
        }

    }

    public class VOS_Organization_View : VOS_Organization{

    }
}
