using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.ViewModel.Business.VOS_UserVMs;
using VOS.Model;
using System.Linq;
using VOS.Areas.BaseControllers;

namespace VOS.Controllers
{
    [Area("Business")]
    [ActionDescription("新用户管理")]
    public partial class VOS_UserController : VOS_BaseControllers
    {
        #region Search
        [ActionDescription("Search")]
        public ActionResult Index(string[] IDs = null)
        {
            var vm = CreateVM<VOS_UserListVM>();
            if (IDs.Length > 0 && IDs != null)
            {
                ViewBag.id = string.Join(',', IDs);
                vm.SearcherMode = ListVMSearchModeEnum.Custom1;
            }
            ViewBag.IsShow = IsSuperAdministrator;
            return PartialView(vm);
        }

        [ActionDescription("Search")]
        [HttpPost]
        public string Search(VOS_UserListVM vm)
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
            var vm = CreateVM<VOS_UserVM>();
            ViewBag.IsShow = IsSuperAdministrator;
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(VOS_UserVM vm)
        {
            vm.Entity.IsValid = true;
            if (!IsSuperAdministrator) {
                vm.Entity.DistributionID = GetDistributionID;
            }
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
            var vm = CreateVM<VOS_UserVM>(id);
            ViewBag.IsShow = IsSuperAdministrator;
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(VOS_UserVM vm)
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
            var vm = CreateVM<VOS_UserVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = CreateVM<VOS_UserVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region Details
        [ActionDescription("Details")]
        public ActionResult Details(string id)
        {
            var vm = CreateVM<VOS_UserVM>(id);
            return PartialView(vm);
        }
        #endregion

        #region BatchEdit
        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = CreateVM<VOS_UserBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult DoBatchEdit(VOS_UserBatchVM vm, IFormCollection nouse)
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
            var vm = CreateVM<VOS_UserBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult DoBatchDelete(VOS_UserBatchVM vm, IFormCollection nouse)
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
            var vm = CreateVM<VOS_UserImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Import")]
        public ActionResult Import(VOS_UserImportVM vm, IFormCollection nouse)
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

        #region 修改密码
        [ActionDescription("修改密码")]
        public ActionResult EditPassword1(string id)
        {
            var vm = CreateVM<VOS_UserVM>(id);
            return PartialView(vm);
        }
        [ActionDescription("修改密码")]
        [HttpPost]
        public ActionResult EditPassword1(VOS_UserVM vm)
        {
            try
            {
                var _User = DC.Set<VOS_User>().Where(x => x.ID == vm.Entity.ID).SingleOrDefault();
                _User.Password = Utils.GetMD5String(vm.Entity.Password);
                DC.Set<VOS_User>().Update(_User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                DC.SaveChanges();
                return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
            }
            catch (Exception)
            {
                return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID).Alert("修改密码失败");
            }        
        }

        #endregion
        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(VOS_UserListVM vm)
        {
            return vm.GetExportData();
        }

    }
}
