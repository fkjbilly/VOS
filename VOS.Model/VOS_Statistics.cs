using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    [Display(Name = "数据统计")]
    public class VOS_Statistics : PersistPoco
    {
        [Display(Name = "执行人")]
        public Guid? VOS_TaskID { get; set; }

        [Display(Name = "执行人")]
        public VOS_Task VOS_Task { get; set; }

        /// <summary>
        /// 总部佣金=(总部原价+2（只有隔天单，其他不加）)*客户则扣
        /// </summary>
        [NotMapped]
        [Display(Name = "总部佣金")]
        public string Headquarters { get; set; }

        /// <summary>
        /// 代理佣金=代理原价+1（只有隔天单，其他不加）
        /// </summary>
        [NotMapped]
        [Display(Name = "代理佣金")]
        public double proxy { get; set; }

        /// <summary>
        /// 原刷手佣金  刷手=表格中单刷手金额
        /// </summary>
        [NotMapped]
        [Display(Name = "会员佣金")]
        public double member { get; set; }
    }
}
 