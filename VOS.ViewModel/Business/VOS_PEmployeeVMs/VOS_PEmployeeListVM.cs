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

namespace VOS.ViewModel.Business.VOS_PEmployeeVMs
{
    public partial class VOS_PEmployeeListVM : BasePagedListVM<VOS_PEmployee_View, VOS_PEmployeeSearcher>
    {
        /// <summary>
        /// 是否是管理员登录
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
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Details, Localizer["Details"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "Business", dialogWidth: 800),
                    this.MakeStandardAction("VOS_PEmployee", GridActionStandardTypesEnum.Import, Localizer["Import"], "Business", dialogWidth: 800),
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
                        return "#000000";
                    }),
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
                    }),
                    this.MakeGridHeader(x => x.CreateBy),
                    this.MakeGridHeader(x => x.CreateTime),
                    this.MakeGridHeaderAction(width: 200)
                };
            }
            if (IsSuperAdministrator)
            {
                data.Insert(data.Count() - 1, this.MakeGridHeader(x => x.OrganizationName_view));
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
                   .CheckContain(Searcher.WeChat, x => x.WeChat)
                   .CheckContain(Searcher.TaobaAccount, x => x.TaobaAccount)
                   .CheckContain(Searcher.JDAccount, x => x.JDAccount)
                   .CheckEqual(Searcher.PEstate, x => x.PEstate)
                   .CheckContain(Searcher.CreateBy, x => x.CreateBy)
                   .CheckEqual(Searcher.OrganizationID, x => x.OrganizationID)
                   .CheckBetween(Searcher.StartTime, Searcher.EndTime, x => x.CreateTime, includeMax: false)
                   .DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID);
            #endregion
            if (SearcherMode == ListVMSearchModeEnum.Custom1)
            {
                query = query.Where(x => x.PEstate == state.正常);
                var _Task_EmployeeId = DC.Set<VOS_Task>().Where(x => x.ID.ToString().Equals(MemoryCacheHelper.Set_TaskID)).FirstOrDefault().EmployeeId;
                if (_Task_EmployeeId != null)
                {//已分配
                    query = query.Where(x => x.ID.ToString().Equals(_Task_EmployeeId.ToString()));
                    button_show = true;
                }
                else
                {//未分配
                    #region 规则
                    foreach (var item in RuleCaches() as List<VOS_Rule>)
                    {
                        //规则未启用
                        if (item.IsUse == false)
                        {
                            continue;
                        }
                        var _vOS_Task = DC.Set<VOS_Task>().AsQueryable();
                        #region 转换
                        //规则《周期》
                        long Cycle = item.Cycle == "" ? 0 : Convert.ToInt64(item.Cycle);
                        //规则《单量》
                        long Num = item.Num == "" ? 0 : Convert.ToInt64(item.Num);
                        #endregion
                        switch (item.RuleType)
                        {
                            case RuleTypes.类目:
                                #region 类目规则
                                //类目单量
                                string _CycleNum = "";
                                var _Category = DC.Set<Category>().AsQueryable();
                                //时间：任务分配时间要大于规则《类目规则》周期时间                  
                                var _TaskCateId = _vOS_Task.Where(x => x.DistributionTime > DateTime.Now.AddDays(-Cycle) && x.ID.ToString() == MemoryCacheHelper.Set_TaskID).SingleOrDefault();
                                if (_TaskCateId != null)
                                {
                                    _CycleNum = DC.Set<Category>().Where(x => x.ID.Equals(_TaskCateId.TaskCateId)).SingleOrDefault().CycleNum;
                                }
                                var CycleNum = _CycleNum == "" ? -1 : Convert.ToInt64(_CycleNum);
                                //使用以VOS_Task为主表联查
                                var _data_Category = from _Task in _vOS_Task
                                                     join q in query on _Task.EmployeeId equals q.ID
                                                     select new
                                                     {
                                                         _Task.TaskCateId,//类目
                                                         q.ID//刷手编号
                                                     };
                                query = query.Where(x =>
                                   CycleNum < 0 ? _data_Category.Where(y => x.ID.Equals(y.ID)).Count() < Num
                                   : _vOS_Task.Where(y => y.EmployeeId.Equals(x.ID)).Count() < CycleNum);
                                #endregion
                                break;
                            case RuleTypes.店铺:
                                #region 店铺规则
                                var data_vOS_Task = from _Task in _vOS_Task
                                                    join _Shop in DC.Set<VOS_Plan>() on _Task.PlanId equals _Shop.ID
                                                    where _Task.EmployeeId != null
                                                    select new
                                                    {
                                                        _Task.DistributionTime,
                                                        _Task.EmployeeId,
                                                        _Shop.Shopname,
                                                        _Shop.Plan_no,
                                                    };
                                //时间：任务分配时间要大于规则《周期规则》周期时间
                                query = query.Where(x => data_vOS_Task.Where(y => y.DistributionTime > DateTime.Now.AddDays(-Cycle) && x.ID.Equals(y.EmployeeId)).Count() < Num);
                                #endregion
                                break;
                            case RuleTypes.间隔:
                                #region 间隔规则
                                _vOS_Task = _vOS_Task.Where(x =>
                                    //时间：任务分配时间要大于规则《间隔规则》周期时间
                                    x.DistributionTime > DateTime.Now.AddDays(-Cycle));
                                query = query.Where(x =>
                                  _vOS_Task.Where(y => y.EmployeeId.Equals(x.ID)).Count() < 1);
                                #endregion
                                break;
                            case RuleTypes.周期:
                                #region 周期规则
                                //时间：任务分配时间要大于规则《周期规则》周期时间
                                _vOS_Task = _vOS_Task.Where(x => x.DistributionTime > DateTime.Now.AddDays(-Cycle));
                                query = query.Where(x => _vOS_Task.Where(y => y.EmployeeId.Equals(x.ID)).Count() < Num);
                                #endregion
                                break;
                        }
                    }
                    #endregion
                }
            }
            else
            {
                const string list = "超级管理员,管理员,财务管理,财务,会计管理,会计";
                var a = DC.Set<FrameworkUserRole>().Where(x => x.UserId == LoginUserInfo.Id).Select(x => new { x.RoleId }).FirstOrDefault();
                var b = DC.Set<FrameworkRole>().Where(x => x.ID.ToString() == a.RoleId.ToString()).FirstOrDefault();
                if (list.IndexOf(b.RoleName) < 0)
                {
                    query = query.Where(x => x.CreateBy.Equals(LoginUserInfo.ITCode));
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
            }).OrderByDescending(x => x.CreateTime);
        }

        private object RuleCaches()
        {
            string key = MemoryCacheHelper._RuleCaches;
            if (MemoryCacheHelper.Exists(key))
            {
                return MemoryCacheHelper.Get(key);
            }
            else
            {
                var result = DC.Set<VOS_Rule>().ToList();
                MemoryCacheHelper.Set(key, result, new TimeSpan(4, 0, 0));
                return result;
            }
        }
    }

    public class VOS_PEmployee_View : VOS_PEmployee
    {
        [Display(Name = "名称")]
        public String Name_view { get; set; }

        [Display(Name = "组织机构")]
        public String OrganizationName_view { get; set; }


    }
}
