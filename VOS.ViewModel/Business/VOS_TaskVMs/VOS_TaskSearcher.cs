using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskSearcher : BaseSearcher
    {
        [Display(Name = "做单方法")]
        public TaskType? TaskType { get; set; }
        public List<ComboSelectListItem> AllPlans { get; set; }
        [Display(Name = "计划编号")]
        public Guid? PlanId { get; set; }
        [Display(Name = "商品名称")]
        public String CommodityName { get; set; }
        [Display(Name = "搜索关键字")]
        public String SearchKeyword { get; set; }
        [Display(Name = "是否解锁")]
        public Boolean? IsLock { get; set; }
        public List<ComboSelectListItem> AllDistributors { get; set; }
        [Display(Name = "分配人")]
        public Guid? DistributorId { get; set; }
        public List<ComboSelectListItem> AllEmployees { get; set; }
        [Display(Name = "刷手")]
        public Guid? EmployeeId { get; set; }
        [Display(Name = "刷单单号")]
        public String VOrderCode { get; set; }
        [Display(Name = "任务状态")]
        public OrderState? OrderState { get; set; }

        [Display(Name = "店铺")]
        public String ShopName { get; set; }


        [Display(Name = "执行人")]
        public Guid? ExecutorId { get; set; }

        [Display(Name ="组织机构")]
        public Guid? DistributionID { get; set; }
        public List<ComboSelectListItem> AllDistribution { get; set; }
        
        protected override void InitVM()
        {
            AllPlans = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllDistributors = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllEmployees = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            AllDistribution = DC.Set<VOS_Distribution>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.DistributionName);
        }

    }
}
