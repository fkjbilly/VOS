using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Business.VOS_PEmployeeVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_PEmployeeControllerTest
    {
        private VOS_PEmployeeController _controller;
        private string _seed;

        public VOS_PEmployeeControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_PEmployeeController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_PEmployeeListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PEmployeeVM));

            VOS_PEmployeeVM vm = rv.Model as VOS_PEmployeeVM;
            VOS_PEmployee v = new VOS_PEmployee();
			
            v.Address = "hDNvoONt";
            v.FullName = "UXwELm2m";
            v.Mobile = "yWoE";
            v.WeChat = "uRn4ItBUw";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_PEmployee>().FirstOrDefault();
				
                Assert.AreEqual(data.Address, "hDNvoONt");
                Assert.AreEqual(data.FullName, "UXwELm2m");
                Assert.AreEqual(data.Mobile, "yWoE");
                Assert.AreEqual(data.WeChat, "uRn4ItBUw");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_PEmployee v = new VOS_PEmployee();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Address = "hDNvoONt";
                v.FullName = "UXwELm2m";
                v.Mobile = "yWoE";
                v.WeChat = "uRn4ItBUw";
                context.Set<VOS_PEmployee>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PEmployeeVM));

            VOS_PEmployeeVM vm = rv.Model as VOS_PEmployeeVM;
            v = new VOS_PEmployee();
            v.ID = vm.Entity.ID;
       		
            v.Address = "Hm0TXFu";
            v.FullName = "1ZEwogb";
            v.Mobile = "VT5v";
            v.WeChat = "totUj";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Address", "");
            vm.FC.Add("Entity.FullName", "");
            vm.FC.Add("Entity.Mobile", "");
            vm.FC.Add("Entity.WeChat", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_PEmployee>().FirstOrDefault();
 				
                Assert.AreEqual(data.Address, "Hm0TXFu");
                Assert.AreEqual(data.FullName, "1ZEwogb");
                Assert.AreEqual(data.Mobile, "VT5v");
                Assert.AreEqual(data.WeChat, "totUj");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_PEmployee v = new VOS_PEmployee();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Address = "hDNvoONt";
                v.FullName = "UXwELm2m";
                v.Mobile = "yWoE";
                v.WeChat = "uRn4ItBUw";
                context.Set<VOS_PEmployee>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PEmployeeVM));

            VOS_PEmployeeVM vm = rv.Model as VOS_PEmployeeVM;
            v = new VOS_PEmployee();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_PEmployee>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_PEmployee v = new VOS_PEmployee();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Address = "hDNvoONt";
                v.FullName = "UXwELm2m";
                v.Mobile = "yWoE";
                v.WeChat = "uRn4ItBUw";
                context.Set<VOS_PEmployee>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_PEmployee v1 = new VOS_PEmployee();
            VOS_PEmployee v2 = new VOS_PEmployee();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Address = "hDNvoONt";
                v1.FullName = "UXwELm2m";
                v1.Mobile = "yWoE";
                v1.WeChat = "uRn4ItBUw";
                v2.Address = "Hm0TXFu";
                v2.FullName = "1ZEwogb";
                v2.Mobile = "VT5v";
                v2.WeChat = "totUj";
                context.Set<VOS_PEmployee>().Add(v1);
                context.Set<VOS_PEmployee>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_PEmployeeBatchVM));

            VOS_PEmployeeBatchVM vm = rv.Model as VOS_PEmployeeBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_PEmployee>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_PEmployeeListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
