using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;
using static VOS.Model.VOS_Rule;
using static VOS.Model.VOS_PEmployee;
using System.Text;

namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeListVM : BasePagedListVM<VOS_PEmployee_View, VOS_PEmployeeSearcher>
    {

        /// <summary>
        /// 是否显示
        /// </summary>
        private bool button_show = false;

        protected override List<GridAction> InitGridAction()
        {
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                return new List<GridAction> {
                    this.MakeAction("Login","Login","选择",null,GridActionParameterTypesEnum.SingleId,null)
                        .SetOnClickScript("SelectBrushHand").SetShowInRow(true).SetHideOnToolBar(true)
                        .SetBindVisiableColName("btn_show"),
                    this.MakeAction(null,null,"重派",null,GridActionParameterTypesEnum.NoId,null)
                        .SetOnClickScript("ResetBrushHand").SetShowInRow(true).SetHideOnToolBar(true)
                        .SetBindVisiableColName("btn_hide"),
                };

            }
            else
            {
                return new List<GridAction>
                {
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                    //this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                    //this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                    //this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                    //this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
                };
            }
        }

        protected override IEnumerable<IGridColumn<VOS_PEmployee_View>> InitGridHeader()
        {
            List<GridColumn<VOS_PEmployee_View>> data = null;
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                data = new List<GridColumn<VOS_PEmployee_View>>{
                    this.MakeGridHeader(x => x.FullName),
                    this.MakeGridHeader(x => x.WeChat),
                    this.MakeGridHeader(x => x.TaobaAccount),
                    this.MakeGridHeader(x => x.JDAccount),
                    this.MakeGridHeader(x=>x.CreateBy),
                    //选择按钮隐藏与显示
                    this.MakeGridHeader(x=> "btn_show").SetHide().SetFormat((x,y)=>{
                        if(x.button_show)
                            return "false";
                        else
                            return "true";
                    }),
                    //重派按钮隐藏与显示
                    this.MakeGridHeader(x=> "btn_hide").SetHide().SetFormat((x,y)=>{
                       if(x.button_show)
                            return "true";
                        else
                            return "false";
                    }),
                    this.MakeGridHeaderAction(width: 200)
                };
            }
            else
            {
                data = new List<GridColumn<VOS_PEmployee_View>>{
                    this.MakeGridHeader(x => x.FullName),
                    this.MakeGridHeader(x => x.WeChat),
                    this.MakeGridHeader(x => x.TaobaAccount),
                    this.MakeGridHeader(x => x.JDAccount),
                    this.MakeGridHeader(x => x.AlipayPicId).SetFormat(AlipayPicIdFormat),
                    this.MakeGridHeader(x => x.WeChatPicId).SetFormat(WeChatPicIdFormat),
                    this.MakeGridHeader(x => x.WeChatRealNamePicId).SetFormat(WeChatRealNamePicIdFormat),
                    this.MakeGridHeader(x => x.PEstate).SetBackGroundFunc((x)=>{
                        switch (x.PEstate)
                            {
                                case state.休息:
                                return "#c2c2c2";
                                case state.正常:
                                return "#1E9FFF";
                                case state.黑名单:
                                return "#393D49";
                            }
                        return "";
                    }).SetForeGroundFunc((x)=>{
                        return "#FFFFFF";
                    }).SetSort(true),
                    this.MakeGridHeader(x => x.CreateBy),
                    this.MakeGridHeader(x => x.CreateTime).SetSort(true),
                    this.MakeGridHeaderAction(width: 200)
                };
            }
            if (ExpandBaseVM.IsSuperAdministrator(this, LoginUserInfo.Id))
            {
                data.Insert(data.Count() - 1, this.MakeGridHeader(x => x.OrganizationName_view).SetSort(true));
            }
            return data;
        }
        private List<ColumnFormatInfo> AlipayPicIdFormat(VOS_PEmployee_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.AlipayPicId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.AlipayPicId,640,480),
            };
        }

        private List<ColumnFormatInfo> WeChatPicIdFormat(VOS_PEmployee_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.WeChatPicId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.WeChatPicId,640,480),
            };
        }

        private List<ColumnFormatInfo> WeChatRealNamePicIdFormat(VOS_PEmployee_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.WeChatRealNamePicId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.WeChatRealNamePicId,640,480),
            };
        }

        public override IOrderedQueryable<VOS_PEmployee_View> GetSearchQuery()
        {
            #region 共有条件
            var query = DC.Set<VOS_PEmployee>()
                   .CheckContain(Searcher.FullName, x => x.FullName)
                   .CheckContain(Searcher.Mobile, x => x.Mobile)
                   .CheckContain(Searcher.TaobaAccount, x => x.TaobaAccount)
                   .CheckContain(Searcher.JDAccount, x => x.JDAccount)
                   .CheckEqual(Searcher.PEstate, x => x.PEstate)
                   .CheckContain(Searcher.CreateBy, x => x.CreateBy)
                   .CheckContain(Searcher.WeChat, x => x.WeChat)
                   .CheckEqual(Searcher.OrganizationID, x => x.OrganizationID)
                   .CheckBetween(Searcher.StartTime, Searcher.EndTime, x => x.CreateTime, includeMax: false)
                   .DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID);
            #endregion
            //分配会员模式下
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                query = query.Where(x => x.PEstate == state.正常);
                var TaskObj = DC.Set<VOS_Task>().Where(x => x.ID.ToString().Equals(MemoryCacheHelper.Set_TaskID)).FirstOrDefault();
                if (TaskObj != null)
                {//已分配
                    query = query.Where(x => x.ID.ToString().Equals(TaskObj.EmployeeId.ToString()));
                    button_show = true;
                }
                else
                {//未分配
                    if (string.IsNullOrEmpty(Searcher.FullName) &&
                        string.IsNullOrEmpty(Searcher.Mobile) &&
                        string.IsNullOrEmpty(Searcher.TaobaAccount) &&
                        string.IsNullOrEmpty(Searcher.JDAccount) &&
                        string.IsNullOrEmpty(Searcher.CreateBy) &&
                        string.IsNullOrEmpty(Searcher.WeChat))
                    {
                        query = query.Where(x => x.ID.Equals("-1231231"));
                    }
                }
            }
            else
            {//
                if (string.IsNullOrEmpty(Searcher.FullName) &&
                        string.IsNullOrEmpty(Searcher.Mobile) &&
                        string.IsNullOrEmpty(Searcher.TaobaAccount) &&
                        string.IsNullOrEmpty(Searcher.JDAccount) &&
                        string.IsNullOrEmpty(Searcher.CreateBy) &&
                        string.IsNullOrEmpty(Searcher.WeChat))
                {
                    
                    if (ExpandBaseVM.NotInContainRoles(this, LoginUserInfo.Id))
                    {
                        //显示创建
                        query = query.Where(x => x.CreateBy.Equals(LoginUserInfo.ITCode));
                    }
                }
            }

            return query.Select(x => new VOS_PEmployee_View
            {
                ID = x.ID,
                Name_view = x.Area.Name,
                FullName = x.FullName,
                WeChat = x.WeChat,
                TaobaAccount = x.TaobaAccount,
                JDAccount = x.JDAccount,
                AlipayPicId = x.AlipayPicId,
                WeChatPicId = x.WeChatPicId,
                WeChatRealNamePicId = x.WeChatRealNamePicId,
                PEstate = x.PEstate,
                button_show = button_show,
                CreateBy = x.CreateBy,
                CreateTime = x.CreateTime,
                OrganizationName_view = x.Organization.OrganizationName,
                QQAccount = x.QQAccount,
            }).OrderBy(x => x.PEstate).ThenByDescending(x => x.CreateTime.Value);
        }



    }

    public class VOS_PEmployee_View : VOS_PEmployee
    {
        [Display(Name = "名称")]
        public String Name_view { get; set; }

        [Display(Name = "组织机构")]
        public String OrganizationName_view { get; set; }

        public bool MyProperty { get; set; }
    }
}
