using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEPQuestionAnswer.Models;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

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
        public void TestCreate()
        {
            Random rd = new Random();
            //In case the category name is null
            Category category = new Category
            {
                ID = rd.Next(),
                CategoryName = null,
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };
            var result = controller.Create(category) as ViewResult;
            Assert.AreEqual("Điền tên danh mục", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);
          
            //In case the category name is space
            Category category1 = new Category
            {
                ID = rd.Next(),
                CategoryName = "   ",
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };
            var result1 = controller.Create(category1) as ViewResult;
            Assert.AreEqual("Tên danh muc không hợp lệ", controller.ModelState["CategoryName"].Errors[1].ErrorMessage);


            //Testing create category succesfully
            Category category2 = new Category
            {
                ID = rd.Next(),
                CategoryName = rd.NextDouble().ToString(),
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };           
            var result2 = controller.Create(category2) as ViewResult;
            Assert.IsNotNull(result2);                       
        }

        [TestMethod]
        public void TestEditView()
        {
            var id = new Random().Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count());
            var result = controller.Edit(id) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestEdit()
        {
            Random rd = new Random();
            //In case the category name is null
            Category category = new Category
            {
                ID = rd.Next(),
                CategoryName = null,
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };
            var result = controller.Edit(category) as ViewResult;
            Assert.AreEqual("Điền tên danh mục", controller.ModelState["CategoryName"].Errors[0].ErrorMessage);

            //In case the category name is space
            Category category1 = new Category
            {
                ID = rd.Next(),
                CategoryName = "   ",
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };
            var result1 = controller.Edit(category1) as ViewResult;
            Assert.AreEqual("Tên danh muc không hợp lệ", controller.ModelState["CategoryName"].Errors[1].ErrorMessage);


            //Testing create category succesfully
            Category category2 = new Category
            {
                ID = rd.Next(),
                CategoryName = rd.NextDouble().ToString(),
                StatusCategory_ID = rd.Next(db.Categories.FirstOrDefault(p => p.ID > 0).ID, db.StatusCategories.Count())
            };
            var result2 = controller.Edit(category2) as ViewResult;
            Assert.IsNotNull(result2);
        }
    }
}
