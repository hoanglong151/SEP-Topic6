using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    [Authorize(Roles = "BCN,Admin")]
    public class QuestionsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        // GET: Admin/Questions
        public ActionResult Index()
        {
            var countA = db.Questions.Where(s => s.Status == "Accept").ToList().Count();
            var countP = db.Questions.Where(s => s.Status == "Pending").ToList().Count();
            var countD = db.Questions.Where(s => s.Status == "Disable").ToList().Count();
            var count = db.Questions.ToList().Count();
            var countTop6 = db.Questions.OrderByDescending(c => c.CountView).ToList().Take(6);
            var date = DateTime.Now;
            ViewBag.Total = count;
            ViewBag.TotalA = countA;
            ViewBag.TotalP = countP;
            ViewBag.TotalD = countD;
            ViewBag.CountTop6 = countTop6;
            var questions = db.Questions.Include(q => q.Category)
                .OrderByDescending(s => s.Answer == null).ThenByDescending(s => s.Status == "Pending").ThenByDescending(s => s.Status == "Accept").ThenByDescending(s => s.Date).ToList();
            return View(questions);
        }
        // GET: Admin/Questions/Details/5
        public ActionResult Details(int id)
        {
            Question question = db.Questions.Find(id);
            return View(question);
        }

        // GET: Admin/Questions/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName");
            return View();
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            Validation(question);
            if (ModelState.IsValid)
            {
                question.Date = DateTime.Now;
                question.CountView = 0;
                question.Questioner = User.Identity.Name;
                question.Respondent = User.Identity.Name;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            return View(question);
        }

        // GET: Admin/Questions/Edit/5
        public ActionResult Edit(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            return View(question);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            ValidationEdit(question);
            if (ModelState.IsValid)
            {
                question.Date = DateTime.Now;
                question.Respondent = User.Identity.Name;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            return View(question);
        }

        // GET: Admin/Questions/Delete/5
        public ActionResult Delete(int id)
        {
            Question question = db.Questions.Find(id);
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
        public void Validation(Question question)
        {
            var condition = db.Questions.FirstOrDefault(m => m.AskQuestion == question.AskQuestion);
            var result = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(question.Answer));
            if (string.IsNullOrWhiteSpace(question.AskQuestion))
            {
                ModelState.AddModelError("AskQuestion", "Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng");
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                ModelState.AddModelError("Answer", "Câu trả lời không được để trống hoặc nhập ký tự khoảng trắng");
            }
            if (condition != null)
            {
                ModelState.AddModelError("AskQuestion", "Câu hỏi đã tồn tại");
            }
        }

        public void ValidationEdit(Question question)
        {
            var result = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(question.Answer));
            if (string.IsNullOrWhiteSpace(question.AskQuestion))
            {
                ModelState.AddModelError("AskQuestion", "Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng");
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                ModelState.AddModelError("Answer", "Câu trả lời không được để trống hoặc nhập ký tự khoảng trắng");
            }
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
