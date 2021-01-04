using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.VOS_DistributionVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_DistributionControllerTest
    {
        private VOS_DistributionController _controller;
        private string _seed;

        public VOS_DistributionControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_DistributionController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_DistributionListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_DistributionVM));

            VOS_DistributionVM vm = rv.Model as VOS_DistributionVM;
            VOS_Distribution v = new VOS_Distribution();
			
            v.DistributionName = "nq1hMPzY";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Distribution>().FirstOrDefault();
				
                Assert.AreEqual(data.DistributionName, "nq1hMPzY");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Distribution v = new VOS_Distribution();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.DistributionName = "nq1hMPzY";
                context.Set<VOS_Distribution>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_DistributionVM));

            VOS_DistributionVM vm = rv.Model as VOS_DistributionVM;
            v = new VOS_Distribution();
            v.ID = vm.Entity.ID;
       		
            v.DistributionName = "MJY";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.DistributionName", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Distribution>().FirstOrDefault();
 				
                Assert.AreEqual(data.DistributionName, "MJY");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Distribution v = new VOS_Distribution();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.DistributionName = "nq1hMPzY";
                context.Set<VOS_Distribution>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_DistributionVM));

            VOS_DistributionVM vm = rv.Model as VOS_DistributionVM;
            v = new VOS_Distribution();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Distribution>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Distribution v = new VOS_Distribution();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.DistributionName = "nq1hMPzY";
                context.Set<VOS_Distribution>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Distribution v1 = new VOS_Distribution();
            VOS_Distribution v2 = new VOS_Distribution();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.DistributionName = "nq1hMPzY";
                v2.DistributionName = "MJY";
                context.Set<VOS_Distribution>().Add(v1);
                context.Set<VOS_Distribution>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_DistributionBatchVM));

            VOS_DistributionBatchVM vm = rv.Model as VOS_DistributionBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Distribution>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_DistributionListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
