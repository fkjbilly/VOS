using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Plan : PersistPoco
    {
        [Display(Name = "计划编号")]
        [Required(ErrorMessage = "计划编号不能为空")]
        [StringLength(20, ErrorMessage = "计划编号长度超过限制")]
        public string Plan_no { get; set; }
        [Display(Name = "店铺名称")]
        public VOS_Shop Shopname { get; set; }
        [Display(Name = "店铺名称")]
        public Guid ShopnameId { get; set; }

        [Display(Name = "开始时间")]
        public DateTime PlanSatrtTime { get; set; }
        [Display(Name = "结束时间")]
        public DateTime PlanEndTime { get; set; }
        [Display(Name = "计划金额")]
        [Required(ErrorMessage = "计划金额不能为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的计划金额")]
        public string PlanFee { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "组织机构")]
        public Guid OrganizationID { get; set; }

        [Display(Name = "组织机构")]
        public VOS_Organization Organization { get; set; }
    }
}
