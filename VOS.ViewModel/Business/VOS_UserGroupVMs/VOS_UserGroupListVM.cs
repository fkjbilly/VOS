using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserGroupVMs
{
    public partial class VOS_UserGroupListVM : BasePagedListVM<VOS_UserGroup_View, VOS_UserGroupSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_UserGroup", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_UserGroup_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_UserGroup_View>>{
                this.MakeGridHeader(x => x.OrganizationName_view),
                this.MakeGridHeader(x => x.GroupCode),
                this.MakeGridHeader(x => x.GroupName),
                this.MakeGridHeader(x => x.GroupRemark),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_UserGroup_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_UserGroup>().DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID)
                .Select(x => new VOS_UserGroup_View
                {
				    ID = x.ID,
                    OrganizationName_view = x.Organization.OrganizationName,
                    GroupCode = x.GroupCode,
                    GroupName = x.GroupName,
                    GroupRemark = x.GroupRemark,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_UserGroup_View : VOS_UserGroup{
        [Display(Name = "组织机构")]
        public String OrganizationName_view { get; set; }

    }
}
