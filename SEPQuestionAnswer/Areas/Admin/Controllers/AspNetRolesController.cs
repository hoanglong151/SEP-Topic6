using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Ban Chủ Nhiệm, Quản Trị Viên")]
    public class AspNetRolesController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        // GET: Admin/AspNetRoles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
        }
        public ActionResult IndexSV()
        {
            var question = db.Questions.ToList();
            ViewBag.list = question;
            return View(db.AspNetRoles.ToList());
        }

        [ChildActionOnly]
        public ActionResult RenderHeader()
        {
            var countP = db.Questions.Where(s => s.Status == "Pending").Count();
            var listP = db.Questions.Where(s => s.Status == "Pending").OrderByDescending(d => d.Category_ID == null).ThenByDescending(d => d.Date).ToList();
            ViewBag.count = countP;
            ViewBag.listP = listP;
            return PartialView("_AdminNotification",listP);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
