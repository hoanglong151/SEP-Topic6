using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SEPQuestionAnswer.Models;
namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    public class StudentsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        // GET: Admin/Students
        public ActionResult Index(int? page)
        {
            int sizePage = 10;
            int pageNumber = (page ?? 1);
            return View(db.Students.ToList().ToPagedList(pageNumber, sizePage));
        }
    }
}