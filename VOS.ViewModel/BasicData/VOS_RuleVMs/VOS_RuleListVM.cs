using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RuleVMs
{
    public partial class VOS_RuleListVM : BasePagedListVM<VOS_Rule_View, VOS_RuleSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("Login","Login","刷新","", GridActionParameterTypesEnum.NoId).SetOnClickScript("Refresh();"),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.Create, Localizer["Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.Details, Localizer["Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.Import, Localizer["Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("VOS_Rule", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Rule_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Rule_View>>{
                this.MakeGridHeader(x => x.RuleName),
                this.MakeGridHeader(x => x.RuleType),
                this.MakeGridHeader(x => x.Num),
                this.MakeGridHeader(x => x.Cycle),
                this.MakeGridHeader(x => x.IsUse),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Rule_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Rule>()
                .CheckContain(Searcher.RuleName, x=>x.RuleName)
                .CheckEqual(Searcher.RuleType, x=>x.RuleType)
                .CheckEqual(Searcher.IsUse, x=>x.IsUse)
                .Select(x => new VOS_Rule_View
                {
				    ID = x.ID,
                    RuleName = x.RuleName,
                    RuleType = x.RuleType,
                    Num = x.Num,
                    Cycle = x.Cycle,
                    IsUse = x.IsUse,
                    Remark = x.Remark,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Rule_View : VOS_Rule{

    }
}
