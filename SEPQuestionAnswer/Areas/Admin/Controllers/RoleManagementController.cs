using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    [Authorize(Roles = "BCN,Admin")]
    public class RoleManagementController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();


        [Authorize(Roles = "Admin")]
        public ActionResult CreateSVOther()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateSVOther(AspNetUser email)
        {
            string user1 = string.Join(",", email.Users);
            string[] userNew = user1.Split(',');
            for(var i = 0; i < userNew.Length; i++)
            {
                StudentOther student = new StudentOther();
                student.Email = userNew[i];
                db.StudentOthers.Add(student);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "AspNetRoles");
        }


        // GET: Admin/AspNetRoles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.User = new SelectList(db.AspNetUsers, "ID", "Email");
            return View();
        }

        // POST: Admin/AspNetRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string roleId, AspNetUser email)
        {
            for (var i = 0; i < email.Users.Length; i++)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                AspNetRole role = db.AspNetRoles.Find(roleId);
                var user = UserManager.FindById(email.Users[i]);
                    if (role.Name == "Sinh Viên - Giảng Viên")
                    {
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Thành viên đã tồn tại ở vị trí " + role.Name + "');window.location.href='/Admin/AspNetRoles';</script>");
                        }
                        else
                        {
                            UserManager.AddToRole(user.Id, role.Name);
                        }
                    }
                    else
                    {
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Thành viên đã tồn tại ở vị trí " + role.Name + "');window.location.href='/Admin/AspNetRoles';</script>");
                        }
                        else
                        {
                            UserManager.AddToRole(user.Id, role.Name);
                            //return Content("<script language='javascript' type='text/javascript'>alert('Thêm Thành Viên " + role.Name + " Thành Công');window.location.href='/Admin/AspNetRoles';</script>");
                        }
                    } 
            }
            return RedirectToAction("IndexSV", "AspNetRoles");
        }

        // GET: Admin/AspNetRoles/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string roleId, string userId)
        {
            if (roleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(roleId);
            ViewBag.Role = db.AspNetRoles.Find(roleId).Name;
            ViewBag.User = db.AspNetUsers.Find(userId).Email;
            return View(aspNetRole);
        }

        // POST: Admin/AspNetRoles/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string roleId, string userId)
        {
            StudentOther student = new StudentOther();
            AspNetUser aspNetUser = db.AspNetUsers.Find(userId);
            AspNetRole aspNetRole = db.AspNetRoles.Find(roleId);
            var check = aspNetRole;
            var check1 = db.StudentOthers.FirstOrDefault(s => s.Email == aspNetUser.Email);
            if(check1 == null)
            {
                check.AspNetUsers.Remove(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index", "AspNetRoles");
            }
            else
            {
                StudentOther studentD = db.StudentOthers.Find(check1.ID);
                db.StudentOthers.Remove(studentD);
                check.AspNetUsers.Remove(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index", "AspNetRoles");
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
