using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (re.IsMatch(userNew[i]))
                {
                    StudentOther student = new StudentOther();
                    student.Email = userNew[i];
                    db.StudentOthers.Add(student);
                    db.SaveChanges();
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Sai Định Dạng Email');window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
                }
            }
            return RedirectToAction("IndexSV", "AspNetRoles");
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
            var count = 0;
            var countNew = 0;
            for (var i = 0; i < email.Users.Length; i++)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                AspNetRole role = db.AspNetRoles.Find(roleId);
                var user = UserManager.FindById(email.Users[i]);
                    if (role.Name == "Sinh Viên - Giảng Viên")
                    {
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            count += 1;
                        }
                        else
                        {
                            countNew += 1;
                            UserManager.AddToRole(user.Id, role.Name);
                        }
                    }
                    else
                    {
                        if (UserManager.IsInRole(user.Id, role.Name))
                        {
                            count += 1;
                        }
                        else
                        {
                            countNew += 1;
                            UserManager.AddToRole(user.Id, role.Name);
                        }
                    } 
            }
            if(count > 0 && countNew > 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`"+ count +" thành viên đã tồn tại ở vị trí này \n"+countNew +" thành viên đã được thêm vào vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if(count > 0 && countNew == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + count + " thành viên đã tồn tại ở vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert(`"+countNew +" thành viên đã được thêm vào vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
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
