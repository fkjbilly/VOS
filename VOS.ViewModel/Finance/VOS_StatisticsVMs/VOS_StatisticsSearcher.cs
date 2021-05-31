using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_StatisticsVMs
{
    public partial class VOS_StatisticsSearcher : BaseSearcher
    {

        [Display(Name = "开始-结束")]
        public DateRange datetime { get; set; }

        [Display(Name ="店铺")]
        public string ShopName{ get; set; }

        [Display(Name = "执行人")]
        public string Executor { get; set; }

        [Display(Name ="会员")]
        public string MemberName { get; set; }

        [Display(Name ="计划编号")]
        public string Plan_no { get; set; }

        [Display(Name = "方法")]
        public TaskType? TaskType { get; set; }

        public List<ComboSelectListItem> AllOrganization { get; set; }
        [Display(Name = "组织机构")]
        public Guid? OrganizationID { get; set; }

        protected override void InitVM() {
            datetime = new DateRange(Convert.ToDateTime("2021-02-15"/*DateTime.Now.ToString("yyyy-MM-dd")*/), DateTime.Now);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }
}
