﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                StatusCategory_ID = 1
            };

            var controller = new CategoriesController();

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            cate.CategoryName = "Thôi học";
            var check = db.Categories.FirstOrDefault(c => c.CategoryName == cate.CategoryName);
            var result3 = controller.Create(cate) as ViewResult;
            Assert.IsNotNull(result3);
            Assert.AreEqual("Tên danh mục đã tồn tại", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
            

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
            Assert.AreEqual(model.StatusCategory_ID, categories.StatusCategory_ID);
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
                var result = controller.Edit(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            cate.CategoryName = rand.Next().ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                cate.CategoryName = rand.Next().ToString();
                var result = controller.Edit(cate) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }


            cate = db.Categories.AsNoTracking().OrderByDescending(x => x.ID).First();
            using (var scope = new TransactionScope())
            {
                cate.CategoryName = "Nghĩ Học";
                var result = controller.Edit(cate) as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Tên danh mục đã tồn tại", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
            }

            cate.CategoryName = null;
            controller.ModelState.Clear();

            var result1 = controller.Edit(cate) as ViewResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Tên danh mục không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
        }
    }
}
