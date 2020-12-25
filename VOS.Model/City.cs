using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class City : BasePoco,ITreeData<City>
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(20, ErrorMessage = "名称长度超过限制")]
        public string Name { get; set; }

        public List<City> Children { get; set; }
        [Display(Name = "父级")]
        public City Parent { get; set; }
        [Display(Name = "父级")]
        public Guid? ParentId { get; set; }

        [NotMapped]
        public string Sheng { get; set; }
        [NotMapped]
        public string Shi { get; set; }
        [NotMapped]
        public string Qu { get; set; }
    }
}
