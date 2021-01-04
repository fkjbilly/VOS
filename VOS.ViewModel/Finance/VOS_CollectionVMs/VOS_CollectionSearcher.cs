using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_CollectionVMs
{
    public partial class VOS_CollectionSearcher : BaseSearcher
    {
        public List<ComboSelectListItem> AllPlan_nos { get; set; }
        public List<ComboSelectListItem> AllOrganization { get; set; }
        [Display(Name = "计划编号")]
        public Guid? Plan_noId { get; set; }
        [Display(Name ="组织机构")]
        public Guid? OrganizationID { get; set; }
        protected override void InitVM()
        {
            AllPlan_nos = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }
}
