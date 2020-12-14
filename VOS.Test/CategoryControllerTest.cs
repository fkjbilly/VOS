using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using VOS.Controllers;
using VOS.ViewModel.BasicData.CategoryVMs;
using VOS.Model;
using VOS.DataAccess;

namespace VOS.Test
{
    [TestClass]
    public class CategoryControllerTest
    {
        private CategoryController _controller;
        private string _seed;

        public CategoryControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<CategoryController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as CategoryListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(CategoryVM));

            CategoryVM vm = rv.Model as CategoryVM;
            Category v = new Category();
			
            v.Name = "ttbc6G";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Category>().FirstOrDefault();
				
                Assert.AreEqual(data.Name, "ttbc6G");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Category v = new Category();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Name = "ttbc6G";
                context.Set<Category>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(CategoryVM));

            CategoryVM vm = rv.Model as CategoryVM;
            v = new Category();
            v.ID = vm.Entity.ID;
       		
            v.Name = "sEXfPP9s";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Name", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Category>().FirstOrDefault();
 				
                Assert.AreEqual(data.Name, "sEXfPP9s");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Category v = new Category();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Name = "ttbc6G";
                context.Set<Category>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(CategoryVM));

            CategoryVM vm = rv.Model as CategoryVM;
            v = new Category();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Category>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Category v = new Category();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Name = "ttbc6G";
                context.Set<Category>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Category v1 = new Category();
            Category v2 = new Category();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Name = "ttbc6G";
                v2.Name = "sEXfPP9s";
                context.Set<Category>().Add(v1);
                context.Set<Category>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(CategoryBatchVM));

            CategoryBatchVM vm = rv.Model as CategoryBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Category>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as CategoryListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }


    }
}
