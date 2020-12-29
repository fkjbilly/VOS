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

        public override void Validate()
        {
            base.Validate();
        }

        public override void DoAdd()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    base.DoAdd();
                    //添加到账记录后解锁任务
                    var Tasks = DC.Set<VOS_Task>().Where(x => x.PlanId == Entity.Plan_noId);
                    foreach (var task in Tasks)
                    {
                        task.IsLock = true;
                        task.UnlockerId = LoginUserInfo.Id;
                        task.UnlockTime = DateTime.Now;
                        DC.Set<VOS_Task>().Update(task);
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception)
                {
                    transaction.Rollback();
                    MSD.AddModelError("123","添加失败");
                }
            }
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }

        public override DuplicatedInfo<VOS_Collection> SetDuplicatedCheck() 
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.Plan_noId));
            return rv;
        }

        public void EidtLock(string PlanId)
        {
            SqlParameter[] parameters = {
                           new SqlParameter("@PlanId",PlanId) };
                DC.Run("update VOS_Tasks set IsLock=false where PlanId=@PlanId", System.Data.CommandType.Text, parameters);
        }
    }
}
