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
using System.Web.Helpers;
using System.Net.Mail;
using System.Text;

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
        public void TestDashboard()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var result = controller.Dashboard(null) as ViewResult;
            Assert.IsNotNull(result);
            var entity = result.Model as List<Question>;
            Assert.AreEqual(0, entity.Count);

            controller.ModelState.Clear();
            var cate = db.Categories.FirstOrDefault();
            var result1 = controller.Dashboard(cate.ID) as ViewResult;
            Assert.IsNotNull(result1);
            var question = db.Questions.Where(q => q.Category_ID == cate.ID).ToList();
            Assert.AreEqual(cate.CountQuestion, question.Count);
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

            question.AskQuestion = "Học phí của các ngành khoa Công nghệ thông tin năm 2018";
            question.Answer = "blabla";
            controller.ModelState.Clear();
            var result3 = controller.Create(question) as ViewResult;
            Assert.IsNotNull(result3);
            Assert.AreEqual("Câu hỏi đã tồn tại", controller.ModelState["AskQuestion"].Errors[0].ErrorMessage);
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

            question.AskQuestion = "Test011";
            question.Answer = "Test022";
            question.Status = "Accept";
            question.SendMail = false;
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("faqteam10@gmail.com");
                mail.To.Add(question.Questioner);
                mail.Subject = "Câu hỏi của bạn đã được Khoa trả lời";
                mail.Body = "Thân gửi bạn," + "<br/>" + "<br/>"
                    + "Chúc mừng bạn, câu hỏi của bạn đã được Ban Chủ Nhiệm Khoa Trả lời."
                    + "<br/>" + "Nội dung chi tiết câu hỏi:"
                    + "<br/>" + "Câu hỏi: " + question.AskQuestion
                    + "<br/>" + "Câu trả lời: " + question.Answer
                    + "<br/>" + "Địa chỉ Website:"
                    + "<a style='text-decoration: none;font-size: 16px;font-weight: 500;color: red' href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/'> Tại Đây<a/>"
                    + "<br/><br/>" + "Trân trọng cảm ơn."
                    + "<br/>" + "FAQ Website";
                mail.BodyEncoding = Encoding.GetEncoding("utf-8");
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("faqteam10@gmail.com", "Team10K24T");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                var result3 = controller.Edit(question, 1) as RedirectToRouteResult;
                Assert.IsNotNull(result3);
                Assert.AreEqual("Index", result3.RouteValues["action"]);
            }

            controller.ModelState.Clear();
            var cate1 = db.Categories.OrderByDescending(s => s.ID).FirstOrDefault();
            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(question, cate1.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
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

            var question1 = db.Questions.AsNoTracking().OrderByDescending(s => s.ID).FirstOrDefault();
            var check = db.Categories.FirstOrDefault(s => s.ID == question1.Category_ID);
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                var result1 = controller.DeleteConfirmed(question1.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result1);
                Assert.AreEqual("Index", result1.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void TestDeleteConfirmCateNull()
        {
            var db = new SEP24Team10Entities();
            var controller = new QuestionsController();
            var question = db.Questions.AsNoTracking().OrderByDescending(s => s.ID).First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(question.ID) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
