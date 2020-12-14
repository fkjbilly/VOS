using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VOS.Model;


namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeListVM : BasePagedListVM<VOS_PEmployee_View, VOS_PEmployeeSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Create, Localizer["Create"],"Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
                this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], "Business"),
            };
        }


        protected override IEnumerable<IGridColumn<VOS_PEmployee_View>> InitGridHeader()
        {
            return new List<GridColumn<VOS_PEmployee_View>>{
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.FullName),
                this.MakeGridHeader(x => x.WeChat),
                this.MakeGridHeader(x => x.TaobaAccount),
                this.MakeGridHeader(x => x.JDAccount),
                this.MakeGridHeader(x => x.AlipayPicId).SetFormat(AlipayPicIdFormat),
                this.MakeGridHeader(x => x.WeChatPicId).SetFormat(WeChatPicIdFormat),
                this.MakeGridHeader(x => x.WeChatRealNamePicId).SetFormat(WeChatRealNamePicIdFormat),
                this.MakeGridHeader(x => x.PEstate),
                this.MakeGridHeaderAction(width: 200)
            };
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
            var query = DC.Set<VOS_PEmployee>()
                .CheckContain(Searcher.FullName, x=>x.FullName)
                .CheckContain(Searcher.Mobile, x=>x.Mobile)
                .CheckContain(Searcher.WeChat, x=>x.WeChat)
                .CheckContain(Searcher.TaobaAccount, x=>x.TaobaAccount)
                .CheckContain(Searcher.JDAccount, x=>x.JDAccount)
                .CheckEqual(Searcher.PEstate, x=>x.PEstate)
                .Select(x => new VOS_PEmployee_View
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
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class VOS_PEmployee_View : VOS_PEmployee{
        [Display(Name = "名称")]
        public String Name_view { get; set; }

    }
}
