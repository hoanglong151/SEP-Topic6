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
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

        public ActionResult UpdateStatus(int id)
        {
            Category category = db.Categories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void Validation(Category category)
        {
            var condition = db.Categories.Where(m => m.CategoryName.Equals(category.CategoryName));

            if(HttpContext.Request.Url.AbsolutePath == "/Admin/Categories/Create")
            {
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "Thông tin không hợp lệ!");
                }

                if (condition.Count() >= 1)
                {
                    ModelState.AddModelError("CategoryName", "Tên đã tồn tại");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "Thông tin không hợp lệ!");
                }

                if (condition.Count() >= 1)
                {
                    ModelState.AddModelError("CategoryName", "Vui lòng nhập tên mới hoặc tên đã tồn tại trong hệ thống");
                }
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
