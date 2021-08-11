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
using Microsoft.AspNet.Identity;
using Moq;
using Microsoft.Owin.Security;
using System.IO;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class RoleManagementControllerTest
    {
        [TestMethod]
        public void TestCreateSVOtherG()
        {
            var controller = new RoleManagementController();

            var result = controller.CreateSVOther() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateSVOtherP()
        {
            var controller = new RoleManagementController();
            var db = new SEP24Team10Entities();
            string[] arr = { "long.187pm13956@vanlanguni.vn,trung.187pm14027@vanlanguni.vn" };
            AspNetUser student = new AspNetUser();
            student.Users = arr;
            string user1 = string.Join(",", student.Users);
            string[] userNew = user1.Split(',');
            for(var i = 0; i < userNew.Length; i++)
            {
                var studentOther = new StudentOther
                {
                    Email = userNew[i],
                };
                using (var scope = new TransactionScope())
                {
                    db.StudentOthers.Add(studentOther);
                    db.SaveChanges();
                }
            }
            var result = controller.CreateSVOther(student) as ContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("<script language='javascript' type='text/javascript'>alert(`2 thành viên đã tồn tại vị trí này `);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>", result.Content);

        }

        [TestMethod]
        public void TestCreateG()
        {
            var controller = new RoleManagementController();

            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        private static HttpContext CreateHttpContext(bool userLoggedIn)
        {
            var httpContext = new HttpContext(
                new HttpRequest(string.Empty, "http://sample.com", string.Empty),
                new HttpResponse(new StringWriter())
            )
            {
                User = userLoggedIn
                    ? new GenericPrincipal(new GenericIdentity("userName"), new string[0])
                    : new GenericPrincipal(new GenericIdentity(string.Empty), new string[0])
            };

            return httpContext;
        }

        [TestMethod]
        public void TestCreateP()
        {
            // Arrange
            HttpContext.Current = CreateHttpContext(userLoggedIn: true);
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>();
            var signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);
        }

        [TestMethod]
        public void TestIndexSV()
        {
            var db = new SEP24Team10Entities();
            var controller = new AspNetRolesController();

            var result = controller.IndexSV() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<AspNetRole>;
            Assert.AreEqual(db.AspNetRoles.Count(), result1.Count);
        }

        [TestMethod]
        public void RenderHeader()
        {
            var db = new SEP24Team10Entities();
            var controller = new AspNetRolesController();
            var result = controller.RenderHeader() as PartialViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Question>;
            Assert.AreEqual(db.Questions.Where(s => s.Status == "Pending").Count(), result1.Count);
        }

        [TestMethod]
        public void TestDelete()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            var user = db.AspNetUsers.First();
            var role = db.AspNetRoles.First();

            AspNetRole aspNetRole = db.AspNetRoles.Find(role.Id);
            var result2 = controller.Delete(role.Id, user.Id) as ViewResult;
            Assert.IsNotNull(result2);
            var entity = result2.Model as AspNetRole;
            Assert.AreEqual(aspNetRole.Id, entity.Id);

            role.Id = null;
            var result = controller.Delete(role.Id, user.Id) as HttpStatusCodeResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestDeleteConfirmed()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            var user = db.AspNetUsers.First();
            var role = db.AspNetRoles.First();
            var check = role;
            var check1 = db.StudentOthers.FirstOrDefault(s => s.Email == user.Email);

            using (var scope = new TransactionScope())
            {
                if(check1 != null)
                {
                    StudentOther studentD = db.StudentOthers.Find(check1.ID);
                    db.StudentOthers.Remove(studentD);
                    check.AspNetUsers.Remove(user);
                    var result = controller.DeleteConfirmed(role.Id, user.Id) as RedirectToRouteResult;
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Index", result.RouteValues["action"]);
                }
                else
                {
                    check.AspNetUsers.Remove(user);
                    var result = controller.DeleteConfirmed(role.Id, user.Id) as RedirectToRouteResult;
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Index", result.RouteValues["action"]);
                }
            }

            user.Email = "long1511@gmail.com";
            user.UserName = "long1511@gmail.com";
            check1 = db.StudentOthers.FirstOrDefault(s => s.Email == user.Email);
            using (var scope = new TransactionScope())
            {
                if (check1 == null)
                {
                    check.AspNetUsers.Remove(user);
                    var result = controller.DeleteConfirmed(role.Id, user.Id) as RedirectToRouteResult;
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Index", result.RouteValues["action"]);
                }
                else
                {
                    StudentOther studentD = db.StudentOthers.Find(check1.ID);
                    db.StudentOthers.Remove(studentD);
                    check.AspNetUsers.Remove(user);
                    var result = controller.DeleteConfirmed(role.Id, user.Id) as RedirectToRouteResult;
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Index", result.RouteValues["action"]);
                }
            }
        }
    }
}
