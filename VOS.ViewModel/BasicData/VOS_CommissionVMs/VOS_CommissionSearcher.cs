using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_CommissionVMs
{
    public partial class VOS_CommissionSearcher : BaseSearcher
    {
        public List<ComboSelectListItem> AllVOS_Ranges { get; set; }
        [Display(Name = "价格范围")]
        public Guid? VOS_RangeID { get; set; }

        protected override void InitVM()
        {
            AllVOS_Ranges = DC.Set<VOS_Range>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.PriceRangeGroup);
        }

    }
}
