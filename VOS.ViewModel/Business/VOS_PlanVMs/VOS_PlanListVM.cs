using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PlanVMs
{
    public partial class VOS_PlanListVM : BasePagedListVM<VOS_Plan_View, VOS_PlanSearcher>
    {
        /// <summary>
        /// 是否是超级管理员登录
        /// </summary>
        private bool IsSuperAdministrator
        {
            get
            {
                var a = DC.Set<FrameworkUserRole>().Where(x => x.UserId == LoginUserInfo.Id).Select(x => new { x.RoleId }).FirstOrDefault();
                var b = DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == a.RoleId.ToString()).FirstOrDefault();
                if (b.RoleName.Equals("超级管理员"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_Plan", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Plan_View>> InitGridHeader()
        {
            var data = new List<GridColumn<VOS_Plan_View>>{
                this.MakeGridHeader(x => x.Plan_no).SetSort(true),
                this.MakeGridHeader(x => x.ShopName_view),
                this.MakeGridHeader(x => x.PlanSatrtTime),
                this.MakeGridHeader(x => x.PlanEndTime),
                this.MakeGridHeader(x => x.PlanFee),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeaderAction(width: 200)
            };
            if (IsSuperAdministrator)
            {
                data.Insert(data.Count() - 1, this.MakeGridHeader(x => x.OrganizationName_view).SetSort(true));
            }
            return data;
        }

        public override IOrderedQueryable<VOS_Plan_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Plan>()
                .CheckContain(Searcher.Plan_no, x => x.Plan_no)
                .CheckContain(Searcher.ShopName, x => x.Shopname.ShopName)
                .CheckEqual(Searcher.OrganizationID, x => x.OrganizationID)
                .CheckBetween(Searcher.PlanSatrtTime?.GetStartTime(), Searcher.PlanSatrtTime?.GetEndTime(), x => x.PlanSatrtTime, includeMax: false)
                .CheckBetween(Searcher.PlanEndTime?.GetStartTime(), Searcher.PlanEndTime?.GetEndTime(), x => x.PlanEndTime, includeMax: false)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID)
                .Select(x => new VOS_Plan_View
                {
                    ID = x.ID,
                    Plan_no = x.Plan_no,
                    PlanSatrtTime = x.PlanSatrtTime,
                    PlanEndTime = x.PlanEndTime,
                    PlanFee = x.PlanFee,
                    Remark = x.Remark,
                    ShopName_view = x.Shopname.ShopName,
                    OrganizationName_view = x.Organization.OrganizationName,
                })
                .OrderByDescending(x => x.Plan_no);
            return query;
        }

    }

    public class VOS_Plan_View : VOS_Plan
    {
        [Display(Name = "店铺")]
        public String ShopName_view { get; set; }

        [Display(Name = "组织机构")]
        public String OrganizationName_view { get; set; }
    }
}
