using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using VOS.ViewModel.Business.VOS_PEmployeeVMs;

namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskVM : BaseCRUDVM<VOS_Task>
    {
        public List<ComboSelectListItem> AllPlans { get; set; }
        public List<ComboSelectListItem> AllTaskCates { get; set; }
        public List<ComboSelectListItem> AllUnlockers { get; set; }
        public List<ComboSelectListItem> AllExecutors { get; set; }
        public List<ComboSelectListItem> AllDistributors { get; set; }
        public List<ComboSelectListItem> AllEmployees { get; set; }
        public List<ComboSelectListItem> AllCompleters { get; set; }
        public List<ComboSelectListItem> AllRefunders { get; set; }
        public List<ComboSelectListItem> AllOrganization { get; set; }
        public List<ComboSelectListItem> AllShopnames { get; set; }

        #region MyRegion
        public VOS_PEmployeeListVM VOS_PEmployeeListVM { get; set; }
        #endregion

        public VOS_TaskVM()
        {
            SetInclude(x => x.Plan);
            SetInclude(x => x.TaskCate);
            SetInclude(x => x.Unlocker);
            SetInclude(x => x.Executor);
            SetInclude(x => x.Distributor);
            SetInclude(x => x.Employee);
            SetInclude(x => x.Completer);
            SetInclude(x => x.Refunder);
        }

        protected override void InitVM()
        {
            VOS_PEmployeeListVM = new VOS_PEmployeeListVM();
            //VOS_PEmployeeList = new VOS_PEmployeeListVM();
            //VOS_PEmployeeList.CopyContext(this);
            //VOS_PEmployeeList.SearcherMode = ListVMSearchModeEnum.Custom1;
            
            AllPlans = DC.Set<VOS_Plan>().DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
            AllTaskCates = DC.Set<Category>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name).ToList();
            AllUnlockers = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllExecutors = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllDistributors = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllEmployees = DC.Set<VOS_PEmployee>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.FullName);
            AllCompleters = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllRefunders = DC.Set<FrameworkUserBase>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.CodeAndName);
            AllOrganization = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
            AllShopnames = DC.Set<VOS_Shop>().DPWhere(LoginUserInfo.DataPrivileges, x => x.Customer.OrganizationID).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.ShopName);
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

        public override DuplicatedInfo<VOS_Task> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.Task_no));
            //rv.AddGroup(SimpleField(a => a.PlanId), SimpleField(a => a.CommodityName));
            return rv;
        }
    }
}
