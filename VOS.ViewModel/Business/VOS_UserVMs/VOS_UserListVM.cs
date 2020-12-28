using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_UserVMs
{
    public partial class VOS_UserListVM : BasePagedListVM<VOS_User_View, VOS_UserSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                return new List<GridAction> {
                    this.MakeAction("Login","Login","选择",null, GridActionParameterTypesEnum.SingleId).SetOnClickScript("SelectAll").SetShowInRow(true).SetHideOnToolBar(true),
                };
            }
            else
            {
                return new List<GridAction>
                {
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                    this.MakeAction("VOS_User","EditPassword1","修改密码","修改密码",GridActionParameterTypesEnum.SingleId,"Business").SetShowInRow(true).SetHideOnToolBar(true),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_User", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
                };
            }

        }


        protected override IEnumerable<IGridColumn<VOS_User_View>> InitGridHeader()
        {
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                return new List<GridColumn<VOS_User_View>>{
                    this.MakeGridHeader(x => x.ITCode),
                    this.MakeGridHeader(x => x.Name),
                    this.MakeGridHeader(x => x.Sex),
                    this.MakeGridHeader(x => x.CellPhone),
                    this.MakeGridHeader(x => x.IsValid),
                    this.MakeGridHeaderAction(width: 100)
                };
            }
            else
            {
                return new List<GridColumn<VOS_User_View>>{
                    this.MakeGridHeader(x => x.ITCode),
                    this.MakeGridHeader(x => x.Name),
                    this.MakeGridHeader(x => x.Sex),
                    this.MakeGridHeader(x => x.CellPhone),
                    this.MakeGridHeader(x => x.PhotoId).SetFormat(PhotoIdFormat),
                    this.MakeGridHeader(x => x.IsValid),
                    this.MakeGridHeader(x => x.RoleName_view),
                    this.MakeGridHeader(x => x.GroupName_view),
                    this.MakeGridHeaderAction(width: 300)
                };
            }

        }
        private List<ColumnFormatInfo> PhotoIdFormat(VOS_User_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.PhotoId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.PhotoId,640,480),
            };
        }


        public override IOrderedQueryable<VOS_User_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_User>()
                .CheckContain(Searcher.ITCode, x => x.ITCode)
                .CheckContain(Searcher.Name, x => x.Name)
                .CheckContain(Searcher.CellPhone, x => x.CellPhone)
                .Select(x => new VOS_User_View
                {
                    ID = x.ID,
                    ITCode = x.ITCode,
                    Name = x.Name,
                    Sex = x.Sex,
                    CellPhone = x.CellPhone,
                    PhotoId = x.PhotoId,
                    IsValid = x.IsValid,
                    RoleName_view = x.UserRoles.Select(y => y.Role.RoleName).ToSpratedString(null, ","),
                    GroupName_view = x.UserGroups.Select(y => y.Group.GroupName).ToSpratedString(null, ","),
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_User_View : VOS_User
    {
        [Display(Name = "RoleName")]
        public String RoleName_view { get; set; }
        [Display(Name = "GroupName")]
        public String GroupName_view { get; set; }

    }
}
