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
            var RangeList = DC.Set<VOS_Range>().OrderBy(x => x.MinNumber).ToList();
            List<ComboSelectListItem> _ListItem = new List<ComboSelectListItem>();
            foreach (var item in RangeList)
            {
                _ListItem.Add(new ComboSelectListItem() { 
                    Text=item.PriceRangeGroup,
                    Value=item.ID,
                });
            }
            AllVOS_Ranges = _ListItem;// DC.Set<VOS_Range>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.PriceRangeGroup);
        }

    }
}
