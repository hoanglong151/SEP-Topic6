using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Ban Chủ Nhiệm,Quản Trị Viên")]
    public class QuestionsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        // GET: Admin/Questions
        public ActionResult Index()
        {
            if (TempData["success"] != null)
            {
                ViewBag.success = TempData["success"];
            }
            var questions = db.Questions.Include(q => q.Category)
                .OrderByDescending(s => s.Answer == null).ThenByDescending(s => s.Status == "Pending").ThenByDescending(s => s.Status == "Accept").ThenByDescending(s => s.DateCreate).ToList();
            return View(questions);
        }

        public ActionResult Dashboard(int? id)
        {
            var countA = db.Questions.Where(s => s.Status == "Accept").ToList().Count();
            var countP = db.Questions.Where(s => s.Status == "Pending").ToList().Count();
            var countD = db.Questions.Where(s => s.Status == "Disable").ToList().Count();
            var count = db.Questions.ToList().Count();
            ViewBag.Total = count;
            ViewBag.TotalA = countA;
            ViewBag.TotalP = countP;
            ViewBag.TotalD = countD;
            var cate = db.Categories.OrderByDescending(c => c.CountQuestion).ToList().Take(5);
            ViewBag.countQ = cate;
            var question = db.Questions.Where(k => k.Category_ID == id).Where(c => c.Category_ID != null).OrderByDescending(s => s.Answer == null).ThenByDescending(s => s.Status == "Pending").ThenByDescending(s => s.Status == "Accept").ThenByDescending(s => s.DateCreate).ToList();
            return View(question);
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
            var check = db.Categories.FirstOrDefault(s => s.ID == question.Category_ID);
            Validation(question);
            if (ModelState.IsValid)
            {
                question.SendMail = true;
                question.Date = DateTime.Now;
                question.DateCreate = DateTime.Now.ToString("dd/MM/yyyy");
                question.CountView = 0;
                question.Questioner = User.Identity.Name;
                question.Respondent = User.Identity.Name;
                db.Questions.Add(question);

                check.CountQuestion = check.Questions.Count();
                db.Entry(check).State = EntityState.Modified;
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
        public ActionResult Edit(Question question, int? old)
        {
            ValidationEdit(question);
            var checkCate_old = db.Categories.FirstOrDefault(s => s.ID == old);
            var checkCate_new = db.Categories.FirstOrDefault(s => s.ID == question.Category_ID);
            if (ModelState.IsValid)
            {
                if (question.Status == "Accept" && question.SendMail != true)
                {
                    question.SendMail = true;
                    question.Date = DateTime.Now;
                    question.DateUpdate = DateTime.Now.ToString("dd/MM/yyyy");
                    question.Respondent = User.Identity.Name;
                    string subject = "Câu hỏi của bạn đã được Khoa trả lời";
                    string body = "Thân gửi bạn," + "<br/>" + "<br/>"
                        + "Chúc mừng bạn, câu hỏi của bạn đã được Ban Chủ Nhiệm Khoa Trả lời."
                        + "<br/>" + "Nội dung chi tiết câu hỏi:"
                        + "<br/>" + "Câu hỏi: " + question.AskQuestion
                        + "<br/>" + "Câu trả lời: " + question.Answer
                        + "<br/>" + "Địa chỉ Website:"
                        + "<a style='text-decoration: none;font-size: 16px;font-weight: 500;color: red' href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/'> Tại Đây<a/>"
                        + "<br/><br/>" + "Trân trọng cảm ơn."
                        + "<br/>" + "FAQ Website";
                    WebMail.Send(question.Questioner, subject, body, from: "FAQ K24T <faqteam10@gmail.com>", null, null, true, null, null, null, null, null, null);
                    TempData["success"] = "Gửi Mail Thành Công";
                }
                else
                {
                    question.Date = DateTime.Now;
                    question.DateUpdate = DateTime.Now.ToString("dd/MM/yyyy");
                    question.Respondent = User.Identity.Name;
                }
                db.Entry(question).State = EntityState.Modified;
                if (checkCate_old != null && checkCate_new.ID != checkCate_old.ID)
                {
                    checkCate_old.CountQuestion -= 1;
                    db.Entry(checkCate_old).State = EntityState.Modified;
                    checkCate_new.CountQuestion = checkCate_new.Questions.Count();
                    db.Entry(checkCate_new).State = EntityState.Modified;
                }
                else
                {
                    checkCate_new.CountQuestion = checkCate_new.Questions.Count();
                    db.Entry(checkCate_new).State = EntityState.Modified;
                }
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
            var check = db.Categories.FirstOrDefault(s => s.ID == question.Category_ID);
            if(check == null)
            {
                db.Questions.Remove(question);
            }
            else
            {
                db.Questions.Remove(question);
                check.CountQuestion = check.Questions.Count();
                db.Entry(check).State = EntityState.Modified;
            }
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
