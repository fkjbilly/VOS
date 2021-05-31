using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.VOS_CommissionVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_CommissionControllerTest
    {
        private VOS_CommissionController _controller;
        private string _seed;

        public VOS_CommissionControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_CommissionController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_CommissionListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CommissionVM));

            VOS_CommissionVM vm = rv.Model as VOS_CommissionVM;
            VOS_Commission v = new VOS_Commission();
			
            v.HeadquartersPrice = 24;
            v.proxyCommission = 25;
            v.memberCommission = 62;
            v.HeadquartersSeparate = 82;
            v.proxySeparate = 62;
            v.PriceRange = "c0jDZLBhc";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Commission>().FirstOrDefault();
				
                Assert.AreEqual(data.HeadquartersPrice, 24);
                Assert.AreEqual(data.proxyCommission, 25);
                Assert.AreEqual(data.memberCommission, 62);
                Assert.AreEqual(data.HeadquartersSeparate, 82);
                Assert.AreEqual(data.proxySeparate, 62);
                Assert.AreEqual(data.PriceRange, "c0jDZLBhc");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Commission v = new VOS_Commission();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.HeadquartersPrice = 24;
                v.proxyCommission = 25;
                v.memberCommission = 62;
                v.HeadquartersSeparate = 82;
                v.proxySeparate = 62;
                v.PriceRange = "c0jDZLBhc";
                context.Set<VOS_Commission>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CommissionVM));

            VOS_CommissionVM vm = rv.Model as VOS_CommissionVM;
            v = new VOS_Commission();
            v.ID = vm.Entity.ID;
       		
            v.HeadquartersPrice = 4;
            v.proxyCommission = 31;
            v.memberCommission = 35;
            v.HeadquartersSeparate = 84;
            v.proxySeparate = 95;
            v.PriceRange = "X2iN1W";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.HeadquartersPrice", "");
            vm.FC.Add("Entity.proxyCommission", "");
            vm.FC.Add("Entity.memberCommission", "");
            vm.FC.Add("Entity.HeadquartersSeparate", "");
            vm.FC.Add("Entity.proxySeparate", "");
            vm.FC.Add("Entity.PriceRange", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Commission>().FirstOrDefault();
 				
                Assert.AreEqual(data.HeadquartersPrice, 4);
                Assert.AreEqual(data.proxyCommission, 31);
                Assert.AreEqual(data.memberCommission, 35);
                Assert.AreEqual(data.HeadquartersSeparate, 84);
                Assert.AreEqual(data.proxySeparate, 95);
                Assert.AreEqual(data.PriceRange, "X2iN1W");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Commission v = new VOS_Commission();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.HeadquartersPrice = 24;
                v.proxyCommission = 25;
                v.memberCommission = 62;
                v.HeadquartersSeparate = 82;
                v.proxySeparate = 62;
                v.PriceRange = "c0jDZLBhc";
                context.Set<VOS_Commission>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CommissionVM));

            VOS_CommissionVM vm = rv.Model as VOS_CommissionVM;
            v = new VOS_Commission();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Commission>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Commission v = new VOS_Commission();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.HeadquartersPrice = 24;
                v.proxyCommission = 25;
                v.memberCommission = 62;
                v.HeadquartersSeparate = 82;
                v.proxySeparate = 62;
                v.PriceRange = "c0jDZLBhc";
                context.Set<VOS_Commission>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Commission v1 = new VOS_Commission();
            VOS_Commission v2 = new VOS_Commission();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.HeadquartersPrice = 24;
                v1.proxyCommission = 25;
                v1.memberCommission = 62;
                v1.HeadquartersSeparate = 82;
                v1.proxySeparate = 62;
                v1.PriceRange = "c0jDZLBhc";
                v2.HeadquartersPrice = 4;
                v2.proxyCommission = 31;
                v2.memberCommission = 35;
                v2.HeadquartersSeparate = 84;
                v2.proxySeparate = 95;
                v2.PriceRange = "X2iN1W";
                context.Set<VOS_Commission>().Add(v1);
                context.Set<VOS_Commission>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CommissionBatchVM));

            VOS_CommissionBatchVM vm = rv.Model as VOS_CommissionBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Commission>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_CommissionListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
