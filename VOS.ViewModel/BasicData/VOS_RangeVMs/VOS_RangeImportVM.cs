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
    public partial class VOS_RangeTemplateVM : BaseTemplateVM
    {
        [Display(Name = "最小值")]
        public ExcelPropety MinNumber_Excel = ExcelPropety.CreateProperty<VOS_Range>(x => x.MinNumber);
        [Display(Name = "最大值")]
        public ExcelPropety MaxNumber_Excel = ExcelPropety.CreateProperty<VOS_Range>(x => x.MaxNumber);
        [Display(Name = "价格范围")]
        public ExcelPropety PriceRangeGroup_Excel = ExcelPropety.CreateProperty<VOS_Range>(x => x.PriceRangeGroup);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_RangeImportVM : BaseImportVM<VOS_RangeTemplateVM, VOS_Range>
    {

    }

}
