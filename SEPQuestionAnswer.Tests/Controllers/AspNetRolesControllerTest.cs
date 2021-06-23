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
    public class AspNetRolesControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var db = new SEP24Team10Entities();
            var controller = new AspNetRolesController();

            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<AspNetRole>;
            Assert.AreEqual(db.AspNetRoles.Count(), result1.Count);
        }

        [TestMethod]
        public void TestCreateG()
        {
            var controller = new AspNetRolesController();
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateP()
        {
            var controller = new AspNetRolesController();
            var rand = new Random();
            var role = new AspNetRole
            {
                Name = rand.ToString()
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(role) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void TestEditG()
        {
            var db = new SEP24Team10Entities();
            var controller = new AspNetRolesController();
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
            var controller = new AspNetRolesController();

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
            var controller = new AspNetRolesController();
            var result = controller.Delete("0") as HttpNotFoundResult;
            Assert.IsNotNull(result);
            var role = db.AspNetRoles.First();
            var result1 = controller.Delete(role.Id) as ViewResult;
            Assert.IsNotNull(result1);
            var entity = result1.Model as AspNetRole;
            Assert.IsNotNull(entity);
            Assert.AreEqual(role.Name, entity.Name);
        }

        [TestMethod]
        public void TestDeleteConfirm()
        {
            var db = new SEP24Team10Entities();
            var controller = new AspNetRolesController();
            var role = db.AspNetRoles.AsNoTracking().First();
            using (var scope = new TransactionScope())
            {
                var result = controller.DeleteConfirmed(role.Id) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }
        }
    }
}
