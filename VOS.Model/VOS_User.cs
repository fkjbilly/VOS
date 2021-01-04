using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    [Table("FrameworkUsers")]
    public class VOS_User : FrameworkUserBase
    {
        [Display(Name = "组织机构")]
        public Guid OrganizationID { get; set; }

        [Display(Name = "组织机构")]
        public VOS_Organization Organization { get; set; }
    }
}
