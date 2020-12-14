using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Business.VOS_TaskVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_TaskControllerTest
    {
        private VOS_TaskController _controller;
        private string _seed;

        public VOS_TaskControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_TaskController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_TaskListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_TaskVM));

            VOS_TaskVM vm = rv.Model as VOS_TaskVM;
            VOS_Task v = new VOS_Task();
			
            v.Task_no = "aSd4ySIyM";
            v.PlanId = AddPlan();
            v.ComDis = "efXZNA2uA";
            v.CommodityName = "b1gCJLeVc";
            v.CommodityLink = "HWLB7F0";
            v.CommodityPrice = "2v0l0";
            v.Commission = "kY0";
            v.OtherExpenses = "4OzclEnJ";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Task>().FirstOrDefault();
				
                Assert.AreEqual(data.Task_no, "aSd4ySIyM");
                Assert.AreEqual(data.ComDis, "efXZNA2uA");
                Assert.AreEqual(data.CommodityName, "b1gCJLeVc");
                Assert.AreEqual(data.CommodityLink, "HWLB7F0");
                Assert.AreEqual(data.CommodityPrice, "2v0l0");
                Assert.AreEqual(data.Commission, "kY0");
                Assert.AreEqual(data.OtherExpenses, "4OzclEnJ");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Task v = new VOS_Task();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Task_no = "aSd4ySIyM";
                v.PlanId = AddPlan();
                v.ComDis = "efXZNA2uA";
                v.CommodityName = "b1gCJLeVc";
                v.CommodityLink = "HWLB7F0";
                v.CommodityPrice = "2v0l0";
                v.Commission = "kY0";
                v.OtherExpenses = "4OzclEnJ";
                context.Set<VOS_Task>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_TaskVM));

            VOS_TaskVM vm = rv.Model as VOS_TaskVM;
            v = new VOS_Task();
            v.ID = vm.Entity.ID;
       		
            v.Task_no = "3sa";
            v.ComDis = "5IUWTB";
            v.CommodityName = "A3v";
            v.CommodityLink = "0CK9YVAI";
            v.CommodityPrice = "Zh3";
            v.Commission = "KLoc";
            v.OtherExpenses = "8FC1";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Task_no", "");
            vm.FC.Add("Entity.PlanId", "");
            vm.FC.Add("Entity.ComDis", "");
            vm.FC.Add("Entity.CommodityName", "");
            vm.FC.Add("Entity.CommodityLink", "");
            vm.FC.Add("Entity.CommodityPrice", "");
            vm.FC.Add("Entity.Commission", "");
            vm.FC.Add("Entity.OtherExpenses", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Task>().FirstOrDefault();
 				
                Assert.AreEqual(data.Task_no, "3sa");
                Assert.AreEqual(data.ComDis, "5IUWTB");
                Assert.AreEqual(data.CommodityName, "A3v");
                Assert.AreEqual(data.CommodityLink, "0CK9YVAI");
                Assert.AreEqual(data.CommodityPrice, "Zh3");
                Assert.AreEqual(data.Commission, "KLoc");
                Assert.AreEqual(data.OtherExpenses, "8FC1");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Task v = new VOS_Task();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Task_no = "aSd4ySIyM";
                v.PlanId = AddPlan();
                v.ComDis = "efXZNA2uA";
                v.CommodityName = "b1gCJLeVc";
                v.CommodityLink = "HWLB7F0";
                v.CommodityPrice = "2v0l0";
                v.Commission = "kY0";
                v.OtherExpenses = "4OzclEnJ";
                context.Set<VOS_Task>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_TaskVM));

            VOS_TaskVM vm = rv.Model as VOS_TaskVM;
            v = new VOS_Task();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Task>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Task v = new VOS_Task();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Task_no = "aSd4ySIyM";
                v.PlanId = AddPlan();
                v.ComDis = "efXZNA2uA";
                v.CommodityName = "b1gCJLeVc";
                v.CommodityLink = "HWLB7F0";
                v.CommodityPrice = "2v0l0";
                v.Commission = "kY0";
                v.OtherExpenses = "4OzclEnJ";
                context.Set<VOS_Task>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Task v1 = new VOS_Task();
            VOS_Task v2 = new VOS_Task();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Task_no = "aSd4ySIyM";
                v1.PlanId = AddPlan();
                v1.ComDis = "efXZNA2uA";
                v1.CommodityName = "b1gCJLeVc";
                v1.CommodityLink = "HWLB7F0";
                v1.CommodityPrice = "2v0l0";
                v1.Commission = "kY0";
                v1.OtherExpenses = "4OzclEnJ";
                v2.Task_no = "3sa";
                v2.PlanId = v1.PlanId; 
                v2.ComDis = "5IUWTB";
                v2.CommodityName = "A3v";
                v2.CommodityLink = "0CK9YVAI";
                v2.CommodityPrice = "Zh3";
                v2.Commission = "KLoc";
                v2.OtherExpenses = "8FC1";
                context.Set<VOS_Task>().Add(v1);
                context.Set<VOS_Task>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_TaskBatchVM));

            VOS_TaskBatchVM vm = rv.Model as VOS_TaskBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Task>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_TaskListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCustomer()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.cust_no = "lmYAGD";
                v.cust_name = "9OcrWDL2P";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }

        private Guid AddShopname()
        {
            VOS_Shop v = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.CustomerId = AddCustomer();
                v.ShopName = "kyR";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }

        private Guid AddPlan()
        {
            VOS_Plan v = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.Plan_no = "Apn";
                v.ShopnameId = AddShopname();
                v.PlanFee = "49vw8NOp";
                context.Set<VOS_Plan>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
