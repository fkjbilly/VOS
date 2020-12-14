using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RuleVMs
{
    public partial class VOS_RuleTemplateVM : BaseTemplateVM
    {
        [Display(Name = "规则名称")]
        public ExcelPropety RuleName_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.RuleName);
        [Display(Name = "规则类别")]
        public ExcelPropety RuleType_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.RuleType);
        [Display(Name = "单量")]
        public ExcelPropety Num_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.Num);
        [Display(Name = "周期")]
        public ExcelPropety Cycle_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.Cycle);
        [Display(Name = "是否启用")]
        public ExcelPropety IsUse_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.IsUse);
        [Display(Name = "备注")]
        public ExcelPropety Remark_Excel = ExcelPropety.CreateProperty<VOS_Rule>(x => x.Remark);

	    protected override void InitVM()
        {
        }

    }

    public class VOS_RuleImportVM : BaseImportVM<VOS_RuleTemplateVM, VOS_Rule>
    {

    }

}
