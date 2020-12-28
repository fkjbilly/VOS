﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;
using VOS.ViewModel.Business.VOS_PEmployeeVMs;

namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskListVM : BasePagedListVM<VOS_Task_View, VOS_TaskSearcher>
    {

        protected override List<GridAction> InitGridAction()
        {
            if (SearcherMode == ListVMSearchModeEnum.MasterDetail)
            {
                return new List<GridAction>();
            }
            else
            {
                return new List<GridAction>
            {
                this.MakeAction("VOS_Task","BrushHand","派单","刷手分配",GridActionParameterTypesEnum.SingleId,"Business",800,600)
                .SetShowInRow(true).SetHideOnToolBar(true).SetBindVisiableColName("OrderStateHide"),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeAction("VOS_User","Index","设置执行人","执行人分配",GridActionParameterTypesEnum.MultiIds,"Business",900,600).SetIconCls("_wtmicon _wtmicon-icon_shezhi"),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
            };
            }
        }


        protected override IEnumerable<IGridColumn<VOS_Task_View>> InitGridHeader()
        {
            if (SearcherMode == ListVMSearchModeEnum.MasterDetail)
            {
                return new List<GridColumn<VOS_Task_View>>
                {
                this.MakeGridHeader(x => x.Task_no),
                this.MakeGridHeader(x => x.TaskType),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CommodityName),
                this.MakeGridHeader(x => x.TBAccount),
                };
            }
            else
            {
                return new List<GridColumn<VOS_Task_View>>{
                this.MakeGridHeader(x => x.Task_no).SetBackGroundFunc((x)=>{
                   if(x.IsLock == false){
                    return "#FFB800";
                    }
                   return "";
                }),
                this.MakeGridHeader(x => x.TaskType),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CommodityName),
                this.MakeGridHeader(x => x.CommodityPrice),
                this.MakeGridHeader(x => x.SearchKeyword),
                this.MakeGridHeader(x => x.SKU),
                this.MakeGridHeader(x => x.FullName_view),
                this.MakeGridHeader(x => x.VOrderCode).SetFormat(VOrderCodeFormat).SetWidth(150),
                this.MakeGridHeader(x=> "OrderStateHide").SetHide().SetFormat((a,b)=>{
                    if(a.OrderState == OrderState.已完成 || a.OrderState == OrderState.已返款 )
                    {
                        return "false";
                    }
                    return "true";
                }),
                this.MakeGridHeader(x => x.OrderState).SetBackGroundFunc((x)=>{
                    switch (x.OrderState)
                    {
                        case OrderState.未分配:
                            return "#009688";
                        case OrderState.已分配:
                          return "#5FB878";
                        case OrderState.进行中:
                            return "#FF5722";
                        case OrderState.已完成:
                           return "#1E9FFF";
                        case OrderState.已返款:
                            return "#CCFF99";
                        default:
                            return "";
                    }
                }).SetForeGroundFunc((x)=>{
                    return "#000000";
                }),
                this.MakeGridHeaderAction(width: 200)
            };
            }
        }

        /// <summary>
        /// 双击文本
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private string VOrderCodeFormat(VOS_Task_View entity, object val)
        {
            switch (entity.OrderState)
            {
                case OrderState.未分配:
                case OrderState.已分配:
                    return "";
                case OrderState.进行中:
                    return "<input type='text' title='双击填写单号' placeholder='双击填写单号' value='"+ entity.VOrderCode + "' readonly data-code='" + entity.ID + "' class='layui-input brushAlone' style='width:150px;' />";
                case OrderState.已完成:
                case OrderState.已返款:
                    return entity.VOrderCode;
                default:
                    return "";
            }
        }

        public override IOrderedQueryable<VOS_Task_View> GetSearchQuery()
        {
            return SelectWhereTask();
        }

        private IOrderedQueryable<VOS_Task_View> SelectWhereTask()
        {
            #region 共有条件
            var query = DC.Set<VOS_Task>()
                .CheckEqual(Searcher.TaskType, x => x.TaskType)
                //.CheckEqual(Searcher.PlanId, x => x.PlanId)//计划编号
                .CheckContain(Searcher.CommodityName, x => x.CommodityName)
                .CheckContain(Searcher.SearchKeyword, x => x.SearchKeyword)
                .CheckEqual(Searcher.IsLock, x => x.IsLock)
                .CheckEqual(Searcher.DistributorId, x => x.DistributorId)
                .CheckEqual(Searcher.EmployeeId, x => x.EmployeeId)
                .CheckContain(Searcher.VOrderCode, x => x.VOrderCode)
                //.CheckContain(Searcher.TBAccount, x => x.TBAccount)
                .Where(x => x.IsValid == true);
            #endregion

            const string list = "超级管理员,管理员,财务管理,财务,会计管理,会计";
            var a = DC.Set<FrameworkUserRole>().Where(x => x.UserId == LoginUserInfo.Id).Select(x => new { x.RoleId }).FirstOrDefault();
            var b = DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == a.RoleId.ToString()).FirstOrDefault();
            if (list.IndexOf(b.RoleName) < 0)
            {
                query = query.Where(x => x.ExecutorId.Equals(LoginUserInfo.Id));
            }

            return query
                    .Select(x => new VOS_Task_View
                    {
                        ID = x.ID,
                        Task_no = x.Task_no,
                        TaskType = x.TaskType,
                        Name_view = x.TaskCate.Name,
                        CommodityName = x.CommodityName,
                        CommodityPrice = x.CommodityPrice,
                        SearchKeyword = x.SearchKeyword,
                        SKU = x.SKU,
                        FullName_view = x.Employee.FullName,
                        VOrderCode = x.VOrderCode,
                        OrderState = x.OrderState,
                        CreateTime = x.CreateTime,
                        IsLock = x.IsLock,
                    })
                    .OrderByDescending(x => x.CreateTime);
        }
       
    }

    public class VOS_Task_View : VOS_Task
    {
        [Display(Name = "计划编号")]
        public String Plan_no_view { get; set; }
        [Display(Name = "类目名称")]
        public String Name_view { get; set; }
        [Display(Name = "刷手")]
        public String FullName_view { get; set; }

    }
}
