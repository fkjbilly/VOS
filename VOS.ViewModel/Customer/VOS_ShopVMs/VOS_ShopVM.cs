using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_ShopVMs
{
    public partial class VOS_ShopVM : BaseCRUDVM<VOS_Shop>
    {
        public List<ComboSelectListItem> AllCustomers { get; set; }

        public VOS_ShopVM()
        {
            SetInclude(x => x.Customer);
        }

        protected override void InitVM()
        {
            AllCustomers = DC.Set<VOS_Customer>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.cust_name);
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
