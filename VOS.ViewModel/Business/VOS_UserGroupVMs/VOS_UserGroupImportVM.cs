using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserGroupVMs
{
    public partial class VOS_UserGroupTemplateVM : BaseTemplateVM
    {
        [Display(Name = "组织机构")]
        public ExcelPropety Organization_Excel = ExcelPropety.CreateProperty<VOS_UserGroup>(x => x.OrganizationID);
        [Display(Name = "GroupCode")]
        public ExcelPropety GroupCode_Excel = ExcelPropety.CreateProperty<VOS_UserGroup>(x => x.GroupCode);
        [Display(Name = "GroupName")]
        public ExcelPropety GroupName_Excel = ExcelPropety.CreateProperty<VOS_UserGroup>(x => x.GroupName);
        [Display(Name = "Remark")]
        public ExcelPropety GroupRemark_Excel = ExcelPropety.CreateProperty<VOS_UserGroup>(x => x.GroupRemark);

	    protected override void InitVM()
        {
            Organization_Excel.DataType = ColumnDataType.ComboBox;
            Organization_Excel.ListItems = DC.Set<VOS_Organization>().GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.OrganizationName);
        }

    }

    public class VOS_UserGroupImportVM : BaseImportVM<VOS_UserGroupTemplateVM, VOS_UserGroup>
    {

    }

}
