using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using static VOS.Model.VOS_Shop;

namespace VOS.ViewModel.Customer.VOS_ShopVMs
{
    public partial class VOS_ShopSearcher : BaseSearcher
    {
        public List<ComboSelectListItem> AllCustomers { get; set; }
        [Display(Name = "客户")]
        public Guid? CustomerId { get; set; }
        [Display(Name = "店铺名称")]
        public String ShopName { get; set; }
        [Display(Name = "所属平台")]
        public Plat? ShopPlat { get; set; }
        [Display(Name ="组织机构")]
        public Guid? DistributionID { get; set; }
        public List<ComboSelectListItem> AllDistribution { get; set; }

        protected override void InitVM()
        {
            AllCustomers = DC.Set<VOS_Customer>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.cust_name);
            AllDistribution = DC.Set<VOS_Distribution>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.DistributionName);
        }

    }
}
