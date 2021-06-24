using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Controllers
{
    public class HomeController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        private int compare = 0;
        public ActionResult Index()
        {
            var question = db.Questions.OrderByDescending(x => x.CountView).Take(10).ToList();
            return View(question);
        }
        public ActionResult IndexCate()
        {
            var cate = db.Categories.Where(k => k.Status == true).ToList();
            return View(cate);
        }

        public ActionResult IndexQByC(int id)
        {
            var question = db.Questions.Where(k => k.Category_ID == id).ToList();
            return View(question);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public Question countView1(int id)
        {
            var view = db.Questions.Find(id);
            view.CountView += 1;
            db.SaveChanges();
            return view;
        }
        [HttpPost]
        public JsonResult countView(int id)
        {
            var result = countView1(id);
            return Json(new
            {
                view = result
            });
        }
        public Question countView2(int id)
        {
            var view = db.Questions.Find(id);
            view.CountView += 0;
            db.SaveChanges();
            return view;
        }
        [HttpPost]
        public JsonResult countView3(int id)
        {
            var result = countView2(id);
            return Json(new
            {
                view = result
            });
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                question.CountView = 0;
                question.Questioner = User.Identity.Name;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            return View(question);
        }
        public ActionResult Category()
        {
            ViewBag.Message = "Your Category page.";
            return View();
        }
        public ActionResult Search()
        {
            ViewBag.Message = "Your Search page.";
            var question = db.Questions.Where(x => x.Status == "Accept").Where(k => k.Category.Status == true).ToList();
            return View(question);
        }

        public ActionResult Deny()
        {
            return View();
        }
    }
}