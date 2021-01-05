using Microsoft.AspNetCore.Http;
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
                if (vOS_Task.CommodityPic == null || vOS_Task.SearchKeyword == null)
                {
                    return Json("3", 200, "图片或关键字没有！无法派单");
                }
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
                        var a = (vOS_Task.DistributionTime.Value.AddHours(24) - DateTime.Now).Hours + 1;
                        return Json(a);
                    }
                    vOS_Task.OrderState = OrderState.已完成;
                }
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
        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(VOS_TaskListVM vm)
        {
            return vm.GetExportData();
        }
    }
}
