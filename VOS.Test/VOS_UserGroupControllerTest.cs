using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Business.VOS_UserGroupVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_UserGroupControllerTest
    {
        private VOS_UserGroupController _controller;
        private string _seed;

        public VOS_UserGroupControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_UserGroupController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_UserGroupListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserGroupVM));

            VOS_UserGroupVM vm = rv.Model as VOS_UserGroupVM;
            VOS_UserGroup v = new VOS_UserGroup();
			
            v.GroupCode = "J04P";
            v.GroupName = "W3dHFX";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_UserGroup>().FirstOrDefault();
				
                Assert.AreEqual(data.GroupCode, "J04P");
                Assert.AreEqual(data.GroupName, "W3dHFX");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_UserGroup v = new VOS_UserGroup();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.GroupCode = "J04P";
                v.GroupName = "W3dHFX";
                context.Set<VOS_UserGroup>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserGroupVM));

            VOS_UserGroupVM vm = rv.Model as VOS_UserGroupVM;
            v = new VOS_UserGroup();
            v.ID = vm.Entity.ID;
       		
            v.GroupCode = "ZVW";
            v.GroupName = "0BP40dZSR";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.GroupCode", "");
            vm.FC.Add("Entity.GroupName", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_UserGroup>().FirstOrDefault();
 				
                Assert.AreEqual(data.GroupCode, "ZVW");
                Assert.AreEqual(data.GroupName, "0BP40dZSR");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_UserGroup v = new VOS_UserGroup();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.GroupCode = "J04P";
                v.GroupName = "W3dHFX";
                context.Set<VOS_UserGroup>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserGroupVM));

            VOS_UserGroupVM vm = rv.Model as VOS_UserGroupVM;
            v = new VOS_UserGroup();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_UserGroup>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_UserGroup v = new VOS_UserGroup();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.GroupCode = "J04P";
                v.GroupName = "W3dHFX";
                context.Set<VOS_UserGroup>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_UserGroup v1 = new VOS_UserGroup();
            VOS_UserGroup v2 = new VOS_UserGroup();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.GroupCode = "J04P";
                v1.GroupName = "W3dHFX";
                v2.GroupCode = "ZVW";
                v2.GroupName = "0BP40dZSR";
                context.Set<VOS_UserGroup>().Add(v1);
                context.Set<VOS_UserGroup>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_UserGroupBatchVM));

            VOS_UserGroupBatchVM vm = rv.Model as VOS_UserGroupBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_UserGroup>().Count(), 0);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_UserGroupListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
