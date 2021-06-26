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
    public class AspNetRolesController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        [Authorize(Roles = "Admin, BCN")]
        // GET: Admin/AspNetRoles
        public ActionResult Index()
        {
            return View(db.AspNetRoles.ToList());
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
