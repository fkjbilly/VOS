using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.CategoryVMs
{
    public partial class CategoryVM : BaseCRUDVM<Category>
    {
        public List<ComboSelectListItem> AllParents { get; set; }

        public CategoryVM()
        {
            SetInclude(x => x.Parent);
        }

        protected override void InitVM()
        {
            AllParents = DC.Set<Category>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name);
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

        public override DuplicatedInfo<Category> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.Name), SimpleField(a => a.ParentId));
            return rv;
        }
    }
}
