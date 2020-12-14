using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Business.VOS_PlanVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_PlanControllerTest
    {
        private VOS_PlanController _controller;
        private string _seed;

        public VOS_PlanControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_PlanController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_PlanListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PlanVM));

            VOS_PlanVM vm = rv.Model as VOS_PlanVM;
            VOS_Plan v = new VOS_Plan();
			
            v.Plan_no = "5gKbDD";
            v.ShopnameId = AddShopname();
            v.PlanFee = "QDD";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Plan>().FirstOrDefault();
				
                Assert.AreEqual(data.Plan_no, "5gKbDD");
                Assert.AreEqual(data.PlanFee, "QDD");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Plan v = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Plan_no = "5gKbDD";
                v.ShopnameId = AddShopname();
                v.PlanFee = "QDD";
                context.Set<VOS_Plan>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PlanVM));

            VOS_PlanVM vm = rv.Model as VOS_PlanVM;
            v = new VOS_Plan();
            v.ID = vm.Entity.ID;
       		
            v.Plan_no = "Hvyf";
            v.PlanFee = "4cskm";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Plan_no", "");
            vm.FC.Add("Entity.ShopnameId", "");
            vm.FC.Add("Entity.PlanFee", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Plan>().FirstOrDefault();
 				
                Assert.AreEqual(data.Plan_no, "Hvyf");
                Assert.AreEqual(data.PlanFee, "4cskm");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Plan v = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Plan_no = "5gKbDD";
                v.ShopnameId = AddShopname();
                v.PlanFee = "QDD";
                context.Set<VOS_Plan>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PlanVM));

            VOS_PlanVM vm = rv.Model as VOS_PlanVM;
            v = new VOS_Plan();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Plan>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Plan v = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Plan_no = "5gKbDD";
                v.ShopnameId = AddShopname();
                v.PlanFee = "QDD";
                context.Set<VOS_Plan>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Plan v1 = new VOS_Plan();
            VOS_Plan v2 = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Plan_no = "5gKbDD";
                v1.ShopnameId = AddShopname();
                v1.PlanFee = "QDD";
                v2.Plan_no = "Hvyf";
                v2.ShopnameId = v1.ShopnameId; 
                v2.PlanFee = "4cskm";
                context.Set<VOS_Plan>().Add(v1);
                context.Set<VOS_Plan>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PlanBatchVM));

            VOS_PlanBatchVM vm = rv.Model as VOS_PlanBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Plan>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_PlanListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCustomer()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.cust_no = "xod7i";
                v.cust_name = "TyAf";
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
                v.ShopName = "3LprtqDC";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
