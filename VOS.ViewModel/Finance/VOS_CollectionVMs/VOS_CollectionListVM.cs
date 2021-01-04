using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_CollectionVMs
{
    public partial class VOS_CollectionListVM : BasePagedListVM<VOS_Collection_View, VOS_CollectionSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.Create, Localizer["Create"],"Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.Details, Localizer["Details"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.Import, Localizer["Import"], "Finance", dialogWidth: 800),
                this.MakeStandardAction("VOS_Collection", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Finance"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Collection_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Collection_View>>{
                this.MakeGridHeader(x => x.Plan_no_view),
                this.MakeGridHeader(x => x.Collection),
                this.MakeGridHeader(x => x.Remarks),
                this.MakeGridHeader(x => x.OrganizationName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Collection_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Collection>()
                .CheckEqual(Searcher.Plan_noId, x=>x.Plan_noId)
                .CheckEqual(Searcher.OrganizationID, x=>x.Plan_no.OrganizationID)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.Plan_no.OrganizationID)
                .Select(x => new VOS_Collection_View
                {
				    ID = x.ID,
                    Plan_no_view = x.Plan_no.Plan_no,
                    Collection = x.Collection,
                    Remarks = x.Remarks,
                    OrganizationName_view = x.Plan_no.Organization.OrganizationName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Collection_View : VOS_Collection{
        [Display(Name = "计划编号")]
        public String Plan_no_view { get; set; }

        [Display(Name ="组织机构")]
        public String OrganizationName_view { get; set; }

    }
}
