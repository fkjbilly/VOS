using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{

    [Display(Name ="佣金范围")]
    public class VOS_Range : BasePoco
    {
        [Display(Name = "最小值")]
        [Required(ErrorMessage ="{0}不允许为空")]
        public int MinNumber { get; set; }

        [Display(Name = "最大值")]
        [Required(ErrorMessage = "{0}不允许为空")]
        public int MaxNumber { get; set; }

        [Display(Name ="价格范围")]
        public string PriceRangeGroup { get; set; }
    }
}
