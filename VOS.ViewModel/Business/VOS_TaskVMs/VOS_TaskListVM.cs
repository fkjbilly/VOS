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
                this.MakeAction("VOS_Task","BrushHand","分配","刷手分配",GridActionParameterTypesEnum.SingleId,"Business",800,600).SetShowInRow(true).SetHideOnToolBar(true),
                this.MakeStandardAction("VOS_Task", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeAction("VOS_Task","Index","批量执行人","执行人分配",GridActionParameterTypesEnum.MultiIds,"Business",900,600),
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
                this.MakeGridHeader(x => x.Task_no),
                this.MakeGridHeader(x => x.TaskType),
                this.MakeGridHeader(x => x.Plan_no_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.CommodityName),
                this.MakeGridHeader(x => x.Eweight),
                this.MakeGridHeader(x => x.SearchKeyword),
                this.MakeGridHeader(x => x.SKU),
                this.MakeGridHeader(x => x.FullName_view),
                this.MakeGridHeader(x => x.VOrderCode),
                this.MakeGridHeader(x => x.TBAccount),
                this.MakeGridHeader(x => x.OrderState),
                this.MakeGridHeaderAction(width: 200)
            };
            }
        }

        public override IOrderedQueryable<VOS_Task_View> GetSearchQuery()
        {
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
            return query;
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
}
