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
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.Web;
using System.Buffers.Text;

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
                catch (Exception)
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
                var returnjson = Post(VOrderCode);
                if (returnjson.IndexOf("\"data\":\"yes\"") > 0)
                {
                    //return Json(new { Msg = "淘客订单，不允许完成", State = 5 });
                }
                var vOS_Task = DC.Set<VOS_Task>().Where(x => x.ID == ID).SingleOrDefault();
                vOS_Task.VOrderCode = VOrderCode;
                if (b == true)
                {
                    if (vOS_Task.TaskType == TaskType.隔天单 && DateTime.Now < vOS_Task.DistributionTime.Value.AddHours(8))
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

        #region DistributionOrganization 分配机构
        [ActionDescription("分配组织机构")]
        public ActionResult DistributionOrganization(string[] IDs)
        {
            var vm = CreateVM<VOS_TaskBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("分配组织机构")]
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

        [ActionDescription("批量创建")]
        public ActionResult BatchCreation()
        {
            var vm = CreateVM<VOS_TaskVM>();
            ViewBag.Plan_no = "P" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量创建")]
        public ActionResult DoBatchCreation(string plan, string tasklist)
        {

            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var _plan = JsonConvert.DeserializeObject<VOS_Plan>(plan);
                    _plan.CreateTime = DateTime.Now;
                    _plan.CreateBy = LoginUserInfo.ITCode;
                    _plan.IsValid = true;
                    DC.Set<VOS_Plan>().Add(_plan).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    DC.SaveChanges();
                    var _Task = JsonConvert.DeserializeObject<List<task>>(tasklist);
                    var _PlanId = DC.Set<VOS_Plan>().Where(x => x.Plan_no == _plan.Plan_no).FirstOrDefault().ID;
                    foreach (var item in _Task)
                    {
                        if (item.VOS_Number > 1)
                        {
                            for (int i = 0; i < item.VOS_Number; i++)
                            {
                                VOS_Task _Task_Number = new VOS_Task();
                                _Task_Number.CommodityLink = item.CommodityLink;
                                _Task_Number.CommodityName = item.CommodityName;
                                _Task_Number.CommodityPrice = item.CommodityPrice;
                                _Task_Number.ImplementStartTime = item.ImplementStartTime;
                                _Task_Number.SKU = item.SKU;
                                _Task_Number.TaskType = (TaskType)Enum.Parse(typeof(TaskType), item.TaskType);
                                _Task_Number.Task_no = item.Task_no + "-" + (i + 1);
                                _Task_Number.TaskCateId = item.TaskCateId;
                                _Task_Number.CommodityPicId = SaveImg(item.base64);
                                _Task_Number.ComDis = "/";
                                _Task_Number.Commission = "1";
                                _Task_Number.OtherExpenses = "1";
                                _Task_Number.PlanId = _PlanId;
                                _Task_Number.CreateBy = LoginUserInfo.ITCode;
                                _Task_Number.CreateTime = DateTime.Now;
                                _Task_Number.IsValid = true;
                                DC.Set<VOS_Task>().Add(_Task_Number).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                                DC.SaveChanges();
                            }
                        }
                        else
                        {
                            VOS_Task _Task1 = new VOS_Task();
                            _Task1.CommodityLink = item.CommodityLink;
                            _Task1.CommodityName = item.CommodityName;
                            _Task1.CommodityPrice = item.CommodityPrice;
                            _Task1.ImplementStartTime = item.ImplementStartTime;
                            _Task1.SKU = item.SKU;
                            _Task1.TaskType = (TaskType)Enum.Parse(typeof(TaskType), item.TaskType);
                            _Task1.Task_no = item.Task_no;
                            _Task1.TaskCateId = item.TaskCateId;
                            _Task1.CommodityPicId = SaveImg(item.base64);
                            _Task1.ComDis = "/";
                            _Task1.Commission = "1";
                            _Task1.OtherExpenses = "1";
                            _Task1.PlanId = _PlanId;
                            _Task1.CreateBy = LoginUserInfo.ITCode;
                            _Task1.CreateTime = DateTime.Now;
                            _Task1.IsValid = true;
                            DC.Set<VOS_Task>().Add(_Task1).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            DC.SaveChanges();
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
            var _Category = DC.Set<Category>().Where(x => x.ParentId == null).GetSelectListItems(LoginUserInfo?.DataPrivileges, null, y => y.Name).Select(x => new { x.Value, x.Text }).ToList();
            return Json(_Category);
        }


        /// <summary>
        /// 保存图片并获取图片ID
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        private Guid SaveImg(string StringBase64)
        {
            try
            {
                byte[] base64 = Convert.FromBase64String(StringBase64.Substring(StringBase64.IndexOf(",") + 1));
                FileAttachment file = new FileAttachment();
                file.CreateTime = DateTime.Now;
                file.CreateBy = LoginUserInfo.ITCode;
                //图片名称
                file.FileName = DateTime.Now.ToString("yyyymmddhhmmss");
                //图片类型
                file.FileExt = GetImageSuffix(base64).RawFormat.ToString();
                //长度
                file.Length = base64.Length;
                file.IsTemprory = true;
                file.SaveFileMode = SaveFileModeEnum.Database;
                file.FileData = base64;
                DC.Set<FileAttachment>().Add(file).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                DC.SaveChanges();
                return file.ID;
            }
            catch (Exception ex)
            {
                return new Guid();
            }
        }
        /// <summary>
        /// 获取图片后缀
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private  Image GetImageSuffix(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            ms.Position = 0;
            Image img = Image.FromStream(ms);
            ms.Close();
            return img;
        }

        #endregion


        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(VOS_TaskListVM vm)
        {
            return vm.GetExportData();
        }

    }

    class task
    {
        public string CommodityLink { get; set; }
        public string CommodityName { get; set; }
        public string CommodityPrice { get; set; }
        public DateTime ImplementStartTime { get; set; }
        public string SKU { get; set; }
        public string TaskType { get; set; }
        public string Task_no { get; set; }
        public Guid? TaskCateId { get; set; }
        public int VOS_Number { get; set; }
        public string base64 { get; set; }
        public string filename { get; set; }
    }
}
