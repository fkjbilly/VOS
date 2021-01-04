using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Distribution : PersistPoco
    {
        [Display(Name = "组织机构")]
        [Required(ErrorMessage = "不可为空")]
        [StringLength(50, ErrorMessage = "字符在50以内")]
        public string DistributionName { get; set; }

    }
}
