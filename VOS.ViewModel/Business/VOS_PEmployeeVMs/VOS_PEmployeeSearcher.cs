using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using static VOS.Model.VOS_PEmployee;

namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeSearcher : BaseSearcher
    {
        [Display(Name = "姓名")]
        public String FullName { get; set; }
        [Display(Name = "联系电话")]
        public String Mobile { get; set; }
        [Display(Name = "微信账号")]
        public String WeChat { get; set; }
        [Display(Name = "淘宝账号")]
        public String TaobaAccount { get; set; }
        [Display(Name = "京东账号")]
        public String JDAccount { get; set; }
        [Display(Name = "刷手状态")]
        public state? PEstate { get; set; }
        [Display(Name ="创建人")]
        public string CreateBy { get; set; }
        [Display(Name ="开始时间")]
        public DateTime? StartTime { get; set; }
        [Display(Name ="结束时间")]
        public DateTime? EndTime { get; set; }

        public List<ComboSelectListItem> AllDistribution { get; set; }
        [Display(Name = "组织机构")]
        public Guid? DistributionID { get; set; }

        protected override void InitVM()
        {
            AllDistribution = DC.Set<VOS_Distribution>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.DistributionName);
        }

    }
}
