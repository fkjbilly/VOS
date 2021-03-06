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
using System.Data.Common;

namespace VOS.ViewModel.Business.VOS_TaskVMs
{
    public partial class VOS_TaskListVM : BasePagedListVM<VOS_Task_View, VOS_TaskSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            if (SearcherMode != ListVMSearchModeEnum.MasterDetail)
            {
                return new List<GridAction>
                {
                    this.MakeAction("VOS_Task","BrushHand","更改","更改信息",GridActionParameterTypesEnum.SingleId,"Business",800,600)
                    .SetShowInRow(true).SetHideOnToolBar(true).SetBindVisiableColName("OrderStateShow"),

                    this.MakeAction("VOS_Task","BrushHand","派单","会员分配",GridActionParameterTypesEnum.SingleId,"Business",800,600)
                    .SetShowInRow(true).SetHideOnToolBar(true).SetBindVisiableColName("OrderStateHide"),

                    this.MakeAction("VOS_Task","BatchCreation","批量创建","批量创建任务",GridActionParameterTypesEnum.NoId,"Business",1000,600).SetMax().SetShowInRow(false).SetHideOnToolBar(false).SetIconCls("layui-icon layui-icon-find-fill"),
                    this.MakeAction("VOS_Task","DistributionOrganization","分配组织机构","分配机构",GridActionParameterTypesEnum.MultiIds,"Business",800,600).SetShowInRow(false).SetHideOnToolBar(false).SetIconCls("_wtmicon _wtmicon-guanfangbanben"),

                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                    this.MakeAction("VOS_User","Index","设置执行人","执行人分配",GridActionParameterTypesEnum.MultiIds,"Business",900,600).SetIconCls("_wtmicon _wtmicon-icon_shezhi"),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800).SetShowInRow(false).SetHideOnToolBar(true),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
                };
            }
            else
            {
                return null;
            }
        }

        protected override IEnumerable<IGridColumn<VOS_Task_View>> InitGridHeader()
        {
            return SearcherMode switch
            {
                _ when SearcherMode == ListVMSearchModeEnum.MasterDetail =>
                    new List<GridColumn<VOS_Task_View>>{
                    this.MakeGridHeader(x => x.Task_no),
                    this.MakeGridHeader(x => x.TaskType).SetWidth(90),
                    this.MakeGridHeader(x => x.Name_view).SetWidth(90),
                    this.MakeGridHeader(x => x.CommodityName),
                    this.MakeGridHeader(x => x.DistributionTime),
                    this.MakeGridHeader(x=>x.executorName).SetWidth(100),
                    },
                _ when SearcherMode == ListVMSearchModeEnum.CheckExport || SearcherMode ==
                ListVMSearchModeEnum.Export => new List<GridColumn<VOS_Task_View>>{
                    this.MakeGridHeader(x => x.method),
                    this.MakeGridHeader(x => x.ShopName),
                    this.MakeGridHeader(x => x.DoubleCommodityPrice),
                    this.MakeGridHeader(x => x.keyword),
                    this.MakeGridHeader(x => x.SKU),
                    this.MakeGridHeader(x => x.Wangwang),
                    this.MakeGridHeader(x => x.OddNumbers),
                    this.MakeGridHeader(x => x.executorName),
                    this.MakeGridHeader(x => x.OrderState),
                    this.MakeGridHeader(x => x.CompleteTime),
                },
                _ => new List<GridColumn<VOS_Task_View>>{
                    this.MakeGridHeader(x => x.TaskType).SetForeGroundFunc((x)=>{
                        string color =  x.TaskType  switch
                            {
                               TaskType.搜索=>"#e54d42",
                               TaskType.隔天=>"#45b97c",
                               TaskType.非搜=>"#8dc63f",
                               TaskType.动销=>"#9c26b0",
                               TaskType.其他=>"#c2c2c2",
                                          _ =>"#8799a3"
                            };
                         return color;
                    }).SetSort(true).SetWidth(90),
                    this.MakeGridHeader(x => x.ShopName).SetWidth(100),
                    this.MakeGridHeader(x => x.DoubleCommodityPrice).SetSort(true).SetShowTotal(true).SetWidth(110),
                    this.MakeGridHeader(x => x.SearchKeyword),
                    this.MakeGridHeader(x => x.SKU),
                    this.MakeGridHeader(x => x.CommodityPicId).SetFormat(CommodityPicIdFormat).SetWidth(90),
                    this.MakeGridHeader(x => x.FullName_view),
                    this.MakeGridHeader(x => x.VOrderCode).SetFormat(VOrderCodeFormat).SetWidth(110),
                    this.MakeGridHeader(x=> "OrderStateHide").SetHide().SetFormat((a,b)=>{
                        if(a.OrderState == OrderState.已完成 || a.OrderState == OrderState.已返款 )
                        {
                            return "false";
                        }
                        return "true";
                    }),
                    this.MakeGridHeader(x=> "OrderStateShow").SetHide().SetFormat((a,b)=>{
                        if(a.OrderState == OrderState.已完成 || a.OrderState == OrderState.已返款 )
                        {
                            return "true";
                        }
                        return "false";
                    }),
                    this.MakeGridHeader(x => x.DoubleOtherExpenses).SetSort(true).SetShowTotal(true).SetWidth(110),
                    this.MakeGridHeader(x=>x.executorName).SetWidth(80),
                    this.MakeGridHeader(x => x.OrderState).SetBackGroundFunc((x)=>{
                     string color =  x.OrderState  switch
                        {
                             OrderState.未分配=>"#009688",
                             OrderState.已分配=>"#5FB878",
                             OrderState.进行中=>"#FF5722",
                             OrderState.已完成=>"#1E9FFF",
                             OrderState.已返款=>"#CCFF99",
                                             _=>""
                        };
                        return color;
                    }).SetForeGroundFunc((x)=>{
                        return "#000000";
                    }).SetWidth(102).SetSort(true),
                    this.MakeGridHeaderAction(width: 165),
                }
            };
        }

        private string VOrderCodeFormat(VOS_Task_View entity, object val)
        {
            //非批量操作模式
            if (SearcherMode != ListVMSearchModeEnum.Batch)
            {
                return entity.OrderState switch
                {
                    _ when entity.OrderState == OrderState.进行中 => "<input type='text' title='双击填写单号' placeholder='双击填写单号' value='" + entity.VOrderCode + "' readonly data-code='" + entity.ID + "' class='layui-input brushAlone' style='width:150px;' />",
                    _ => entity.VOrderCode
                };
            }
            else
            {
                return entity.VOrderCode;
            }

        }

        public override IOrderedQueryable<VOS_Task_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Task>()
            .CheckEqual(Searcher.TaskType, x => x.TaskType)
            .CheckContain(Searcher.CommodityName, x => x.CommodityName)
            .CheckContain(Searcher.SearchKeyword, x => x.SearchKeyword)
            .CheckEqual(Searcher.EmployeeId, x => x.EmployeeId)
            .CheckContain(Searcher.VOrderCode, x => x.VOrderCode)
            .CheckEqual(Searcher.OrderState, x => x.OrderState)
            .CheckContain(Searcher.ShopNames, x => x.Plan.Shopname.ID)
            .CheckContain(Searcher.newShopName, x => x.Plan.Shopname.ShopName)
            .CheckEqual(Searcher.OrganizationID, x => x.Plan.OrganizationID)
            .CheckBetween(Searcher.Time?.GetStartTime(), Searcher.Time?.GetEndTime(), x => x.ImplementStartTime, includeMax: false)
            .CheckContain(Searcher.Member, x => x.Employee.FullName)
            .CheckContain(Searcher.ExecutorName, x => x.Executor.Name)
            .Where(x => x.IsValid == true);
            if (SearcherMode != ListVMSearchModeEnum.MasterDetail)
            {
                if (ExpandVM.NoContainRoles(this, LoginUserInfo.Id))
                {
                    query = query.Where(x => x.ExecutorId.Equals(LoginUserInfo.Id));
                }
                else
                {
                    query = query.DPWhere(LoginUserInfo.DataPrivileges, x => x.Plan.OrganizationID);
                }
            }
            var data = query.Select(x => new VOS_Task_View
            {
                ID = x.ID,
                Task_no = x.Task_no,
                TaskType = x.TaskType,
                Name_view = x.TaskCate.Name,
                CommodityName = x.CommodityName,
                DoubleCommodityPrice = Convert.ToDouble(x.CommodityPrice),
                SearchKeyword = x.SearchKeyword,
                SKU = x.SKU,
                FullName_view = x.Employee.TaobaAccount,
                VOrderCode = x.VOrderCode,
                OrderState = x.OrderState,
                CreateTime = x.CreateTime,
                IsLock = x.IsLock,
                ShopName = x.Plan.Shopname.ShopName,
                executorName = x.Executor.Name,
                DoubleOtherExpenses = Convert.ToDouble(x.OtherExpenses),
                CommodityPicId = x.CommodityPicId,
                DistributionTime = x.DistributionTime,
                CompleteTime = x.CompleteTime,
                method = x.TaskType,
                keyword = x.SearchKeyword,
                OddNumbers = x.VOrderCode,
                Wangwang = x.Employee.TaobaAccount,
            });

            if (SearcherMode == ListVMSearchModeEnum.MasterDetail)
            {
                return data.OrderByDescending(x => x.DistributionTime.Value);
            }
            else
            {
                //return data.OrderBy(x => x.OrderState == OrderState.已分配 ? 0 : x.OrderState == OrderState.未分配 ? 1 : x.OrderState == OrderState.进行中 ? 2 : x.OrderState == OrderState.已完成 ? 3 : 4);
                return data.OrderBy(x => x.OrderState == 0 ? 1 : (int)x.OrderState == 1 ? 0 : (int)x.OrderState );
            }
        }

        private List<ColumnFormatInfo> CommodityPicIdFormat(VOS_Task_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                //ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.CommodityPicId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.CommodityPicId,640,480),
            };
        }
    }

    public class VOS_Task_View : VOS_Task
    {
        [Display(Name = "计划编号")]
        public String Plan_no_view { get; set; }
        [Display(Name = "类目名称")]
        public String Name_view { get; set; }
        [Display(Name = "会员旺旺号")]
        public String FullName_view { get; set; }
        [Display(Name = "店铺")]
        public String ShopName { get; set; }
        [Display(Name = "执行人")]
        public String executorName { get; set; }

        [Display(Name = "商品价格")]
        public double DoubleCommodityPrice { get; set; }

        [Display(Name = "其它费用")]
        public double DoubleOtherExpenses { get; set; }

        [Display(Name = "方法")]
        public TaskType method { get; set; }
        [Display(Name = "关键字")]
        public String keyword { get; set; }
        [Display(Name = "旺旺号")]
        public String Wangwang { get; set; }
        [Display(Name = "单号")]
        public String OddNumbers { get; set; }

    }
}