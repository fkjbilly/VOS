using System;
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

        public VOS_Task_OrderState GetOrderState { get; set; }
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
                this.MakeAction("VOS_Task","BrushHand","分配","刷手分配",GridActionParameterTypesEnum.SingleId,"Business",800,600)
                .SetShowInRow(true).SetHideOnToolBar(true).SetBindVisiableColName("OrderStateHide"),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeAction("VOS_User","Index","批量执行人","执行人分配",GridActionParameterTypesEnum.MultiIds,"Business",900,600),
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
                this.MakeGridHeader(x => x.Plan_no_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CommodityName),
                this.MakeGridHeader(x => x.TBAccount),
                };
            }
            else
            {
                return new List<GridColumn<VOS_Task_View>>{
                this.MakeGridHeader(x => x.Task_no).SetBackGroundFunc((x)=>{
                    if(x.IsLock ==false){
                        return "#FFB800";
                    }
                    return "";
                }).SetForeGroundFunc((x)=>{
                    return "#000000";
                }),
                this.MakeGridHeader(x => x.TaskType),
                this.MakeGridHeader(x => x.Plan_no_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CommodityName),
                this.MakeGridHeader(x => x.Eweight),
                this.MakeGridHeader(x => x.SearchKeyword),
                this.MakeGridHeader(x => x.SKU),
                this.MakeGridHeader(x => x.FullName_view),
                this.MakeGridHeader(x => x.VOrderCode).SetFormat(VOrderCodeFormat).SetWidth(150),
                this.MakeGridHeader(x=> "OrderStateHide").SetHide().SetFormat((a,b)=>{
                    if(a.OrderState == OrderState.未分配 || a.OrderState == OrderState.已分配)
                    {
                        return "true";
                    }
                    return "false";
                }),
                this.MakeGridHeader(x => x.TBAccount),
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
                    return "#000000";//FFFFFF
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
            string str;
            if (!string.IsNullOrEmpty(entity.VOrderCode))
            {
                str = "<input type='text' title='双击更改单号' readonly value='" + entity.VOrderCode + "' data-code='" + entity.ID + "' class='layui-input brushAlone' style='width:150px;' />";
            }
            else
            {
                if (entity.OrderState == OrderState.未分配 || entity.OrderState == OrderState.已分配)
                {
                    str = "";
                }
                else
                {
                    str = "<input type='text' title='双击填写单号' placeholder='双击填写单号' readonly data-code='" + entity.ID + "' class='layui-input brushAlone' style='width:150px;' />";

                }
            }
            return str;
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
                .CheckEqual(Searcher.PlanId, x => x.PlanId)
                .CheckContain(Searcher.CommodityName, x => x.CommodityName)
                .CheckContain(Searcher.SearchKeyword, x => x.SearchKeyword)
                .CheckEqual(Searcher.IsLock, x => x.IsLock)
                .CheckEqual(Searcher.DistributorId, x => x.DistributorId)
                .CheckEqual(Searcher.EmployeeId, x => x.EmployeeId)
                .CheckContain(Searcher.VOrderCode, x => x.VOrderCode)
                .CheckContain(Searcher.TBAccount, x => x.TBAccount)
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
                        Plan_no_view = x.Plan.Plan_no,
                        Name_view = x.TaskCate.Name,
                        CommodityName = x.CommodityName,
                        Eweight = x.Eweight,
                        SearchKeyword = x.SearchKeyword,
                        SKU = x.SKU,
                        FullName_view = x.Employee.FullName,
                        VOrderCode = x.VOrderCode,
                        TBAccount = x.TBAccount,
                        OrderState = x.OrderState,
                    })
                    .OrderBy(x => x.ID);
        }

        public void Task_OrderStateCount()
        {
            var query = SelectWhereTask()
                .GroupBy(x => x.OrderState).Select(x => new { a = x.Key, b = x.Count() }).ToDictionary(x => x.a, x => x.b);
            this.GetOrderState = new VOS_Task_OrderState()
            {
                undistributed = query.ContainsKey(OrderState.未分配) ? query[OrderState.未分配] : 0,
                allocated = query.ContainsKey(OrderState.已分配) ? query[OrderState.已分配] : 0,
                conduct = query.ContainsKey(OrderState.进行中) ? query[OrderState.进行中] : 0,
                Completed = query.ContainsKey(OrderState.已完成) ? query[OrderState.已完成] : 0,
                Refund = query.ContainsKey(OrderState.已返款) ? query[OrderState.已返款] : 0,
            };
        }
    }

    public class VOS_Task_View : VOS_Task
    {
        [Display(Name = "计划编号")]
        public String Plan_no_view { get; set; }
        [Display(Name = "类目名称")]
        public String Name_view { get; set; }
        [Display(Name = "姓名")]
        public String FullName_view { get; set; }

    }

    public class VOS_Task_OrderState
    {
        /// <summary>
        /// 未分配
        /// </summary>
        [Display(Name = "未分配")]
        public int undistributed { get; set; }

        /// <summary>
        /// 已分配
        /// </summary>
        [Display(Name = "已分配")]
        public int allocated { get; set; }

        /// <summary>
        /// 进行中
        /// </summary>
        [Display(Name = "进行中")]
        public int conduct { get; set; }

        /// <summary>
        /// 已完成
        /// </summary>
        [Display(Name = "已完成")]
        public int Completed { get; set; }

        /// <summary>
        /// 已返款
        /// </summary>
        [Display(Name = "已返款")]
        public int Refund { get; set; }

    }
}
