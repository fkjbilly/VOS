using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PlanVMs
{
    public partial class VOS_PlanVM : BaseCRUDVM<VOS_Plan>
    {
        public List<ComboSelectListItem> AllShopnames { get; set; }

        public VOS_PlanVM()
        {
            SetInclude(x => x.Shopname);
        }

        protected override void InitVM()
        {
            AllShopnames = DC.Set<VOS_Shop>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
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
