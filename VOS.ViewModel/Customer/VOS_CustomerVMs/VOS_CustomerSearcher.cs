using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using static VOS.Model.VOS_Customer;

namespace VOS.ViewModel.Customer.VOS_CustomerVMs
{
    public partial class VOS_CustomerSearcher : BaseSearcher
    {
        [Display(Name = "客户名称")]
        public String cust_name { get; set; }
        public List<ComboSelectListItem> Allcust_regions { get; set; }
        [Display(Name = "客户地区")]
        public Guid? cust_regionId { get; set; }
        [Display(Name = "客户等级")]
        public level? cust_level { get; set; }
        [Display(Name = "客户标识")]
        public flag? cust_flag { get; set; }
        [Display(Name = "联系人")]
        public String link_name { get; set; }
        [Display(Name = "手机")]
        public String link_mobile { get; set; }
        [Display(Name ="组织机构")]
        public String DistributionID { get; set; }
        public List<ComboSelectListItem> AllDistribution { get; set; }

        protected override void InitVM()
        {
            AllDistribution = DC.Set<VOS_Distribution>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.DistributionName);
            Allcust_regions = DC.Set<City>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
