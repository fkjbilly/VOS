using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_CommissionVMs
{
    public partial class VOS_CommissionTemplateVM : BaseTemplateVM
    {
        [Display(Name = "总部原价")]
        public ExcelPropety HeadquartersPrice_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.HeadquartersPrice);
        [Display(Name = "代理佣金")]
        public ExcelPropety proxyCommission_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.proxyCommission);
        [Display(Name = "会员佣金")]
        public ExcelPropety memberCommission_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.memberCommission);
        [Display(Name = "总部隔天")]
        public ExcelPropety HeadquartersSeparate_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.HeadquartersSeparate);
        [Display(Name = "代理隔天")]
        public ExcelPropety proxySeparate_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.proxySeparate);
        [Display(Name = "价格范围")]
        public ExcelPropety PriceRange_Excel = ExcelPropety.CreateProperty<VOS_Commission>(x => x.PriceRange);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_CommissionImportVM : BaseImportVM<VOS_CommissionTemplateVM, VOS_Commission>
    {

    }

}
