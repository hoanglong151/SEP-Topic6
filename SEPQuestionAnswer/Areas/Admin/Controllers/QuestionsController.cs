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
    public class QuestionsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        // GET: Admin/Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Category).Include(q => q.StatusQuestion);
            return View(questions.ToList());
        }

        // GET: Admin/Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        public void CheckValidate(Question question)
        {
            if (string.IsNullOrWhiteSpace(question.AskQuestion))
            {
                ModelState.AddModelError("AskQuestion", "Thông tin nhập không chính xác!");
            }

            if (string.IsNullOrWhiteSpace(question.Answer))
            {
                ModelState.AddModelError("Answer", "Thông tin nhập không chính xác!");
            }
        }

        // GET: Admin/Questions/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName");
            ViewBag.Status_ID = new SelectList(db.StatusQuestions, "ID", "StatusName");
            return View();
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            CheckValidate(question);
            if (ModelState.IsValid)
            {
                question.CountView = 0;
                question.Respondent = User.Identity.Name;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            ViewBag.Status_ID = new SelectList(db.StatusQuestions, "ID", "StatusName", question.Status_ID);
            return View(question);
        }

        // GET: Admin/Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            ViewBag.Status_ID = new SelectList(db.StatusQuestions, "ID", "StatusName", question.Status_ID);            
            return View(question);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            CheckValidate(question);
            if (ModelState.IsValid)
            {                
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            ViewBag.Status_ID = new SelectList(db.StatusQuestions, "ID", "StatusName", question.Status_ID);
            return View(question);
        }

        // GET: Admin/Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Admin/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SortByStatus()
        {
            var model = db.Questions.OrderBy(m => m.Status_ID).ToList();
            return View("Index", model);
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
