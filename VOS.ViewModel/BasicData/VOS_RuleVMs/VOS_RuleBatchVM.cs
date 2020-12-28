using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RuleVMs
{
    public partial class VOS_RuleBatchVM : BaseBatchVM<VOS_Rule, VOS_Rule_BatchEdit>
    {
        public VOS_RuleBatchVM()
        {
            ListVM = new VOS_RuleListVM();
            LinkedVM = new VOS_Rule_BatchEdit();
        }

        public override bool DoBatchDelete()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    foreach (var item in Ids)
                    {
                        var _Rule = DC.Set<VOS_Rule>().Where(x => x.ID.ToString().Equals(item)).SingleOrDefault();
                        DC.Set<VOS_Rule>().Update(_Rule).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    return true;//base.DoBatchDelete();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
           
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_Rule_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
