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
    public class CategoriesController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        // GET: Admin/Categories
        [Authorize(Roles = "BCN")]
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.StatusCategory);
            return View(categories.ToList());
        }

        private void CheckValidate(Category category)
        {
            var check = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Tên danh muc không được để trống hoặc nhập ký tự khoảng trắng");
            }
            if (check != null)
            {
                ModelState.AddModelError("CategoryName", "Tên danh muc đã tồn tại");
            }
        }
        // GET: Admin/Categories/Create
        [Authorize(Roles = "BCN")]
        public ActionResult Create()
        {
            ViewBag.StatusCategory_ID = new SelectList(db.StatusCategories, "ID", "StatusName");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            CheckValidate(category);
            var check = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusCategory_ID = new SelectList(db.StatusCategories, "ID", "StatusName", category.StatusCategory_ID);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        [Authorize(Roles = "BCN")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusCategory_ID = new SelectList(db.StatusCategories, "ID", "StatusName", category.StatusCategory_ID);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            CheckValidate(category);
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusCategory_ID = new SelectList(db.StatusCategories, "ID", "StatusName", category.StatusCategory_ID);
            return View(category);
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
