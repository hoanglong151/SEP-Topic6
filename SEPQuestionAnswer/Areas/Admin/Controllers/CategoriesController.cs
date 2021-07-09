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
    [Authorize(Roles = "BCN, Admin")]
    public class CategoriesController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            var cate = db.Categories.OrderByDescending(s => s.Status == true).ToList();
            return View(cate);
        }
       
        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            Validation(category);
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            Validation(category);
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult UpdateStatus(int id)
        {
            Category category = db.Categories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void Validation(Category category)
        {
            var condition = db.Categories.FirstOrDefault(m => m.CategoryName == category.CategoryName);
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "Tên danh mục không được để trống hoặc nhập ký tự khoảng trắng");
                }
                if (condition != null)
                {
                    ModelState.AddModelError("CategoryName", "Tên danh mục đã tồn tại");
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
