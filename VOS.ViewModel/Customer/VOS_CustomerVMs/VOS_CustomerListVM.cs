using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_CustomerVMs
{
    public partial class VOS_CustomerListVM : BasePagedListVM<VOS_Customer_View, VOS_CustomerSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.Create, Localizer["Create"],"Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.Details, Localizer["Details"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.Import, Localizer["Import"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Customer", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Customer"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Customer_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_Customer_View>>{
                this.MakeGridHeader(x => x.cust_name),
                this.MakeGridHeader(x => x.cust_level),
                this.MakeGridHeader(x => x.cust_telephone),
                this.MakeGridHeader(x => x.cust_flag),
                this.MakeGridHeader(x => x.link_name),
                this.MakeGridHeader(x => x.link_mobile),
                this.MakeGridHeader(x => x.OrganizationName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<VOS_Customer_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Customer>()
                .CheckContain(Searcher.cust_name, x=>x.cust_name)
                .CheckEqual(Searcher.cust_regionId, x=>x.cust_regionId)
                .CheckEqual(Searcher.cust_level, x=>x.cust_level)
                .CheckEqual(Searcher.cust_flag, x=>x.cust_flag)
                .CheckContain(Searcher.link_name, x=>x.link_name)
                .CheckContain(Searcher.link_mobile, x=>x.link_mobile)
                .CheckEqual(Searcher.OrganizationID,x=>x.OrganizationID)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID)
                .Select(x => new VOS_Customer_View
                {
				    ID = x.ID,
                    cust_name = x.cust_name,
                    cust_level = x.cust_level,
                    cust_telephone = x.cust_telephone,
                    cust_flag = x.cust_flag,
                    link_name = x.link_name,
                    link_mobile = x.link_mobile,
                    OrganizationName_view = x.Organization.OrganizationName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Customer_View : VOS_Customer{
        [Display(Name = "部门")]
        public String OrganizationName_view { get; set; }
    }
}
