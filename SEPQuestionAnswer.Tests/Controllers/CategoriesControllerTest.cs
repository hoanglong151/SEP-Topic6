using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEPQuestionAnswer.Models;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTest
    {
        CategoriesController controller = new CategoriesController();
        SEP24Team10Entities db = new SEP24Team10Entities();
        [TestMethod]
        public void TestIndex()
        {            
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<Category>;
            Assert.IsNotNull(model);
            
            Assert.AreEqual(db.Categories.Count(), model.Count);           
        }

        [TestMethod]
        public void TestCreateView()
        {
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var rand = new Random();
            var cate = new Category
            {
                CategoryName = rand.NextDouble().ToString(),
                Status = false
            };

            var controller = new CategoriesController();

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            cate.Status = true;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }          

            cate.CategoryName = null;
            controller.ModelState.Clear();

            var result2 = controller.Create(cate) as ViewResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual("Tên danh mục không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestEditG()
        {
            var db = new SEP24Team10Entities();
            var controller = new CategoriesController();
            var result0 = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result0);
            var categories = db.Categories.FirstOrDefault();
            var result = controller.Edit(categories.ID) as ViewResult;
            Assert.IsNotNull(result);

            var model = result.Model as Category;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.CategoryName, categories.CategoryName);
        }

        [TestMethod]
        public void TestEditP()
        {
            var db = new SEP24Team10Entities();
            var cate = db.Categories.AsNoTracking().First();
            var rand = new Random();
            var controller = new CategoriesController();
            using (var scope = new TransactionScope())
            {
                cate.CategoryName = "Nghĩ Học";
                cate.Status = true;
                var result = controller.Edit(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
            cate.CategoryName = "Nghĩ Học";
            cate.Status = false;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.Edit(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result1);
                Assert.AreEqual("Index", result1.RouteValues["action"]);
            }

            cate.CategoryName = rand.Next().ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                cate.CategoryName = rand.Next().ToString();
                var result2 = controller.Edit(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result2);
                Assert.AreEqual("Index", result2.RouteValues["action"]);
            }

            cate.CategoryName = null;
            controller.ModelState.Clear();

            var result3 = controller.Edit(cate) as ViewResult;
            Assert.IsNotNull(result3);
            Assert.AreEqual("Tên danh mục không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void UpdateStatus()
        {
            var db = new SEP24Team10Entities();
            var controller = new CategoriesController();
            var cate = db.Categories.AsNoTracking().FirstOrDefault();
            cate.Status = true;
            using (var scope = new TransactionScope())
            {
                var result = controller.UpdateStatus(cate.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            cate.Status = false;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result = controller.UpdateStatus(cate.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
