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

        public List<ComboSelectListItem> AllOrganization { get; set; }

        public VOS_PlanVM()
        {
            SetInclude(x => x.Shopname);
        }

        protected override void InitVM()
        {
            //.DPWhere(LoginUserInfo.DataPrivileges, x => x.Customer.OrganizationID)数据权限
            AllShopnames = DC.Set<VOS_Shop>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
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
        public override DuplicatedInfo<VOS_Plan> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.Plan_no));
            return rv;
        }
    }
}
