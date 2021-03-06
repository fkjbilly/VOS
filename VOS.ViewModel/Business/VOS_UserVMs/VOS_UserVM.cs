﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserVMs
{
    public partial class VOS_UserVM : BaseCRUDVM<VOS_User>
    {
        
        public List<ComboSelectListItem> AllUserRoless { get; set; }
        [Display(Name = "角色")]
        public List<Guid> SelectedUserRolesIDs { get; set; }
        public List<ComboSelectListItem> AllUserGroupss { get; set; }
        [Display(Name = "用户组")]
        public List<Guid> SelectedUserGroupsIDs { get; set; }

        public List<ComboSelectListItem> AllOrganization { get; set; }
        public VOS_UserVM()
        {
            SetInclude(x => x.UserRoles);
            SetInclude(x => x.UserGroups);
        }

        protected override void InitVM()
        {
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
            AllUserRoless = DC.Set<FrameworkRole>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.RoleName);
            SelectedUserRolesIDs = Entity.UserRoles?.Select(x => x.RoleId).ToList();
            AllUserGroupss = DC.Set<VOS_UserGroup>().DPWhere(LoginUserInfo?.DataPrivileges,x=>x.OrganizationID).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.GroupName);
            SelectedUserGroupsIDs = Entity.UserGroups?.Select(x => x.GroupId).ToList();
        }

        public override void DoAdd()
        {
            Entity.Password = Utils.GetMD5String(Entity.Password);
            Entity.UserRoles = new List<FrameworkUserRole>();
            if (SelectedUserRolesIDs != null)
            {
                foreach (var id in SelectedUserRolesIDs)
                {
                    Entity.UserRoles.Add(new FrameworkUserRole { RoleId = id });
                }
            }

            Entity.UserGroups = new List<FrameworkUserGroup>();
            if (SelectedUserGroupsIDs != null)
            {
                foreach (var id in SelectedUserGroupsIDs)
                {
                    Entity.UserGroups.Add(new FrameworkUserGroup { GroupId = id });
                }
            }
           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            Entity.UserRoles = new List<FrameworkUserRole>();
            if(SelectedUserRolesIDs != null )
            {
                SelectedUserRolesIDs.ForEach(x => Entity.UserRoles.Add(new FrameworkUserRole { ID = Guid.NewGuid(), RoleId = x }));
            }

            Entity.UserGroups = new List<FrameworkUserGroup>();
            if(SelectedUserGroupsIDs != null )
            {
                SelectedUserGroupsIDs.ForEach(x => Entity.UserGroups.Add(new FrameworkUserGroup { ID = Guid.NewGuid(), GroupId = x }));
            }

            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            var _User = DC.Set<VOS_User>().Where(x => x.ID.Equals(Entity.ID)).FirstOrDefault();
            _User.IsValid = false;
            DC.SaveChanges();
            //base.DoDelete();
        }
    }
}
