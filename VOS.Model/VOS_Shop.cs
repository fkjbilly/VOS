using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Shop : PersistPoco
    {
        public enum Plat 
        {
            淘宝,
            天猫,
            京东,
            其他
        }

        [Display(Name ="客户")]
        public VOS_Customer Customer { get; set; }
        [Display(Name = "客户")]
        public Guid CustomerId { get; set; }
        [Display(Name ="店铺名称")]
        [Required(ErrorMessage = "请输入店铺名称")]
        [StringLength(50, ErrorMessage = "名称长度超过限制")]
        public string ShopName { get; set; }
        [Display(Name ="所属平台")]
        public Plat ShopPlat { get; set; }
        [Display(Name = "开店时间")]
        public DateTime OpenTime { get; set; }

    }
}
