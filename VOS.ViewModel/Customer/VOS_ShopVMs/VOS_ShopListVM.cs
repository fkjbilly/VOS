using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Customer.VOS_ShopVMs
{
    public partial class VOS_ShopListVM : BasePagedListVM<VOS_Shop_View, VOS_ShopSearcher>
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
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.Create, Localizer["Create"],"Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.Details, Localizer["Details"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.Import, Localizer["Import"], "Customer", dialogWidth: 800),
                this.MakeStandardAction("VOS_Shop", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Customer"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_Shop_View>> InitGridHeader()
        {
            var data = new List<GridColumn<VOS_Shop_View>>{
                this.MakeGridHeader(x => x.cust_name_view),
                this.MakeGridHeader(x => x.ShopName),
                this.MakeGridHeader(x => x.ShopPlat),
                this.MakeGridHeader(x => x.OpenTime),
                this.MakeGridHeaderAction(width: 200)
            };
            if (IsSuperAdministrator)
            {
                data.Insert(data.Count() - 1, this.MakeGridHeader(x => x.OrganizationName_view));
            }
            return data;
        }

        public override IOrderedQueryable<VOS_Shop_View> GetSearchQuery()
        {
            var query = DC.Set<VOS_Shop>()
                .CheckEqual(Searcher.CustomerId, x=>x.CustomerId)
                .CheckContain(Searcher.ShopName, x=>x.ShopName)
                .CheckEqual(Searcher.ShopPlat, x=>x.ShopPlat)
                .CheckEqual(Searcher.OrganizationID, x=>x.Customer.OrganizationID)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.Customer.OrganizationID)
                .Select(x => new VOS_Shop_View
                {
				    ID = x.ID,
                    cust_name_view = x.Customer.cust_name,
                    ShopName = x.ShopName,
                    ShopPlat = x.ShopPlat,
                    OpenTime = x.OpenTime,
                    OrganizationName_view = x.Customer.Organization.OrganizationName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_Shop_View : VOS_Shop{
        [Display(Name = "客户名称")]
        public String cust_name_view { get; set; }
        [Display(Name = "部门")]
        public String OrganizationName_view { get; set; }
    }
}
