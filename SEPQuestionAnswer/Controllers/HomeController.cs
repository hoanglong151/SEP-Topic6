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
        public ActionResult Index()
        {
            var question = db.Questions.Where(x => x.Status == "Accept").Where(x => x.Category.Status == true).OrderByDescending(x => x.CountView).Take(5).ToList();
            return View(question);
        }
        public ActionResult Category()
        {
            var cate = db.Categories.Where(k => k.Status == true).ToList();
            return View(cate);
        }

        public ActionResult Questions(int id)
        {
            var question = db.Questions.Where(k => k.Category_ID == id).ToList();
            return View(question);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
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