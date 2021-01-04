using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VOS.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace VOS.Areas.BaseControllers
{
    public class VOS_BaseControllers : BaseController
    {
        /// <summary>
        /// 是否是管理员登录
        /// </summary>
        public bool IsSuperAdministrator
        {
            get
            {
                var a = DC.Set<FrameworkUserRole>().Where(x => x.UserId == LoginUserInfo.Id).Select(x => new { x.RoleId }).FirstOrDefault();
                var b = DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == a.RoleId.ToString()).FirstOrDefault();
                if (b.RoleName.Equals("超级管理员"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 当前登录人(组织机构)ID
        /// </summary>
        public Guid GetDistributionID
        {
            get
            {
                return DC.Set<VOS_User>().Where(x => x.ID.Equals(LoginUserInfo.Id)).FirstOrDefault().DistributionID;
            }
        }
    }
}
