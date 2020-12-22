using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Business.VOS_UserVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_UserControllerTest
    {
        private VOS_UserController _controller;
        private string _seed;

        public VOS_UserControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_UserController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_UserListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserVM));

            VOS_UserVM vm = rv.Model as VOS_UserVM;
            VOS_User v = new VOS_User();
			
            v.ITCode = "lAoh1Z";
            v.Password = "4JkIz";
            v.Name = "fhOE1";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_User>().FirstOrDefault();
				
                Assert.AreEqual(data.ITCode, "lAoh1Z");
                Assert.AreEqual(data.Password, "4JkIz");
                Assert.AreEqual(data.Name, "fhOE1");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_User v = new VOS_User();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ITCode = "lAoh1Z";
                v.Password = "4JkIz";
                v.Name = "fhOE1";
                context.Set<VOS_User>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserVM));

            VOS_UserVM vm = rv.Model as VOS_UserVM;
            v = new VOS_User();
            v.ID = vm.Entity.ID;
       		
            v.ITCode = "QUFpT7";
            v.Password = "UUcPnSe";
            v.Name = "U4OiX";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ITCode", "");
            vm.FC.Add("Entity.Password", "");
            vm.FC.Add("Entity.Name", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_User>().FirstOrDefault();
 				
                Assert.AreEqual(data.ITCode, "QUFpT7");
                Assert.AreEqual(data.Password, "UUcPnSe");
                Assert.AreEqual(data.Name, "U4OiX");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_User v = new VOS_User();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ITCode = "lAoh1Z";
                v.Password = "4JkIz";
                v.Name = "fhOE1";
                context.Set<VOS_User>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserVM));

            VOS_UserVM vm = rv.Model as VOS_UserVM;
            v = new VOS_User();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_User>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_User v = new VOS_User();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ITCode = "lAoh1Z";
                v.Password = "4JkIz";
                v.Name = "fhOE1";
                context.Set<VOS_User>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_User v1 = new VOS_User();
            VOS_User v2 = new VOS_User();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ITCode = "lAoh1Z";
                v1.Password = "4JkIz";
                v1.Name = "fhOE1";
                v2.ITCode = "QUFpT7";
                v2.Password = "UUcPnSe";
                v2.Name = "U4OiX";
                context.Set<VOS_User>().Add(v1);
                context.Set<VOS_User>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserBatchVM));

            VOS_UserBatchVM vm = rv.Model as VOS_UserBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_User>().Count(), 0);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_UserListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
