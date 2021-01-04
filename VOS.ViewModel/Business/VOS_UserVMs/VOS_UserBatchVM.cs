using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserVMs
{
    public partial class VOS_UserBatchVM : BaseBatchVM<VOS_User, VOS_User_BatchEdit>
    {
        public VOS_UserBatchVM()
        {
            ListVM = new VOS_UserListVM();
            LinkedVM = new VOS_User_BatchEdit();
        }

        public override bool DoBatchDelete()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    foreach (var item in Ids)
                    {
                        var _User = DC.Set<VOS_User>().Where(x => x.ID.ToString().Equals(item)).FirstOrDefault();
                        _User.IsValid = false;
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            //return base.DoBatchDelete();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class VOS_User_BatchEdit : BaseVM
    {
        [Display(Name = "密码")]
        public String Password { get; set; }

        protected override void InitVM()
        {
        }

    }

}
