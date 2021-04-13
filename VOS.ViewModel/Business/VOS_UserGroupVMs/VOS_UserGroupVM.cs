using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserGroupVMs
{
    public partial class VOS_UserGroupVM : BaseCRUDVM<VOS_UserGroup>
    {
        public List<ComboSelectListItem> AllOrganizations { get; set; }

        public VOS_UserGroupVM()
        {
            SetInclude(x => x.Organization);
        }

        protected override void InitVM()
        {
            AllOrganizations = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

        public override void DoAdd()
        {           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
