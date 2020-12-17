using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using VOS.ViewModel.Business.VOS_TaskVMs;

namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeVM : BaseCRUDVM<VOS_PEmployee>
    {
        public VOS_TaskListVM taskVM { get; set; } = new VOS_TaskListVM();
        public List<ComboSelectListItem> AllRecoms { get; set; }
        public List<ComboSelectListItem> AllSheng { get; set; }

        public List<ComboSelectListItem> AllShi { get; set; }
        public List<ComboSelectListItem> AllQu { get; set; }

        public Guid? ShengId { get; set; }
        public Guid? ShiId { get; set; }
        public VOS_PEmployeeVM()
        {
            SetInclude(x => x.Recom);
            SetInclude(x => x.Area);
        }

        protected override void InitVM()
        {
            taskVM.CopyContext(this);
            taskVM.SearcherMode = ListVMSearchModeEnum.MasterDetail;
            taskVM.Searcher.EmployeeId = Entity.ID;
            AllRecoms = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            AllSheng = DC.Set<City>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
            if (Entity.AreaId != null)
            {
                ShiId = DC.Set<City>().Where(a => a.ID == Entity.AreaId).Select(a => a.ParentId).SingleOrDefault();
                ShengId = DC.Set<City>().Where(a => a.ID == ShiId).Select(a => a.ParentId).SingleOrDefault();

                AllQu = DC.Set<City>().Where(x => x.ParentId == ShiId).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
                AllShi = DC.Set<City>().Where(x => x.ParentId == ShengId).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
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
        public override DuplicatedInfo<VOS_PEmployee> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.FullName));
            rv.AddGroup(SimpleField(a => a.Mobile));
            rv.AddGroup(SimpleField(a => a.JDAccount));
            rv.AddGroup(SimpleField(a => a.TaobaAccount));
            rv.AddGroup(SimpleField(a => a.WeChat));
            return rv;
        }
    }
}
