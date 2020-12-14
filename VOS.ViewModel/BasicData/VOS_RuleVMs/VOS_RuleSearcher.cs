using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;
using static VOS.Model.VOS_Rule;

namespace VOS.ViewModel.BasicData.VOS_RuleVMs
{
    public partial class VOS_RuleSearcher : BaseSearcher
    {
        [Display(Name = "规则名称")]
        public String RuleName { get; set; }
        [Display(Name = "规则类别")]
        public RuleTypes? RuleType { get; set; }
        [Display(Name = "是否启用")]
        public Boolean? IsUse { get; set; }

        protected override void InitVM()
        {
        }

    }
}
