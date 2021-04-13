using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    [Table("FrameworkGroups")]
    public class VOS_UserGroup: FrameworkGroup
    {
        [Required(ErrorMessage= "组织机构必填")]
        [Display(Name = "组织机构")]
        public Guid? OrganizationID { get; set; }

        [Display(Name = "组织机构")]
        public VOS_Organization? Organization { get; set; }
    }
}
