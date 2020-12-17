using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using VOS.ViewModel.BasicData.CityVMs;

namespace VOS.ViewModel.Customer.VOS_CustomerVMs
{
    public partial class VOS_CustomerVM : BaseCRUDVM<VOS_Customer>
    {

        public List<ComboSelectListItem> AllSheng { get; set; }

        public List<ComboSelectListItem> AllShi { get; set; }
        public List<ComboSelectListItem> AllQu { get; set; }

        public Guid? ShengId { get; set; }
        public Guid? ShiId { get; set; }

        public CityListVM citys { get; set; }
        public VOS_CustomerVM()
        {
            SetInclude(x => x.cust_region);
        }

        protected override void InitVM()
        {
            AllSheng = DC.Set<City>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
            if (Entity.cust_regionId != null)
            {
                ShiId = DC.Set<City>().Where(a => a.ID == Entity.cust_regionId).Select(a => a.ParentId).SingleOrDefault();
                ShengId = DC.Set<City>().Where(a => a.ID == ShiId).Select(a => a.ParentId).SingleOrDefault();

                AllQu = DC.Set<City>().Where(x => x.ParentId == ShiId).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
                AllShi = DC.Set<City>().Where(x => x.ParentId == ShengId).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
            }

            citys = new CityListVM();
            citys.CopyContext(this);
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

        public override DuplicatedInfo<VOS_Customer> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.cust_no));
            rv.AddGroup(SimpleField(a => a.cust_name));
            return rv;
        }
    }
}
