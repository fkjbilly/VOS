using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    [Table("VOS_Organization")]
    public class VOS_Organization : PersistPoco
    {
        [Display(Name = "组织机构")]
        [Required(ErrorMessage = "不可为空")]
        [StringLength(50, ErrorMessage = "字符在50以内")]
        public string OrganizationName { get; set; }

    }
}
