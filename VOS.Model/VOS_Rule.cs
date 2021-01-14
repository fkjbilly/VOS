using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Rule : PersistPoco
    {
        public enum RuleTypes
        {
            //类目,
            店铺=1,
            间隔,
            周期
        }

        [Display(Name ="规则名称")]
        [Required(ErrorMessage = "规则名称不能为空")]
        [StringLength(20, ErrorMessage = "规则名称长度超过限制")]
        public string RuleName { get; set; }
        [Display(Name = "规则类别")]
        [Required(ErrorMessage = "规则类别不能为空")]
        public RuleTypes RuleType { get; set; }
        [Display(Name = "单量")]
        [Required(ErrorMessage = "单量不能为空")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入正确单量")]
        public string Num { get; set; }
        [Display(Name = "周期")]
        [Required(ErrorMessage = "周期不能为空")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入正确周期")]
        public string Cycle { get; set; }
        [Display(Name = "是否启用")]
        public bool IsUse { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
