﻿using System;
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

        [Display(Name = "计划编号")]
        public Guid? PlanId { get; set; }
        public List<ComboSelectListItem> AllPlans { get; set; }

        [Display(Name = "方法")]
        public TaskType? TaskType { get; set; }

        [Display(Name = "任务状态")]
        public OrderState? OrderState { get; set; }

        public List<ComboSelectListItem> AllOrganization { get; set; }

        [Display(Name = "组织机构")]
        public Guid? OrganizationID { get; set; }

        protected override void InitVM() {
            datetime = new DateRange(Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")), DateTime.Now);
            MyInitVM();
        }

        public void MyInitVM() {
            AllPlans = DC.Set<VOS_Plan>().DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }
}
