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

        [Display(Name = "执行人")]
        public Guid? ExecutorId { get; set; }

        [Display(Name = "组织机构")]
        public Guid? OrganizationID { get; set; }
        public List<ComboSelectListItem> AllOrganization { get; set; }
        [Display(Name = "执行时间")]
        public DateRange Time { get; set; }

        [Display(Name = "店铺")]
        public List<Guid> ShopNames { get; set; }
        public List<ComboSelectListItem> AllShopName { get; set; }
        protected override void InitVM()
        {
            var _task = DC.Set<VOS_Task>().Select(x => new { shopid = x.Plan.Shopname.ID }).Distinct(x => x.shopid).ToList();
            string str = "";
            foreach (var item in _task)
            {
                str += item.shopid + ",";
            }
            AllShopName = DC.Set<VOS_Shop>().Where(x => str.IndexOf(x.ID.ToString()) >= 0).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
            Time = new DateRange(DateTime.Now.AddDays(-1).Date, DateTime.Now.AddHours(1));
            AllPlans = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllDistributors = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllEmployees = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }
}
