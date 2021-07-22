using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEPQuestionAnswer.Models;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Security.Principal;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class QuestionControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();

            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Question>;
            Assert.AreEqual(db.Questions.Count(), result1.Count);
        }

        [TestMethod]
        public void TestDetails()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var question = db.Questions.First();
            var result1 = controller.Details(question.ID) as ViewResult;
            var entity = result1.Model as Question;
            Assert.IsNotNull(result1);
            Assert.AreEqual(question.Answer, entity.Answer);
            Assert.AreEqual(question.AskQuestion, entity.AskQuestion);
            Assert.AreEqual(question.Category_ID, entity.Category_ID);
            Assert.AreEqual(question.CountView, entity.CountView);
            Assert.AreEqual(question.Questioner, entity.Questioner);
            Assert.AreEqual(question.Respondent, entity.Respondent);
            Assert.AreEqual(question.Status, entity.Status);
        }

        [TestMethod]
        public void TestCreateG()
        {
            var controller = new QuestionsController();
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        public class MockHttpContextBase : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }

        [TestMethod]
        public void TestCreateP()
        {
            var controller = new QuestionsController();
            var rand = new Random();

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

            var question = new Question
            {
                Answer = rand.ToString(),
                AskQuestion = rand.ToString(),
                Category_ID = rand.Next(1,2),
                CountView = 0,
                Status = rand.ToString(),
                Questioner = fakeIdentity.Name,
                Respondent = fakeIdentity.Name
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(question) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            question.Answer = null;
            controller.ModelState.Clear();
            var result1 = controller.Create(question) as ViewResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Câu trả lời không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["Answer"].Errors[0].ErrorMessage);

            question.AskQuestion = null;
            controller.ModelState.Clear();
            var result2 = controller.Create(question) as ViewResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual("Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["AskQuestion"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestEditG()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var result0 = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result0);
            var question = db.Questions.First();
            var result = controller.Edit(question.ID) as ViewResult;
            Assert.IsNotNull(result);
            var entity = result.Model as Question;
            Assert.IsNotNull(entity);
            Assert.AreEqual(question.Answer, entity.Answer);
            Assert.AreEqual(question.AskQuestion, entity.AskQuestion);
            Assert.AreEqual(question.Category_ID, entity.Category_ID);
            Assert.AreEqual(question.CountView, entity.CountView);
            Assert.AreEqual(question.Questioner, entity.Questioner);
            Assert.AreEqual(question.Respondent, entity.Respondent);
            Assert.AreEqual(question.Status, entity.Status);
        }

        [TestMethod]
        public void TestEditP()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
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
            var question = db.Questions.AsNoTracking().First();
            question.Answer = rand.ToString();
            question.AskQuestion = rand.ToString();
            question.Category_ID = rand.Next(1,2);
            question.Status = rand.ToString();
            question.Respondent = fakeIdentity.Name;

            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(question, 1) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            question.Answer = null;
            controller.ModelState.Clear();
            var result1 = controller.Edit(question, 1) as ViewResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Câu trả lời không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["Answer"].Errors[0].ErrorMessage);

            question.AskQuestion = null;
            controller.ModelState.Clear();
            var result2 = controller.Edit(question, 1) as ViewResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual("Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng", controller.ModelState["AskQuestion"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestDelete()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var question = db.Questions.First();
            var result1 = controller.Delete(question.ID) as ViewResult;
            Assert.IsNotNull(result1);
            var entity = result1.Model as Question;
            Assert.IsNotNull(entity);
            Assert.AreEqual(question.Answer, entity.Answer);
            Assert.AreEqual(question.AskQuestion, entity.AskQuestion);
            Assert.AreEqual(question.Category_ID, entity.Category_ID);
            Assert.AreEqual(question.CountView, entity.CountView);
            Assert.AreEqual(question.Questioner, entity.Questioner);
            Assert.AreEqual(question.Respondent, entity.Respondent);
            Assert.AreEqual(question.Status, entity.Status);
        }

        [TestMethod]
        public void TestDeleteConfirm()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var question = db.Questions.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(question.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
