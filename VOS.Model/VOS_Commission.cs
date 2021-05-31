using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    [Display(Name ="佣金")]
    public class VOS_Commission: PersistPoco
    {
        /// <summary>
        /// 总部原价
        /// </summary>
        [Display(Name ="总部原价")]
        [Required(ErrorMessage="{0}不允许为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的{0}")]
        public double HeadquartersPrice { get; set; }

        /// <summary>
        /// 代理佣金
        /// </summary>
        [Display(Name ="代理佣金")]
        [Required(ErrorMessage = "{0}不允许为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的{0}")]
        public double proxyCommission { get; set; }

        /// <summary>
        ///原刷手佣金
        /// </summary>
        [Display(Name = "会员佣金")]
        [Required(ErrorMessage = "{0}不允许为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的{0}")]
        public double memberCommission { get; set; }

        /// <summary>
        /// 总部隔天
        /// </summary>
        [Display(Name ="总部隔天")]
        [Required(ErrorMessage = "{0}不允许为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的{0}")]
        public double HeadquartersSeparate { get; set; }

        /// <summary>
        /// 代理隔天
        /// </summary>
        [Display(Name ="代理隔天")]
        [Required(ErrorMessage = "{0}不允许为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的{0}")]
        public double proxySeparate { get; set; }

        [Display(Name ="价格范围")]
        [Required(ErrorMessage = "{0}不允许为空")]
        [StringLength(100,ErrorMessage="{0}超过限制100内")]
        public string PriceRange { get; set; }
    }
}
