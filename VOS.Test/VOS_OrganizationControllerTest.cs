using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.VOS_OrganizationVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_OrganizationControllerTest
    {
        private VOS_OrganizationController _controller;
        private string _seed;

        public VOS_OrganizationControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_OrganizationController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_OrganizationListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_OrganizationVM));

            VOS_OrganizationVM vm = rv.Model as VOS_OrganizationVM;
            VOS_Organization v = new VOS_Organization();
			
            v.OrganizationName = "poq8AbTBY";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Organization>().FirstOrDefault();
				
                Assert.AreEqual(data.OrganizationName, "poq8AbTBY");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Organization v = new VOS_Organization();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.OrganizationName = "poq8AbTBY";
                context.Set<VOS_Organization>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_OrganizationVM));

            VOS_OrganizationVM vm = rv.Model as VOS_OrganizationVM;
            v = new VOS_Organization();
            v.ID = vm.Entity.ID;
       		
            v.OrganizationName = "Lj7KA";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.OrganizationName", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Organization>().FirstOrDefault();
 				
                Assert.AreEqual(data.OrganizationName, "Lj7KA");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Organization v = new VOS_Organization();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.OrganizationName = "poq8AbTBY";
                context.Set<VOS_Organization>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_OrganizationVM));

            VOS_OrganizationVM vm = rv.Model as VOS_OrganizationVM;
            v = new VOS_Organization();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Organization>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Organization v = new VOS_Organization();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.OrganizationName = "poq8AbTBY";
                context.Set<VOS_Organization>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Organization v1 = new VOS_Organization();
            VOS_Organization v2 = new VOS_Organization();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.OrganizationName = "poq8AbTBY";
                v2.OrganizationName = "Lj7KA";
                context.Set<VOS_Organization>().Add(v1);
                context.Set<VOS_Organization>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_OrganizationBatchVM));

            VOS_OrganizationBatchVM vm = rv.Model as VOS_OrganizationBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Organization>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_OrganizationListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
