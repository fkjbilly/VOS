﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_DistributionVMs
{
    public partial class VOS_DistributionSearcher : BaseSearcher
    {
        [Display(Name = "组织机构")]
        public String DistributionName { get; set; }

        protected override void InitVM()
        {
        }

    }
}
