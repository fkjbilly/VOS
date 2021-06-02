using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CityVMs
{
    public partial class CityVM : BaseCRUDVM<City>
    {
        public List<ComboSelectListItem> AllParents { get; set; }

        public CityVM()
        {
            SetInclude(x => x.Parent);
        }

        protected override void InitVM()
        {
            AllParents = DC.Set<City>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
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
