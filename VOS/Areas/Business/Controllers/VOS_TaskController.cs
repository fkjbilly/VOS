﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.ViewModel.Business.VOS_TaskVMs;
using VOS.Model;
using VOS.ViewModel.Business.VOS_PEmployeeVMs;
using System.Linq;
using VOS.Areas.BaseControllers;

namespace VOS.Controllers
{
    [Area("Business")]
    [ActionDescription("任务管理")]
    public partial class VOS_TaskController : VOS_BaseControllers
    {
        #region Search
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<VOS_TaskListVM>();
            ViewBag.IsShow = IsSuperAdministrator;
            vm.SearcherMode = ListVMSearchModeEnum.Custom2;
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Index(VOS_TaskListVM vm)
        {
            ViewBag.IsShow = IsSuperAdministrator;
            vm.SearcherMode = ListVMSearchModeEnum.Custom2;
            vm.Searcher.MyInitVM();
            return PartialView(vm);
        }

        [ActionDescription("Search")]
        [HttpPost]
        public string Search(VOS_TaskListVM vm)
        {
            if (ModelState.IsValid)
            {
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }
        #endregion

        #region Create
        [ActionDescription("Create")]
        public ActionResult Create()
        {
            var vm = CreateVM<VOS_TaskVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(VOS_TaskVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }
        #endregion

        #region Edit
        [ActionDescription("Edit")]
        public ActionResult Edit(string id)
        {
            var vm = CreateVM<VOS_TaskVM>(id);
            vm.Entity.Task_no = "T" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(VOS_TaskVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }
        #endregion

        #region Delete
        [ActionDescription("Delete")]
        public ActionResult Delete(string id)
        {
            var vm = CreateVM<VOS_TaskVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = CreateVM<VOS_TaskVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return FFResult().Alert("删除失败");
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer?["OprationSuccess"]);
            }
        }
        #endregion

        #region Details
        [ActionDescription("Details")]
        public ActionResult Details(string id)
        {
            var vm = CreateVM<VOS_TaskVM>(id);
            return PartialView(vm);
        }
        #endregion

        #region BatchEdit
        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = CreateVM<VOS_TaskBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult DoBatchEdit(VOS_TaskBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer?["OprationSuccess"]);
            }
        }
        #endregion

        #region BatchDelete
        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = CreateVM<VOS_TaskBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        [ValidateFormItemOnly]
        public ActionResult DoBatchDelete(VOS_TaskBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer?["OprationSuccess"]);
            }
        }
        #endregion

        #region Import
        [ActionDescription("Import")]
        public ActionResult Import()
        {
            var vm = CreateVM<VOS_TaskImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Import")]
        public ActionResult Import(VOS_TaskImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer["ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }
        #endregion

        #region BrushHand 单个分配刷手
        [ActionDescription("分配刷手")]
        public ActionResult BrushHand(string id)
        {
            ViewBag.id = id;
            var vm = CreateVM<VOS_PEmployeeListVM>();
            vm.SearcherMode = ListVMSearchModeEnum.Custom1;
            ViewBag.IsShow = IsSuperAdministrator;
            MemoryCacheHelper.Set_TaskID = id;
            return PartialView(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">任务编号</param>
        /// <param name="BrushHandID">刷手编号</param>
        /// <returns></returns>
        [ActionDescription("分配刷手")]
        [HttpPost]
        public ActionResult BrushHand(Guid ID, Guid BrushHandID)
        {
            try
            {
                var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID == ID).SingleOrDefault();
                if (vOS_Task.IsLock == false)
                {
                    return Json("4", 200, "未解锁无法派单");
                }
                //图片  关键字
                if (string.IsNullOrEmpty(vOS_Task.CommodityPicId.ToString()) || string.IsNullOrEmpty(vOS_Task.SearchKeyword))
                {
                    return Json("3", 200, "图片或关键字没有！无法派单");
                }
                #region 规则管理
                var _vOS_Task = DC.Set<VOS_Task>().AsQueryable();
                var _TaskModel = _vOS_Task.Where(x => x.ID.ToString().Equals(ID.ToString())).SingleOrDefault();
                #region 新类目规则
                var _TaskCateId = _TaskModel.TaskCateId.ToString();
                if (!string.IsNullOrEmpty(_TaskCateId))
                {
                    var _Category = DC.Set<Category>().Where(x => x.ID.ToString() == _TaskCateId).SingleOrDefault();
                    //类目规则《周期》
                    long _Cycle = _Category.Cycle == "" ? 0 : Convert.ToInt64(_Category.Cycle);
                    //类目规则《周期单量》
                    long _Num = _Category.CycleNum == "" ? 0 : Convert.ToInt64(_Category.CycleNum);
                    if (_Cycle != 0 && _Num != 0)
                    {
                        var My_vOS_Task = _vOS_Task.Where(x => x.EmployeeId.ToString().Equals(BrushHandID.ToString()) && x.DistributionTime > DateTime.Now.AddDays(-_Cycle) && x.TaskCateId.ToString().Equals(_TaskCateId)).Count();
                        if (My_vOS_Task >= _Num)
                        {
                            return Json("5", 200, "类目“" + _TaskModel.TaskCate.Name + "”规则未通过！！！");
                        }
                    }
                }
                #endregion

                foreach (var item in RuleCaches() as System.Collections.Generic.List<VOS_Rule>)
                {
                    if (item.IsUse == false)
                    {
                        continue;
                    }
                    #region 转换
                    //规则《周期》
                    long Cycle = item.Cycle == "" ? 0 : Convert.ToInt64(item.Cycle);
                    //规则《单量》
                    long Num = item.Num == "" ? 0 : Convert.ToInt64(item.Num);
                    #endregion

                    switch (item.RuleType)
                    {
                        case VOS_Rule.RuleTypes.店铺:
                            var _vOS_Task1 = _vOS_Task;
                            #region 店铺规则
                            var ShopModel = DC.Set<VOS_Plan>().Where(x => x.ID.ToString().Equals(_TaskModel.PlanId.ToString())).SingleOrDefault();

                            var My_vOS_Task2 = _vOS_Task.Where(x => x.EmployeeId.ToString().Equals(BrushHandID.ToString()) 
                                                                && x.DistributionTime > DateTime.Now.AddDays(-Cycle) 
                                                                && x.Plan.Shopname.ID.ToString().Equals(ShopModel.ShopnameId.ToString())).Count();
                            if (My_vOS_Task2 >= Num)
                            {
                                return Json("5", 200, "店铺“" + ShopModel.Shopname+ "”规则未通过！！！");
                            }
                            #endregion
                            break;
                        case VOS_Rule.RuleTypes.间隔:
                            #region 间隔规则
                            var _vOS_Task2 = _vOS_Task.Where(y => y.DistributionTime > DateTime.Now.AddDays(-Cycle)
                                            && y.EmployeeId.ToString().Equals(BrushHandID.ToString())).Count();
                            if (_vOS_Task2 >= Num)
                            {
                                return Json("5", 200, "间隔规则未通过！！！");
                            }
                            #endregion
                            break;
                        case VOS_Rule.RuleTypes.周期:
                            #region 周期规则
                            var _vOS_Task3 = _vOS_Task.Where(x => x.EmployeeId.ToString().Equals(BrushHandID.ToString()) 
                                                             && x.DistributionTime > DateTime.Now.AddDays(-Cycle)).Count();
                            if (_vOS_Task3 >= Num) { 
                                return Json("5", 200, "周期规则未通过！！！");
                            }
                            #endregion
                            break;
                    }
                }
                #endregion
                #region 规则通过
                if (vOS_Task.ExecutorId == null)
                {
                    //执行人   当前登录人信息
                    vOS_Task.ExecutorId = LoginUserInfo.Id;
                }
                //分配人
                vOS_Task.DistributorId = LoginUserInfo.Id;
                //刷手编号
                vOS_Task.EmployeeId = BrushHandID;
                //任务状态
                vOS_Task.OrderState = OrderState.进行中;
                vOS_Task.DistributionTime = DateTime.Now;
                DC.Set<VOS_Task>().Update(vOS_Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                DC.SaveChanges();
                return Json("1", 200, "已选择刷手");
                #endregion
            }
            catch (Exception)
            {
                return Json("5", 200, "发生错误，联系管理员");
            }
        }
        #endregion

        #region DistributionExecutor 批量分配执行人
        [HttpPost]
        [ActionDescription("批量分配执行人")]
        public bool DistributionExecutor(string ids, Guid id)
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    const string str = "已完成,已返款";
                    string[] IDs = ids.Split(',');//任务编号
                    foreach (var item in IDs)
                    {
                        var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID.ToString() == item).SingleOrDefault();
                        if (str.IndexOf(vOS_Task.OrderState.ToString()) >= 0)
                        {
                            continue;
                        }
                        if (vOS_Task.OrderState == OrderState.未分配)
                        {
                            vOS_Task.OrderState = OrderState.已分配;
                        }
                        vOS_Task.ExecutorId = id;
                        vOS_Task.DistributionTime = DateTime.Now;
                    }

                    DC.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        #endregion

        #region BrushAlone 填写刷单单号或已完成
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="VOrderCode"></param>
        /// <param name="b">true 填写单号并更改状态为已完成</param>
        /// <returns></returns>
        [ActionDescription("填写刷单单号")]
        [HttpPost]
        public ActionResult BrushAlone(Guid ID, string VOrderCode, bool b = false)
        {
            try
            {
                var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID == ID).SingleOrDefault();
                vOS_Task.VOrderCode = VOrderCode;
                if (b == true)
                {
                    if (vOS_Task.TaskType == TaskType.隔天单 && DateTime.Now < vOS_Task.DistributionTime.Value.AddHours(24))
                    {
                        var a = (Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")) - vOS_Task.DistributionTime.Value).Hours;
                        return Json(a);
                    }
                    vOS_Task.OrderState = OrderState.已完成;
                }
                DC.Set<VOS_Task>().Update(vOS_Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                DC.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        #endregion

        #region ResetBrushHand 重新派单
        [ActionDescription("重新派单")]
        [HttpPost]
        [Public]
        public ActionResult ResetBrushHand(Guid ID)
        {
            try
            {
                var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID == ID).SingleOrDefault();
                vOS_Task.EmployeeId = null;
                vOS_Task.OrderState = OrderState.已分配;
                if (DC.SaveChanges() > 0)
                    return Json(true);
                else
                    return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        #endregion

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

        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(VOS_TaskListVM vm)
        {
            return vm.GetExportData();
        }
    }
}
