using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    /// <summary>
    /// 扩展 BaseVM  目前仅在ListVM中使用
    /// </summary>
    public static class ExpandBaseVM
    {
        /// <summary>
        /// 当前登陆人是超级管理员
        /// </summary>
        /// <param name="baseVM">传入this</param>
        /// <param name="ID">传入当前登陆人ID</param>
        /// <returns></returns>
        public static bool IsSuperAdministrator(this BaseVM baseVM, Guid ID)
        {
            try
            {
                var FrameworkUserRoleObj = baseVM.DC.Set<FrameworkUserRole>().Where(x => x.UserId == ID).Select(x => new { x.RoleId }).FirstOrDefault();
                if (FrameworkUserRoleObj == null) { return false; }
                var FrameworkRoleObj = baseVM.DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == FrameworkUserRoleObj.RoleId.ToString()).FirstOrDefault();
                return FrameworkRoleObj != null && FrameworkRoleObj.RoleName.Equals("超级管理员");
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 当前登陆人角色不在其中
        /// </summary>
        /// <param name="baseVM">传入this</param>
        /// <param name="ID">当前登陆人ID</param>
        /// <returns></returns>
        public static bool NotInContainRoles(this BaseVM baseVM, Guid ID)
        {
            try
            {
                const string ContainRoles = "超级管理员,管理员,财务管理,财务,会计管理,会计";
                var FrameworkUserRoleObj = baseVM.DC.Set<FrameworkUserRole>().Where(x => x.UserId == ID).Select(x => new { x.RoleId }).FirstOrDefault();
                if (FrameworkUserRoleObj == null) { return false; }
                var FrameworkRoleObj = baseVM.DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == FrameworkUserRoleObj.RoleId.ToString()).FirstOrDefault();
                return FrameworkRoleObj != null && ContainRoles.IndexOf(FrameworkRoleObj.RoleName) < 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
