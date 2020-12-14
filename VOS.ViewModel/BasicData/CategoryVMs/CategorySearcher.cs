using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CategoryVMs
{
    public partial class CategorySearcher : BaseSearcher
    {
        [Display(Name = "类目名称")]
        public String Name { get; set; }
        public List<ComboSelectListItem> AllParents { get; set; }
        [Display(Name = "父级类目")]
        public Guid? ParentId { get; set; }

        protected override void InitVM()
        {
            AllParents = DC.Set<Category>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
        }

    }
}
