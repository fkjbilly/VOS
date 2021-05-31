using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RangeVMs
{
    public partial class VOS_RangeSearcher : BaseSearcher
    {
        [Display(Name = "价格范围")]
        public String PriceRangeGroup { get; set; }

        protected override void InitVM()
        {
        }

    }
}
