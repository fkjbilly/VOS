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
        /// 是超级管理员登录
        /// </summary>
        protected bool IsSuperAdministrator
        {
            get
            {
                var FrameworkRoleList = ExpandVM.GetFrameworkRoleObject(this.DC, LoginUserInfo.Id);
                return FrameworkRoleList != null && FrameworkRoleList.Where(x => x.RoleName == "超级管理员")!.Count() > 0;
            }
        }

        /// <summary>
        /// 当前登录人(组织机构)ID
        /// </summary>
        protected Guid GetOrganizationID
        {
            get
            {
                return (Guid)DC.Set<VOS_User>().Where(x => x.ID.Equals(LoginUserInfo.Id)).FirstOrDefault().OrganizationID;
            }
        }

        /// <summary>
        /// 获取指定key的value值
        /// </summary>
        /// <param name="dic">Dictionary<string, object></param>
        /// <param name="key">需要获取的Key</param></param>
        /// <returns></returns>
        protected object GetAppointValue(Dictionary<string, object> dic, string key)
        {
            object result = "";
            foreach (KeyValuePair<string, object> kvp in dic)
            {
                if (kvp.Key.Equals(key))
                {
                    result = kvp.Value;
                    break;
                }
            }
            return result;
        }
    }
}
