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
			
            v.HeadquartersPrice = 77;
            v.proxyCommission = 93;
            v.memberCommission = 86;
            v.HeadquartersSeparate = 70;
            v.proxySeparate = 79;
            v.VOS_RangeID = AddVOS_Range();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Commission>().FirstOrDefault();
				
                Assert.AreEqual(data.HeadquartersPrice, 77);
                Assert.AreEqual(data.proxyCommission, 93);
                Assert.AreEqual(data.memberCommission, 86);
                Assert.AreEqual(data.HeadquartersSeparate, 70);
                Assert.AreEqual(data.proxySeparate, 79);
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
       			
                v.HeadquartersPrice = 77;
                v.proxyCommission = 93;
                v.memberCommission = 86;
                v.HeadquartersSeparate = 70;
                v.proxySeparate = 79;
                v.VOS_RangeID = AddVOS_Range();
                context.Set<VOS_Commission>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CommissionVM));

            VOS_CommissionVM vm = rv.Model as VOS_CommissionVM;
            v = new VOS_Commission();
            v.ID = vm.Entity.ID;
       		
            v.HeadquartersPrice = 49;
            v.proxyCommission = 37;
            v.memberCommission = 43;
            v.HeadquartersSeparate = 56;
            v.proxySeparate = 36;
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.HeadquartersPrice", "");
            vm.FC.Add("Entity.proxyCommission", "");
            vm.FC.Add("Entity.memberCommission", "");
            vm.FC.Add("Entity.HeadquartersSeparate", "");
            vm.FC.Add("Entity.proxySeparate", "");
            vm.FC.Add("Entity.VOS_RangeID", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Commission>().FirstOrDefault();
 				
                Assert.AreEqual(data.HeadquartersPrice, 49);
                Assert.AreEqual(data.proxyCommission, 37);
                Assert.AreEqual(data.memberCommission, 43);
                Assert.AreEqual(data.HeadquartersSeparate, 56);
                Assert.AreEqual(data.proxySeparate, 36);
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
        		
                v.HeadquartersPrice = 77;
                v.proxyCommission = 93;
                v.memberCommission = 86;
                v.HeadquartersSeparate = 70;
                v.proxySeparate = 79;
                v.VOS_RangeID = AddVOS_Range();
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
                Assert.AreEqual(context.Set<VOS_Commission>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Commission v = new VOS_Commission();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.HeadquartersPrice = 77;
                v.proxyCommission = 93;
                v.memberCommission = 86;
                v.HeadquartersSeparate = 70;
                v.proxySeparate = 79;
                v.VOS_RangeID = AddVOS_Range();
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
				
                v1.HeadquartersPrice = 77;
                v1.proxyCommission = 93;
                v1.memberCommission = 86;
                v1.HeadquartersSeparate = 70;
                v1.proxySeparate = 79;
                v1.VOS_RangeID = AddVOS_Range();
                v2.HeadquartersPrice = 49;
                v2.proxyCommission = 37;
                v2.memberCommission = 43;
                v2.HeadquartersSeparate = 56;
                v2.proxySeparate = 36;
                v2.VOS_RangeID = v1.VOS_RangeID; 
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
                Assert.AreEqual(context.Set<VOS_Commission>().Count(), 0);
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

        private Guid AddVOS_Range()
        {
            VOS_Range v = new VOS_Range();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.MinNumber = 60;
                v.MaxNumber = 1;
                context.Set<VOS_Range>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
