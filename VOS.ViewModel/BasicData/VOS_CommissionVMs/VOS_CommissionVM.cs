using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_CommissionVMs
{
    public partial class VOS_CommissionVM : BaseCRUDVM<VOS_Commission>
    {
        public List<ComboSelectListItem> AllVOS_Ranges { get; set; }

        public VOS_CommissionVM()
        {
            SetInclude(x => x.VOS_Range);
        }

        protected override void InitVM()
        {

            var RangeList = DC.Set<VOS_Range>().OrderBy(x => x.MinNumber).ToList();
            List<ComboSelectListItem> _ListItem = new List<ComboSelectListItem>();
            foreach (var item in RangeList)
            {
                _ListItem.Add(new ComboSelectListItem()
                {
                    Text = item.PriceRangeGroup,
                    Value = item.ID,
                });
            }
            AllVOS_Ranges = _ListItem;//DC.Set<VOS_Range>().OrderBy(x => x.MinNumber).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.PriceRangeGroup);
        }

        public override void DoAdd()
        {
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
