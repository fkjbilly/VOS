using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using VOS.ViewModel.Business.VOS_TaskVMs;
using System.Data.SqlClient;

namespace VOS.ViewModel.Finance.VOS_CollectionVMs
{
    public partial class VOS_CollectionVM : BaseCRUDVM<VOS_Collection>
    {
        public List<ComboSelectListItem> AllPlan_nos { get; set; }

        public VOS_CollectionVM()
        {
            SetInclude(x => x.Plan_no);
        }

        protected override void InitVM()
        {
            AllPlan_nos = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
        }

        public override void DoAdd()
        {           
            base.DoAdd();
            this.EidtLock(Entity.Plan_noId.ToString());
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }

        public void EidtLock(string PlanId)
        {
            SqlParameter[] parameters = {
                           new SqlParameter("@PlanId",PlanId) };
                DC.Run("update VOS_Tasks set IsLock=false where PlanId=@PlanId", System.Data.CommandType.Text, parameters);
        }
    }
}
