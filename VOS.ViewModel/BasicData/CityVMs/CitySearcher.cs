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
    public partial class CitySearcher : BaseSearcher
    {
        [Display(Name = "名称")]
        public String Name { get; set; }
        public List<ComboSelectListItem> AllParents { get; set; }
        [Display(Name = "父级")]
        public Guid? ParentId { get; set; }

        protected override void InitVM()
        {
            AllParents = DC.Set<City>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
