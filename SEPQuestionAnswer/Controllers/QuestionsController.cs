using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;


namespace SEPQuestionAnswer.Controllers
{
    public class QuestionsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        // GET: Questions
        public ActionResult Index()
        {
            var question = db.Questions.Where(q => q.Status == "Accept").Where(q => q.Category.Status == true).OrderByDescending(q=> q.ID).ToList();
            return View(question);
        }

        public ActionResult Details(int id)
        {
            Question question = db.Questions.Find(id);
            question.CountView += 1;
            db.SaveChanges();
            return View(question);
        }
    }
}