using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using SEPQuestionAnswer.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using PagedList;

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
            var list = controller.Index(1) as ViewResult;
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Model as PagedList<Student>);           
        }
    }
}
