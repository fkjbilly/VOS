using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Customer.VOS_ShopVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_ShopControllerTest
    {
        private VOS_ShopController _controller;
        private string _seed;

        public VOS_ShopControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_ShopController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_ShopListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_ShopVM));

            VOS_ShopVM vm = rv.Model as VOS_ShopVM;
            VOS_Shop v = new VOS_Shop();
			
            v.CustomerId = AddCustomer();
            v.ShopName = "PvZ";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Shop>().FirstOrDefault();
				
                Assert.AreEqual(data.ShopName, "PvZ");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Shop v = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.CustomerId = AddCustomer();
                v.ShopName = "PvZ";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_ShopVM));

            VOS_ShopVM vm = rv.Model as VOS_ShopVM;
            v = new VOS_Shop();
            v.ID = vm.Entity.ID;
       		
            v.ShopName = "b1Qcz";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.CustomerId", "");
            vm.FC.Add("Entity.ShopName", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Shop>().FirstOrDefault();
 				
                Assert.AreEqual(data.ShopName, "b1Qcz");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Shop v = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.CustomerId = AddCustomer();
                v.ShopName = "PvZ";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_ShopVM));

            VOS_ShopVM vm = rv.Model as VOS_ShopVM;
            v = new VOS_Shop();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Shop>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Shop v = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.CustomerId = AddCustomer();
                v.ShopName = "PvZ";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Shop v1 = new VOS_Shop();
            VOS_Shop v2 = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.CustomerId = AddCustomer();
                v1.ShopName = "PvZ";
                v2.CustomerId = v1.CustomerId; 
                v2.ShopName = "b1Qcz";
                context.Set<VOS_Shop>().Add(v1);
                context.Set<VOS_Shop>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_ShopBatchVM));

            VOS_ShopBatchVM vm = rv.Model as VOS_ShopBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Shop>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_ShopListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCustomer()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.cust_no = "Lwu";
                v.cust_name = "w1L";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
