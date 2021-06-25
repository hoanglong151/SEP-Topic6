using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using SEPQuestionAnswer.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using PagedList;
using System.Linq;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class StudentsControllerTest
    {
        StudentsController controller = new StudentsController();
        SEP24Team10Entities db = new SEP24Team10Entities();
        [TestMethod]
        public void TestIndex()
        {           
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            var result1 = result.Model as List<Student>;
            Assert.AreEqual(db.Students.Count(), result1.Count);
        }
    }
}
