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
    }
}
