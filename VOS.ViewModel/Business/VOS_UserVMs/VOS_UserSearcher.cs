using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserVMs
{
    public partial class VOS_UserSearcher : BaseSearcher
    {
        [Display(Name = "账号")]
        public String ITCode { get; set; }
        [Display(Name = "姓名")]
        public String Name { get; set; }
        [Display(Name = "电话号码")]
        public String CellPhone { get; set; }
        [Display(Name ="是否有效")]
        public bool? IsValid { get; set; }

        public List<ComboSelectListItem> AllOrganization { get; set; }
        [Display(Name = "组织机构")]
        public Guid? OrganizationID { get; set; }

        protected override void InitVM()
        {
            AllOrganization= DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }
}
