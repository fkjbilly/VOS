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
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using VOS.ViewModel.Business.VOS_PlanVMs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Index(VOS_TaskListVM vm)
        {
            ViewBag.IsShow = IsSuperAdministrator;
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
            //vm.Entity.Task_no = "T" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
            try
            {
                var _task = DC.Set<VOS_Task>().Where(x => x.ID == vm.Entity.ID).FirstOrDefault();
                _task.TaskType = vm.Entity.TaskType;
                _task.PlanId = vm.Entity.PlanId;
                _task.ComDis = vm.Entity.ComDis;
                _task.CustomerService = vm.Entity.CustomerService;
                _task.Contact = vm.Entity.Contact;
                _task.ShopCharge = vm.Entity.ShopCharge;
                _task.ShopChargeContact = vm.Entity.ShopChargeContact;
                _task.ImplementStartTime = vm.Entity.ImplementStartTime;
                _task.ImplementEndTime = vm.Entity.ImplementEndTime;
                _task.TaskCateId = vm.Entity.TaskCateId;
                _task.CommodityName = vm.Entity.CommodityName;
                _task.CommodityPicId = vm.Entity.CommodityPicId;
                _task.CommodityLink = vm.Entity.CommodityLink;
                _task.Eweight = vm.Entity.Eweight;
                _task.TaskFen = vm.Entity.TaskFen;
                _task.CommodityPrice = vm.Entity.CommodityPrice;
                _task.Commission = vm.Entity.Commission;
                _task.OtherExpenses = vm.Entity.OtherExpenses;
                _task.ORequirement = vm.Entity.ORequirement;
                _task.TRequirement = vm.Entity.TRequirement;
                _task.CRemarks = vm.Entity.CRemarks;
                _task.AreaRequirement = vm.Entity.AreaRequirement;
                _task.IsTP = vm.Entity.IsTP;
                _task.SearchKeyword = vm.Entity.SearchKeyword;
                _task.DealKeyword = vm.Entity.DealKeyword;
                _task.SKU = vm.Entity.SKU;
                DC.Set<VOS_Task>().Update(_task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                DC.SaveChanges();
                return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
            }
            catch (Exception)
            {
                return FFResult().CloseDialog().Alert("修改失败");
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
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var _CommodityPicId = GetAppointValue(vm.FC, "LinkedVM.CommodityPicId").ToString();
                    var Ids = GetAppointValue(vm.FC, "Ids").ToString().Split(',');
                    foreach (var item in Ids)
                    {
                        var _task = DC.Set<VOS_Task>().Where(x => x.ID.ToString() == item).FirstOrDefault();
                        _task.CommodityPicId = new Guid(_CommodityPicId);
                        DC.SaveChanges();
                    }
                    transaction.Commit();
                    return FFResult().CloseDialog().RefreshGrid().Alert("已批量修改图片");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return FFResult().CloseDialog().RefreshGrid().Alert("批量修改图片失败");
                }
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

        #region BrushHand 单个分配会员
        [ActionDescription("分配会员")]
        public ActionResult BrushHand(string id)
        {
            ViewBag.id = id;
            var vm = CreateVM<VOS_PEmployeeListVM>();
            //分配会员模式
            vm.SearcherMode = ListVMSearchModeEnum.Custom1;
            ViewBag.IsShow = IsSuperAdministrator;
            MemoryCacheHelper.Set_TaskID = id;
            return PartialView(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">任务编号</param>
        /// <param name="BrushHandID">会员编号</param>
        /// <returns></returns>
        [ActionDescription("分配会员")]
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
                                return Json("5", 200, "店铺“" + ShopModel.Shopname + "”规则未通过！！！");
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
                            if (_vOS_Task3 >= Num)
                            {
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
                //会员编号
                vOS_Task.EmployeeId = BrushHandID;
                //任务状态
                vOS_Task.OrderState = OrderState.进行中;
                vOS_Task.DistributionTime = DateTime.Now;
                DC.Set<VOS_Task>().Update(vOS_Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                int count = DC.SaveChanges();
                return Json("1", 200, "已选择会员");
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
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        #endregion

        #region BrushAlone 填写订单号或已完成
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="VOrderCode"></param>
        /// <param name="b">true 填写单号并更改状态为已完成</param>
        /// <returns></returns>
        [ActionDescription("填写订单号")]
        [HttpPost]
        public ActionResult BrushAlone(Guid ID, string VOrderCode, bool b = false)
        {
            try
            {
                var returnjson = Post(VOrderCode);
                if (returnjson.IndexOf("\"data\":\"yes\"") > 0)
                {
                    //return Json(new { Msg = "淘客订单，不允许完成", State = 5 });
                }
                var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID == ID).SingleOrDefault();
                vOS_Task.VOrderCode = VOrderCode;
                if (b == true)
                {
                    if (vOS_Task.TaskType == TaskType.隔天 && DateTime.Now < vOS_Task.DistributionTime.Value.AddHours(8))
                    {
                        var a = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")) - DateTime.Now;
                        return Json(new { Msg = "隔天单！请" + a.Hours + "小时" + (a.Minutes + 2) + "分钟后提交完成", State = 4 });
                    }
                    vOS_Task.OrderState = OrderState.已完成;
                    vOS_Task.CompleteTime = DateTime.Now;
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

        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public string Sha1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        #region Post请求
        public string Post(string ordercode)
        {
            var appId = "607fd31315c5b";
            var appSecret = "c6621a7abea6cd5f7e49441486726aa738122ec5";
            var timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
            var Url = "https://www.taofake.com/api/account/tbkorder";

            var paramsign = Sha1(ordercode + appId + appSecret + timestamp);

            Url = Url + "?order_id=" + ordercode + "&sign=" + paramsign + "&app_id=" + appId + "&timestamp=" + timestamp;

            //创建Web访问对象
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/json";
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;

            //获取接口返回值
            //通过Web访问对象获取响应内容
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
            string returnjson = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
            reader.Close();
            myResponse.Close();
            return returnjson;

        }
        #endregion
        private object RuleCaches()
        {
            string key = MemoryCacheHelper.GetRuleCaches;
            if (MemoryCacheHelper.Exists(key))
            {
                return MemoryCacheHelper.Get(key);
            }
            else
            {
                var result = DC.Set<VOS_Rule>().ToList();
                MemoryCacheHelper.Set(key, result, new TimeSpan(7, 0, 0, 0));
                return result;
            }
        }

        #region DistributionOrganization 分配机构
        [ActionDescription("分配组织机构页面")]
        public ActionResult DistributionOrganization(string[] IDs)
        {
            var vm = CreateVM<VOS_TaskBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("分配组织机构操作")]
        public ActionResult DoDistributionOrganization(VOS_TaskBatchVM vm)
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    string OrganizationID = GetAppointValue(vm.FC, "LinkedVM.OrganizationID").ToString();
                    Guid guid = new Guid(OrganizationID);
                    var ids = vm.Ids.ToList();
                    List<string> _PlanId = DC.Set<VOS_Task>().Where(x => ids.Contains(x.ID.ToString())).Select(x => x.PlanId.ToString()).ToList();
                    foreach (var item in _PlanId)
                    {
                        var _Plan = DC.Set<VOS_Plan>().Where(x => x.ID.ToString() == item).SingleOrDefault();
                        _Plan.OrganizationID = guid;
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    return FFResult().CloseDialog().RefreshGrid().Alert("组织已分配");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return FFResult().CloseDialog().RefreshGrid().Alert("组织分配失败");
                }
            }
        }
        #endregion

        #region BatchCreation 批量创建相关

        [ActionDescription("批量创建页面")]
        public ActionResult BatchCreation()
        {
            var vm = CreateVM<VOS_PlanVM>();
            vm.Entity.Plan_no = "P" + DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.Entity.PlanSatrtTime = DateTime.Now;
            vm.Entity.PlanEndTime = DateTime.Now.AddDays(+1);
            ViewBag.IsShow = IsSuperAdministrator;
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量创建操作")]
        public async Task<ActionResult> DoBatchCreation(string plan, string tasklist)
        {
            
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var _plan = JsonConvert.DeserializeObject<VOS_Plan>(plan);
                    if (_plan.OrganizationID == null && GetOrganizationID == null)
                    {
                        return Json(new { Msg = GetMsg, icon = 5 });
                    }
                    _plan.OrganizationID = IsSuperAdministrator ? _plan.OrganizationID : (Guid)GetOrganizationID;
                    _plan.CreateTime = DateTime.Now;
                    _plan.CreateBy = LoginUserInfo.ITCode;
                    _plan.IsValid = true;
                    await DC.Set<VOS_Plan>().AddAsync(_plan);
                    await DC.SaveChangesAsync();
                    VOS_Collection _CollectionObj = new VOS_Collection()
                    {
                        CreateTime = DateTime.Now,
                        CreateBy = LoginUserInfo.ITCode,
                        IsValid = true,
                        Plan_noId = _plan.ID,
                        CollectionState = CollectionState.未到账,
                        Collection = _plan.PlanFee,
                    };
                    await DC.Set<VOS_Collection>().AddAsync(_CollectionObj);
                    await DC.SaveChangesAsync();
                    var _Task = JsonConvert.DeserializeObject<List<VOS_Task>>(tasklist);
                    var _PlanId = _plan.ID;
                    foreach (var item in _Task)
                    {
                        if (item.VOS_Number > 1)
                        {
                            for (int i = 0; i < item.VOS_Number; i++)
                            {
                                await Insert_Task(item, _PlanId, true, i);
                            }
                        }
                        else
                        {
                            await Insert_Task(item, _PlanId);
                        }
                    }
                    transaction.Commit();
                    return Json(new { Msg = "已完成批量创建", icon = 1 });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { Msg = "批量创建有误", icon = 5 });
                }
            }

        }

        [HttpGet]
        [ActionDescription("获取类目")]
        public JsonResult GetCategory()
        {
            try
            {
                var _Category = DC.Set<Category>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name).Select(x => new { x.Value, x.Text }).ToList();
                return Json(_Category, 200, "true");
            }
            catch (Exception)
            {
                return Json("", 200, "false");
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <param name="_PlanId">计划编号</param>
        /// <param name="IsMultiple">是否多个添加</param>
        /// <param name="record">【IsMultiple：true】重新赋值任务编号</param>
        /// <returns></returns>
        private async Task Insert_Task(VOS_Task task, Guid _PlanId, bool IsMultiple = false, int record = 0)
        {
            VOS_Task _Taskr = new VOS_Task();
            string _Task_no = "T" + DateTime.Now.ToString("MMdd") + task.Task_no;
            _Taskr.CommodityLink = task.CommodityLink;
            _Taskr.CommodityName = task.CommodityName;
            _Taskr.CommodityPrice = task.CommodityPrice;
            _Taskr.ImplementStartTime = task.ImplementStartTime;
            _Taskr.SKU = task.SKU;
            _Taskr.TaskType = task.TaskType;
            _Taskr.Task_no = IsMultiple ? _Task_no + (record + 1) : _Task_no;
            _Taskr.TaskCateId = task.TaskCateId;
            _Taskr.CommodityPicId = new Guid(task.CommodityPicID);
            _Taskr.SearchKeyword = task.SearchKeyword;
            _Taskr.ComDis = "/";
            _Taskr.Commission = "1";
            _Taskr.OtherExpenses = "1";
            _Taskr.PlanId = _PlanId;
            _Taskr.CreateBy = LoginUserInfo.ITCode;
            _Taskr.CreateTime = DateTime.Now;
            _Taskr.IsValid = true;
            _Taskr.IsLock = true;
            _Taskr.UnlockerId = LoginUserInfo.Id;
            _Taskr.UnlockTime = DateTime.Now;
            await DC.Set<VOS_Task>().AddAsync(_Taskr);
            await DC.SaveChangesAsync();
        }

        [HttpPost]
        [ActionDescription("保存图片")]
        public JsonResult ImgSave()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    string imgPath = Directory.GetCurrentDirectory() + "\\VOSImg\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                    IFormFile fileImage = Request.Form.Files["file"];
                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }
                    imgPath = imgPath + fileImage.FileName;
                    using (FileStream filestream = System.IO.File.Create(imgPath))
                    {
                        fileImage.CopyTo(filestream);
                        filestream.Flush();
                    }
                    FileAttachment file = new FileAttachment();
                    file.CreateTime = DateTime.Now;
                    file.CreateBy = LoginUserInfo.ITCode;
                    file.FileName = fileImage.FileName;
                    file.FileExt = fileImage.ContentType;
                    file.Length = fileImage.Length;
                    file.IsTemprory = true;
                    file.SaveFileMode = SaveFileModeEnum.Local;
                    file.Path = imgPath;
                    DC.Set<FileAttachment>().Add(file);
                    DC.SaveChanges();
                    transaction.Commit();
                    return Json(new { Msg = "success", picid = file.ID });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Json(new { Msg = "error", picid = "" });
                }
            }

        }

        [HttpPost]
        [ActionDescription("批量创建查询")]
        public ActionResult SearchBatchCreation(string SearchBatchCreationModelJson)
        {

            var SearchBatchCreationModel = JsonConvert.DeserializeObject<BatchCreationModel>(SearchBatchCreationModelJson);
            DateTime dateTime = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            if (SearchBatchCreationModel.OrganizationID == null)
            {
                if (GetOrganizationID == null)
                {
                    return Json(new { msg = "请选择组织或您无权限查询", icon = 5 });
                }
                SearchBatchCreationModel.OrganizationID = GetOrganizationID;
            }
            var PlanObject = DC.Set<VOS_Plan>()
                .CheckEqual(SearchBatchCreationModel.OrganizationID, x => x.OrganizationID)
                .CheckEqual(SearchBatchCreationModel.ShopnameId, x => x.ShopnameId)
                .DPWhere(LoginUserInfo.DataPrivileges, x => x.OrganizationID)
                //.CheckBetween(SearchBatchCreationModel.PlanSatrtTime, SearchBatchCreationModel.PlanEndTime, x => x.CreateTime, includeMax: false)
                .Where(x => x.IsValid)
                .OrderByDescending(x => x.CreateTime.Value).FirstOrDefault();
            if (PlanObject != null)
            {
                var TaskListData = DC.Set<VOS_Task>()
                    .Where(x => x.IsValid && x.PlanId == PlanObject.ID && x.Plan.OrganizationID == PlanObject.OrganizationID)
                    .Select(x => new
                    {
                        TaskType = x.TaskType,
                        TaskCateTex = x.TaskCate.Name,
                        TaskCateId = x.TaskCateId,
                        CommodityName = x.CommodityName,
                        CommodityPicId = x.CommodityPicId,
                        CommodityLink = x.CommodityLink,
                        CommodityPrice = x.CommodityPrice,
                        SKU = x.SKU,
                        SearchKeyword = x.SearchKeyword,
                    })
                    .ToList()
                    .GroupBy(x => x.SearchKeyword).Select(y => new { key = y.Key, VOS_Number = y.Count(), Tasklist = y.Distinct(x => x.SearchKeyword) });
                List<VOS_Task> TaskList = new List<VOS_Task>();
                foreach (var TaskListDataObj in TaskListData)
                {
                    foreach (var TasklistObj in TaskListDataObj.Tasklist)
                    {
                        TaskList.Add(new VOS_Task()
                        {
                            TaskTypeText = TasklistObj.TaskType.ToString()
                          ,
                            TaskType = TasklistObj.TaskType
                          ,
                            TaskCateText = TasklistObj.TaskCateTex
                          ,
                            TaskCateId = TasklistObj.TaskCateId
                          ,
                            CommodityName = TasklistObj.CommodityName
                          ,
                            CommodityPicUrl = "/_Framework/GetFile?id=" + TasklistObj.CommodityPicId + "&stream=true&_DONOT_USE_CS=default"
                          ,
                            CommodityPicID = TasklistObj.CommodityPicId.ToString()
                          ,
                            CommodityLink = TasklistObj.CommodityLink
                          ,
                            CommodityPrice = TasklistObj.CommodityPrice
                          ,
                            SKU = TasklistObj.SKU
                          ,
                            SearchKeyword = TasklistObj.SearchKeyword
                          ,
                            VOS_Number = TaskListDataObj.VOS_Number
                        });
                    }
                }
                return Json(new { PlanObject, TaskList });
            }

            return Json(new { PlanObject });
        }

        #endregion

        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(VOS_TaskListVM vm)
        {
            return vm.GetExportData();
        }
    }

    class BatchCreationModel
    {
        public Guid? OrganizationID { get; set; }
        public Guid? ShopnameId { get; set; }
        public DateTime? PlanSatrtTime { get; set; }
        public DateTime? PlanEndTime { get; set; }
    }
}
