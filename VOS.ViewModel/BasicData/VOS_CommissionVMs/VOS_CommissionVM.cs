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
        public List<ComboSelectListItem> AllPriceRange { get; set; }

        public VOS_CommissionVM()
        {
        }

        protected override void InitVM()
        {
            if (MemoryCacheHelper.Exists(MemoryCacheHelper.GetPriceRange)){
                AllPriceRange = (List<ComboSelectListItem>)MemoryCacheHelper.Get(MemoryCacheHelper.GetPriceRange);
            }else {
                AllPriceRange  = new List<ComboSelectListItem>() {
                        new ComboSelectListItem() { Text="1-100",Value= "1-100" },
                        new ComboSelectListItem() { Text="100-299",Value= "100-299" },
                        new ComboSelectListItem() { Text="300-499",Value= "300-499" },
                        new ComboSelectListItem() { Text="500-699",Value= "500-699" },
                        new ComboSelectListItem() { Text="700-999",Value= "700-999" },
                        new ComboSelectListItem() { Text="1000-1499",Value= "1000-1499" },
                        new ComboSelectListItem() { Text="1500-1999",Value= "1500-1999" },
                        new ComboSelectListItem() { Text="2000-2499",Value= "2000-2499" },
                        new ComboSelectListItem() { Text="2500-2999",Value= "2500-2999" },
                        new ComboSelectListItem() { Text="3000-3499",Value= "3000-3499" },
                        new ComboSelectListItem() { Text="3500-3999",Value= "3500-3999" },
                };
                MemoryCacheHelper.Set(MemoryCacheHelper.GetPriceRange, AllPriceRange, new TimeSpan(5, 0, 0, 0));
            }
           
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
