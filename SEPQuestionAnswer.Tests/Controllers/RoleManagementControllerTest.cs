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
using System.Net;
using Microsoft.AspNet.Identity;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class RoleManagementControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();

            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<AspNetRole>;
            Assert.AreEqual(db.AspNetRoles.Count(), result1.Count);
        }

        [TestMethod]
        public void TestDetails()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            var result = controller.Details("0") as HttpNotFoundResult;
            Assert.IsNotNull(result);
            var role = db.AspNetRoles.First();
            var result1 = controller.Details(role.Id) as ViewResult;
            var entity = result1.Model as AspNetRole;
            Assert.IsNotNull(result1);
            Assert.AreEqual(role.Name, entity.Name);
        }

        [TestMethod]
        public void TestCreateG()
        {
            var controller = new RoleManagementController();
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        public class MockHttpContextBase : HttpContextBase
        {
            public override IPrincipal User { get; set; }
        }
        public class ApplicationUserManager : UserManager<ApplicationUser>
        {
            public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
            {
                // configuration-blah-blah
            }
        }
        [TestMethod]
        public void TestCreateP()
        {
            //var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = new ApplicationUser { User = new Email { UserId = 1 } };

            //var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            //store.As<IUserStore<ApplicationUser>>().Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            //UserManager = new ApplicationUserManager(store.Object);
            var db = new SEP24Team10Entities();
            var user = db.AspNetUsers.ToList();
            var check = user;
        }

        [TestMethod]
        public void TestEditG()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            var result0 = controller.Edit("0") as HttpNotFoundResult;
            Assert.IsNotNull(result0);
            var role = db.AspNetRoles.First();
            var result = controller.Edit(role.Id) as ViewResult;
            Assert.IsNotNull(result);
            var entity = result.Model as AspNetRole;
            Assert.IsNotNull(entity);
            Assert.AreEqual(role.Name, entity.Name);
        }

        [TestMethod]
        public void TestEditP()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();

            var rand = new Random();
            var role = db.AspNetRoles.AsNoTracking().First();
            role.Name = rand.ToString();

            using (var scope = new TransactionScope())
            {
                var result = controller.Edit(role) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            //role.Name = null;
            //controller.ModelState.Clear();
            //var result1 = controller.Edit(role) as ViewResult;
            //Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void TestDelete()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            var role = db.AspNetRoles.First();
            var user = db.AspNetUsers.First();
            var result0 = controller.Delete(null, "1") as HttpStatusCodeResult;
            Assert.IsNotNull(result0);
            Assert.AreEqual(HttpStatusCode.BadRequest, System.Net.HttpStatusCode.BadRequest);
            var result1 = controller.Delete("0", "1") as HttpNotFoundResult;
            Assert.IsNotNull(result1);
            var result = controller.Delete(role.Id, user.Id) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestDeleteConfirm()
        {
            var db = new SEP24Team10Entities();
            var controller = new RoleManagementController();
            //var user = db.AspNetUsers.AsNoTracking().First();
            var role = db.AspNetRoles.First();
            var user = role.AspNetUsers.FirstOrDefault();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(role.Id ,user.Id) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
