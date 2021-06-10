using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    /// <summary>
    /// 扩展VM
    /// </summary>
    public static class ExpandVM
    {
        private static string ContainRoles => "超级管理员,管理员,财务管理,财务,会计管理,会计";

        #region 仅在继承BaseVM中使用

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
                var FrameworkRoleList = GetFrameworkRoleObject(baseVM.DC, ID);
                return FrameworkRoleList != null && FrameworkRoleList.Where(x => x.RoleName == "超级管理员")!.Count() > 0;
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
        public static bool NoContainRoles(this BaseVM baseVM, Guid ID)
        {
            try
            {
                var FrameworkRoleList = GetFrameworkRoleObject(baseVM.DC, ID);
                return FrameworkRoleList != null && FrameworkRoleList.Where(x => ContainRoles.IndexOf(x.RoleName) >= 0)!.Count() <= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region 仅在继承BaseSearcher中使用

        /// <summary>
        /// 当前登陆人角色不在其中  
        /// </summary>
        /// <param name="BaseSearcher">传入this</param>
        /// <param name="ID">当前登陆人ID</param>
        /// <returns></returns>
        public static bool NoContainRoles(this BaseSearcher BaseSearcher, Guid ID)
        {
            try
            {
                var FrameworkRoleList = GetFrameworkRoleObject(BaseSearcher.DC, ID);
                return FrameworkRoleList != null && FrameworkRoleList.Where(x => ContainRoles.IndexOf(x.RoleName) >= 0)!.Count() <= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


        /// <summary>
        /// BaseVM 和 BaseSearcher (获取当前登陆人角色List)
        /// </summary>
        /// <param name="DC">数据库环境</param>
        /// <param name="ID">当前登陆人</param>
        /// <returns></returns>
        public static List<FrameworkRole> GetFrameworkRoleObject(IDataContext DC, Guid ID)
        {
            var FrameworkUserRoleList = DC.Set<FrameworkUserRole>().Where(x => x.UserId == ID).Select(x => x.RoleId).ToList();
            if (FrameworkUserRoleList!.Count() < 0) { return null; }
            return DC.Set<FrameworkRole>().Where(x => FrameworkUserRoleList.Contains(x.ID)).ToList();
        }
    }
}
