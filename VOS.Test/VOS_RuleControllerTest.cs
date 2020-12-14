using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.VOS_RuleVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_RuleControllerTest
    {
        private VOS_RuleController _controller;
        private string _seed;

        public VOS_RuleControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_RuleController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_RuleListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RuleVM));

            VOS_RuleVM vm = rv.Model as VOS_RuleVM;
            VOS_Rule v = new VOS_Rule();
			
            v.RuleName = "M5Slioxl";
            v.Num = "ANnY65VK5";
            v.Cycle = "FL17";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Rule>().FirstOrDefault();
				
                Assert.AreEqual(data.RuleName, "M5Slioxl");
                Assert.AreEqual(data.Num, "ANnY65VK5");
                Assert.AreEqual(data.Cycle, "FL17");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Rule v = new VOS_Rule();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.RuleName = "M5Slioxl";
                v.Num = "ANnY65VK5";
                v.Cycle = "FL17";
                context.Set<VOS_Rule>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RuleVM));

            VOS_RuleVM vm = rv.Model as VOS_RuleVM;
            v = new VOS_Rule();
            v.ID = vm.Entity.ID;
       		
            v.RuleName = "SJDbodLor";
            v.Num = "PpUO1I";
            v.Cycle = "VFAKT";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.RuleName", "");
            vm.FC.Add("Entity.Num", "");
            vm.FC.Add("Entity.Cycle", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Rule>().FirstOrDefault();
 				
                Assert.AreEqual(data.RuleName, "SJDbodLor");
                Assert.AreEqual(data.Num, "PpUO1I");
                Assert.AreEqual(data.Cycle, "VFAKT");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Rule v = new VOS_Rule();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.RuleName = "M5Slioxl";
                v.Num = "ANnY65VK5";
                v.Cycle = "FL17";
                context.Set<VOS_Rule>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RuleVM));

            VOS_RuleVM vm = rv.Model as VOS_RuleVM;
            v = new VOS_Rule();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Rule>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Rule v = new VOS_Rule();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.RuleName = "M5Slioxl";
                v.Num = "ANnY65VK5";
                v.Cycle = "FL17";
                context.Set<VOS_Rule>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Rule v1 = new VOS_Rule();
            VOS_Rule v2 = new VOS_Rule();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.RuleName = "M5Slioxl";
                v1.Num = "ANnY65VK5";
                v1.Cycle = "FL17";
                v2.RuleName = "SJDbodLor";
                v2.Num = "PpUO1I";
                v2.Cycle = "VFAKT";
                context.Set<VOS_Rule>().Add(v1);
                context.Set<VOS_Rule>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RuleBatchVM));

            VOS_RuleBatchVM vm = rv.Model as VOS_RuleBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Rule>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_RuleListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
