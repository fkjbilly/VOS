using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.Finance.VOS_CollectionVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class VOS_CollectionControllerTest
    {
        private VOS_CollectionController _controller;
        private string _seed;

        public VOS_CollectionControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VOS_CollectionController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as VOS_CollectionListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CollectionVM));

            VOS_CollectionVM vm = rv.Model as VOS_CollectionVM;
            VOS_Collection v = new VOS_Collection();
			
            v.Plan_noId = AddPlan_no();
            v.Collection = "nMC";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Collection>().FirstOrDefault();
				
                Assert.AreEqual(data.Collection, "nMC");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            VOS_Collection v = new VOS_Collection();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Plan_noId = AddPlan_no();
                v.Collection = "nMC";
                context.Set<VOS_Collection>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CollectionVM));

            VOS_CollectionVM vm = rv.Model as VOS_CollectionVM;
            v = new VOS_Collection();
            v.ID = vm.Entity.ID;
       		
            v.Collection = "WhQdgkKV";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Plan_noId", "");
            vm.FC.Add("Entity.Collection", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<VOS_Collection>().FirstOrDefault();
 				
                Assert.AreEqual(data.Collection, "WhQdgkKV");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            VOS_Collection v = new VOS_Collection();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Plan_noId = AddPlan_no();
                v.Collection = "nMC";
                context.Set<VOS_Collection>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CollectionVM));

            VOS_CollectionVM vm = rv.Model as VOS_CollectionVM;
            v = new VOS_Collection();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Collection>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            VOS_Collection v = new VOS_Collection();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Plan_noId = AddPlan_no();
                v.Collection = "nMC";
                context.Set<VOS_Collection>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            VOS_Collection v1 = new VOS_Collection();
            VOS_Collection v2 = new VOS_Collection();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Plan_noId = AddPlan_no();
                v1.Collection = "nMC";
                v2.Plan_noId = v1.Plan_noId; 
                v2.Collection = "WhQdgkKV";
                context.Set<VOS_Collection>().Add(v1);
                context.Set<VOS_Collection>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VOS_CollectionBatchVM));

            VOS_CollectionBatchVM vm = rv.Model as VOS_CollectionBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<VOS_Collection>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as VOS_CollectionListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Guid AddCustomer()
        {
            VOS_Customer v = new VOS_Customer();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.cust_no = "pKGf1";
                v.cust_name = "0p5uyo9oF";
                context.Set<VOS_Customer>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }

        private Guid AddShopname()
        {
            VOS_Shop v = new VOS_Shop();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.CustomerId = AddCustomer();
                v.ShopName = "lfTNZOtw7";
                context.Set<VOS_Shop>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }

        private Guid AddPlan_no()
        {
            VOS_Plan v = new VOS_Plan();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.Plan_no = "jEbnCt";
                v.ShopnameId = AddShopname();
                v.PlanFee = "ZV4ux";
                context.Set<VOS_Plan>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
