using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PlanVMs
{
    public partial class VOS_PlanSearcher : BaseSearcher
    {
        [Display(Name = "计划编号")]
        public String Plan_no { get; set; }
        public List<ComboSelectListItem> AllShopnames { get; set; }
        [Display(Name = "店铺名称")]
        public Guid? ShopnameId { get; set; }
        [Display(Name = "开始时间")]
        public DateRange PlanSatrtTime { get; set; }
        [Display(Name = "结束时间")]
        public DateRange PlanEndTime { get; set; }

        protected override void InitVM()
        {
            AllShopnames = DC.Set<VOS_Shop>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
        }

    }
}
