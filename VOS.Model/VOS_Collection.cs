using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Collection : PersistPoco
    {
        [Display(Name = "计划编号")]
        public VOS_Plan Plan_no { get; set; }
        [Display(Name = "计划编号")]
        public Guid? Plan_noId { get; set; }
        [Display(Name = "实收金额")]
        [Required(ErrorMessage = "实收金额不能为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的实收金额")]
        public string Collection { get; set; }
        [Display(Name = "状态")]
        [Required(ErrorMessage ="{0}不允许为空")]
        public CollectionState? CollectionState { get; set; }
        [Display(Name = "备注")]
        public string Remarks { get; set; }
    }

    public enum CollectionState
    {
        未到账,
        已到账,
    }
}
