using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEPQuestionAnswer;
using SEPQuestionAnswer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;
using System.Web;
using System.Security.Principal;
using System.Transactions;
using Moq;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        HomeController controller = new HomeController();
        SEP24Team10Entities db = new SEP24Team10Entities();

        [TestMethod]
        public void Index()
        {
            HomeControllerTest builder = new HomeControllerTest();
            var tempData = new TempDataDictionary();
            tempData.Add("fail", "a");
            // Act
            ViewResult result = controller.Index() as ViewResult;
            controller.TempData = tempData;
            controller.ViewBag.fail = tempData;
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(controller.TempData);
            Assert.AreEqual(controller.TempData, result.ViewBag.fail);
            var result1 = result.Model as List<Question>;
            Assert.AreEqual(db.Questions.OrderByDescending(x => x.CountView).Take(10).Count(), result1.Count);
        }

        [TestMethod]
        public void IndexCate()
        {
            var result = controller.IndexCate() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Category>;
            Assert.AreEqual(db.Categories.Where(x => x.Status == true).OrderBy(n => n.CategoryName).Count(), result1.Count);
        }

        [TestMethod]
        public void IndexQByC()
        {
            var result = controller.IndexQByC(2) as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Question>;
            Assert.AreEqual(db.Questions.Where(k => k.Category_ID == 2).Where(s => s.Status == "Accept").Count(), result1.Count);

            controller.ModelState.Clear();
            var result2 = controller.IndexQByC(1) as ViewResult;
            Assert.IsNotNull(result2);
            var result3 = result2.Model as List<Question>;
            Assert.AreEqual(db.Questions.Where(x => x.Category_ID == 1).Where(s => s.Status == "Accept").Count(), result3.Count);
        }

        [TestMethod]
        public void countView1()
        {
            var question = db.Questions.First();
            question.CountView += 1;
            var result = controller.countView1(question.ID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void countView()
        {
            var question = db.Questions.First();
            var result = controller.countView(question.ID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void countView2()
        {
            var question = db.Questions.First();
            question.CountView += 0;
            var result = controller.countView2(question.ID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void countView3()
        {
            var question = db.Questions.First();
            var result = controller.countView2(question.ID);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Search()
        {
            var result = controller.Search() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Question>;
            Assert.AreEqual(db.Questions.Where(x => x.Status == "Accept").Where(k => k.Category.Status == true).Count(), result1.Count);
        }

        [TestMethod]
        public void Deny()
        {
            var result = controller.Deny() as ViewResult;
            Assert.IsNotNull(result);
        }

        public class MockHttpContextBase : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }

        [TestMethod]
        public void Create()
        {

            var userToTest = "User";
            string[] roles = null;

            var fakeIdentity = new GenericIdentity(userToTest);
            var principal = new GenericPrincipal(fakeIdentity, roles);
            var fakeHttpContext = new MockHttpContextBase { User = principal };
            var controllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext
            };

            controller.ControllerContext = controllerContext;
            var rand = new Random();
            var question = new Question
            {
                Questioner = fakeIdentity.Name,
                AskQuestion = rand.ToString(),
            };

            using (var scope = new TransactionScope())
            {
                var result = controller.Create(question) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            question.AskQuestion = null;
            controller.ModelState.Clear();
            var result1 = controller.Create(question) as RedirectToRouteResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["AskQuestion"].Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
