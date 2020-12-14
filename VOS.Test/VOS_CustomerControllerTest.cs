using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Customer.VOS_CustomerVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_CustomerControllerTest
    {
        private VOS_CustomerController _controller;
        private string _seed;

        public VOS_CustomerControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_CustomerController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_CustomerListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CustomerVM));

            VOS_CustomerVM vm = rv.Model as VOS_CustomerVM;
            VOS_Customer v = new VOS_Customer();
			
            v.cust_no = "2gSheVYN";
            v.cust_name = "NJZae6UEQ";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Customer>().FirstOrDefault();
				
                Assert.AreEqual(data.cust_no, "2gSheVYN");
                Assert.AreEqual(data.cust_name, "NJZae6UEQ");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.cust_no = "2gSheVYN";
                v.cust_name = "NJZae6UEQ";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CustomerVM));

            VOS_CustomerVM vm = rv.Model as VOS_CustomerVM;
            v = new VOS_Customer();
            v.ID = vm.Entity.ID;
       		
            v.cust_no = "J3x8";
            v.cust_name = "TvA";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.cust_no", "");
            vm.FC.Add("Entity.cust_name", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Customer>().FirstOrDefault();
 				
                Assert.AreEqual(data.cust_no, "J3x8");
                Assert.AreEqual(data.cust_name, "TvA");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.cust_no = "2gSheVYN";
                v.cust_name = "NJZae6UEQ";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CustomerVM));

            VOS_CustomerVM vm = rv.Model as VOS_CustomerVM;
            v = new VOS_Customer();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Customer>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.cust_no = "2gSheVYN";
                v.cust_name = "NJZae6UEQ";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Customer v1 = new VOS_Customer();
            VOS_Customer v2 = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.cust_no = "2gSheVYN";
                v1.cust_name = "NJZae6UEQ";
                v2.cust_no = "J3x8";
                v2.cust_name = "TvA";
                context.Set<VOS_Customer>().Add(v1);
                context.Set<VOS_Customer>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CustomerBatchVM));

            VOS_CustomerBatchVM vm = rv.Model as VOS_CustomerBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Customer>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_CustomerListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
