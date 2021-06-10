using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CityVMs
{
    public partial class CityBatchVM : BaseBatchVM<City, City_BatchEdit>
    {
        public CityBatchVM()
        {
            ListVM = new CityListVM();
            LinkedVM = new City_BatchEdit();
        }

        public override bool DoBatchDelete()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    foreach (var item in Ids)
                    {
                        var _City = DC.Set<City>().Where(x => x.ID.ToString().Equals(item)).SingleOrDefault();
                        DC.Set<City>().Update(_City).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    return true;
                    //base.DoBatchDelete();
                }
                catch (Exception)
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
    public class City_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
