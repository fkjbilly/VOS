using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Finance.VOS_CollectionVMs
{
    public partial class VOS_CollectionTemplateVM : BaseTemplateVM
    {
        [Display(Name = "计划编号")]
        public ExcelPropety Plan_no_Excel = ExcelPropety.CreateProperty<VOS_Collection>(x => x.Plan_noId);
        [Display(Name = "实收金额")]
        public ExcelPropety Collection_Excel = ExcelPropety.CreateProperty<VOS_Collection>(x => x.Collection);
        [Display(Name = "备注")]
        public ExcelPropety Remarks_Excel = ExcelPropety.CreateProperty<VOS_Collection>(x => x.Remarks);

	    protected override void InitVM()
        {
            Plan_no_Excel.DataType = ColumnDataType.ComboBox;
            Plan_no_Excel.ListItems = DC.Set<VOS_Plan>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Plan_no);
        }

    }

    public class VOS_CollectionImportVM : BaseImportVM<VOS_CollectionTemplateVM, VOS_Collection>
    {

    }

}
