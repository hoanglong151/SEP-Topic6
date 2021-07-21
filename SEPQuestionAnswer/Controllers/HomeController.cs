using System;
using System.Linq;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Controllers
{
    public class HomeController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        public ActionResult Index()
        {
            var question = db.Questions.OrderByDescending(x => x.CountView).Where(s => s.Status == "Accept").Where(c => c.Category.Status == true).Take(10).ToList();
            if (TempData["fail"] != null)
            {
                ViewBag.fail = TempData["fail"];
            }
            else if (TempData["success"] != null)
            {
                ViewBag.success = TempData["success"];
            }
            return View(question);
        }
        public ActionResult IndexCate()
        {
            var cate = db.Categories.Where(k => k.Status == true).ToList();
            return View(cate);
        }

        public ActionResult IndexQByC(int id)
        {
            var question = db.Questions.Where(k => k.Category_ID == id).Where(s => s.Status == "Accept").ToList();
            return View(question);
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
                view = result.CountView
            });
        }
        public Question countView0(int id)
        {
            var view = db.Questions.Find(id);
            view.CountView += 0;
            db.SaveChanges();
            return view;
        }
        [HttpPost]
        public JsonResult countView2(int id)
        {
            var result = countView0(id);
            return Json(new
            {
                view = result.CountView
            });
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void Validation(Question question)
        {
            if (string.IsNullOrWhiteSpace(question.AskQuestion))
            {
                ModelState.AddModelError("AskQuestion", "Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng");
            }
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            Validation(question);
            if (ModelState.IsValid)
            {
                question.DateCreate = DateTime.Now.ToString("dd/MM/yyyy");
                question.CountView = 0;
                question.Questioner = User.Identity.Name;
                db.Questions.Add(question);
                db.SaveChanges();
                TempData["success"] = "Gửi Câu Hỏi Thành Công";
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "CategoryName", question.Category_ID);
            TempData["fail"] = "Gửi Câu Hỏi Thất Bại. Câu hỏi không được để trống hoặc nhập ký tự khoảng trắng";
            return RedirectToAction("Index");
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