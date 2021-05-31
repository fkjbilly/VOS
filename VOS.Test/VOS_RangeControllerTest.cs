using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.VOS_RangeVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_RangeControllerTest
    {
        private VOS_RangeController _controller;
        private string _seed;

        public VOS_RangeControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_RangeController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_RangeListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RangeVM));

            VOS_RangeVM vm = rv.Model as VOS_RangeVM;
            VOS_Range v = new VOS_Range();
			
            v.MinNumber = 34;
            v.MaxNumber = 59;
            v.PriceRangeGroup = "YTcltRxvL";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Range>().FirstOrDefault();
				
                Assert.AreEqual(data.MinNumber, 34);
                Assert.AreEqual(data.MaxNumber, 59);
                Assert.AreEqual(data.PriceRangeGroup, "YTcltRxvL");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Range v = new VOS_Range();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.MinNumber = 34;
                v.MaxNumber = 59;
                v.PriceRangeGroup = "YTcltRxvL";
                context.Set<VOS_Range>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RangeVM));

            VOS_RangeVM vm = rv.Model as VOS_RangeVM;
            v = new VOS_Range();
            v.ID = vm.Entity.ID;
       		
            v.MinNumber = 49;
            v.MaxNumber = 7;
            v.PriceRangeGroup = "mDJCT";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.MinNumber", "");
            vm.FC.Add("Entity.MaxNumber", "");
            vm.FC.Add("Entity.PriceRangeGroup", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Range>().FirstOrDefault();
 				
                Assert.AreEqual(data.MinNumber, 49);
                Assert.AreEqual(data.MaxNumber, 7);
                Assert.AreEqual(data.PriceRangeGroup, "mDJCT");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Range v = new VOS_Range();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.MinNumber = 34;
                v.MaxNumber = 59;
                v.PriceRangeGroup = "YTcltRxvL";
                context.Set<VOS_Range>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RangeVM));

            VOS_RangeVM vm = rv.Model as VOS_RangeVM;
            v = new VOS_Range();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Range>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Range v = new VOS_Range();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.MinNumber = 34;
                v.MaxNumber = 59;
                v.PriceRangeGroup = "YTcltRxvL";
                context.Set<VOS_Range>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Range v1 = new VOS_Range();
            VOS_Range v2 = new VOS_Range();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.MinNumber = 34;
                v1.MaxNumber = 59;
                v1.PriceRangeGroup = "YTcltRxvL";
                v2.MinNumber = 49;
                v2.MaxNumber = 7;
                v2.PriceRangeGroup = "mDJCT";
                context.Set<VOS_Range>().Add(v1);
                context.Set<VOS_Range>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_RangeBatchVM));

            VOS_RangeBatchVM vm = rv.Model as VOS_RangeBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Range>().Count(), 0);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_RangeListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
