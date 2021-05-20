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
        [Display(Name = "方法")]
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
        /// <summary>
        /// 分配人
        /// </summary>
        //public List<ComboSelectListItem> AllDistributors { get; set; }
        //[Display(Name = "分配人")]
        //public Guid? DistributorId { get; set; }

        [Display(Name = "会员")]
        public Guid? EmployeeId { get; set; }

        //文本
        [Display(Name = "会员")]
        public string Member { get; set; }
        [Display(Name = "执行人")]
        public string ExecutorName { get; set; }

        [Display(Name = "订单号")]
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

        [Display(Name = "店铺")]
        public string newShopName { get; set; }

        private DateTime ti => Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        protected override void InitVM()
        {
            Time = new DateRange(ti, DateTime.Now);
            //AllPlans = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            //AllDistributors = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            //AllEmployees = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            //AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
            MyInitVM();
        }

        public void MyInitVM()
        {
            AllPlans = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
            var query = DC.Set<VOS_Task>()
            .CheckEqual(TaskType, x => x.TaskType)
            .CheckContain(CommodityName, x => x.CommodityName)
            .CheckContain(SearchKeyword, x => x.SearchKeyword)
            .CheckContain(VOrderCode, x => x.VOrderCode)
            .CheckEqual(OrderState, x => x.OrderState)
            .CheckContain(ShopNames, x => x.Plan.Shopname.ID)
            .CheckContain(newShopName, x => x.Plan.Shopname.ShopName)
            .CheckEqual(OrganizationID, x => x.Plan.OrganizationID)
            .CheckBetween(Time?.GetStartTime(), Time?.GetEndTime(), x => x.ImplementStartTime, includeMax: false)
            .CheckContain(Member, x => x.Employee.FullName)
            .CheckContain(ExecutorName, x => x.Executor.Name)
            .Where(x => x.IsValid == true);
            const string list = "超级管理员,管理员,财务管理,财务,会计管理,会计";
            var a = DC.Set<FrameworkUserRole>().Where(x => x.UserId == LoginUserInfo.Id).Select(x => new { x.RoleId }).FirstOrDefault();
            var b = DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == a.RoleId.ToString()).FirstOrDefault();
            if (list.IndexOf(b.RoleName) < 0)
            {
                query = query.Where(x => x.ExecutorId.ToString() == LoginUserInfo.Id.ToString());
            }
            else
            {
                query = query.DPWhere(LoginUserInfo.DataPrivileges, x => x.Plan.OrganizationID);
            }
            var data = query.Where(x => x.IsValid == true).Select(x => x.Plan.Shopname.ID).ToList().Distinct();
            string str = "";
            foreach (var item in data)
            {
                str += item+",";
            }
            AllShopName = DC.Set<VOS_Shop>().DPWhere(LoginUserInfo.DataPrivileges, x => x.Customer.OrganizationID).Where(x => str.IndexOf(x.ID.ToString()) >= 0).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);

        }
    }
}
