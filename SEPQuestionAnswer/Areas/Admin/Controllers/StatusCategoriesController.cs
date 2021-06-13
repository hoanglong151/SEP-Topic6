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
    public class StatusCategoriesController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();

        // GET: Admin/StatusCategories
        public ActionResult Index()
        {
            return View(db.StatusCategories.ToList());
        }

        private void CheckValidate(StatusCategory statusCategory)
        {
            var check = db.StatusCategories.FirstOrDefault(s => s.StatusName == statusCategory.StatusName);
            if (string.IsNullOrWhiteSpace(statusCategory.StatusName))
            {
                ModelState.AddModelError("StatusName", "Tên trạng thái không được để trống hoặc nhập kí tự khoảng trắng");
            }
            if(check != null)
            {
                ModelState.AddModelError("StatusName", "Tên trạng thái đã tồn tại");
            }
        }

        // GET: Admin/StatusCategories/Details/5
        public ActionResult Details(int id)
        {
            StatusCategory statusCategory = db.StatusCategories.Find(id);
            if (statusCategory == null)
            {
                return HttpNotFound();
            }
            return View(statusCategory);
        }

        // GET: Admin/StatusCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/StatusCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StatusName")] StatusCategory statusCategory)
        {
            CheckValidate(statusCategory);
            var check = db.StatusCategories.FirstOrDefault(s => s.StatusName == statusCategory.StatusName);
            if (ModelState.IsValid)
            {
                db.StatusCategories.Add(statusCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statusCategory);
        }

        // GET: Admin/StatusCategories/Edit/5
        public ActionResult Edit(int id)
        {
            StatusCategory statusCategory = db.StatusCategories.Find(id);
            if (statusCategory == null)
            {
                return HttpNotFound();
            }
            return View(statusCategory);
        }

        // POST: Admin/StatusCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StatusName")] StatusCategory statusCategory)
        {
            CheckValidate(statusCategory);
            if (ModelState.IsValid)
            {
                db.Entry(statusCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statusCategory);
        }

        // GET: Admin/StatusCategories/Delete/5
        public ActionResult Delete(int id)
        {
            StatusCategory statusCategory = db.StatusCategories.Find(id);
            if (statusCategory == null)
            {
                return HttpNotFound();
            }
            return View(statusCategory);
        }

        // POST: Admin/StatusCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StatusCategory statusCategory = db.StatusCategories.Find(id);
            var check = db.Categories.Where(k => k.StatusCategory_ID == id).Count();
            if(check == 0)
            {
                db.StatusCategories.Remove(statusCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                ModelState.AddModelError("StatusName", "Tình trạng danh mục đang được sử dụng");
                return View(statusCategory);
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
