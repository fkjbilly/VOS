using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class Category : PersistPoco, ITreeData<Category>
    {
        [Display(Name = "类目名称")]
        [Required(ErrorMessage = "类目名称不能为空")]
        [StringLength(20, ErrorMessage = "类目名称长度超过限制")]
        public string Name { get; set; }

        public List<Category> Children { get; set; }
        [Display(Name = "父级类目")]
        public Category Parent { get; set; }
        [Display(Name = "父级类目")]
        public Guid? ParentId { get; set; }
        [Display(Name = "周期单量")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入正确单量")]
        public string CycleNum { get; set; }
    }
}
