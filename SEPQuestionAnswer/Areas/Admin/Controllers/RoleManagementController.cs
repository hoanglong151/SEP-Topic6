﻿using System;
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
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        
        // GET: Admin/AspNetRoles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {                      
            return View();
        }

        // POST: Admin/AspNetRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string roleId, string email)
        {           
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            AspNetRole role = db.AspNetRoles.Find(roleId);
            var user = UserManager.FindByName(email);

            var exists = db.AspNetUsers.ToList().Exists(e => e.Email == email);

            if (exists == false)
            {
                return Content($"<script language='javascript' type='text/javascript'>alert('Không tìm thấy email trên hệ thống. Thử lại!');window.location.href='/Admin/RoleManagement/Create?roleId={roleId}';</script>");
            }
            else
            {
                if (role.Name == "Sinh Viên - Giảng Viên")
                {
                    if (UserManager.IsInRole(user.Id, role.Name))
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Thành viên đã tồn tại ở vị trí " + role.Name + "');window.location.href='/Admin/AspNetRoles';</script>");
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, role.Name);
                        Student student = new Student();
                        student.Email = user.Email;
                        db.Students.Add(student);
                        return Content("<script language='javascript' type='text/javascript'>alert('Thêm Thành Viên " + role.Name + " Thành Công');window.location.href='/Admin/AspNetRoles';</script>");
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
                        return Content("<script language='javascript' type='text/javascript'>alert('Thêm Thành Viên " + role.Name + " Thành Công');window.location.href='/Admin/AspNetRoles';</script>");
                    }
                }
            }                                  
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
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = db.AspNetRoles.Find(roleId).Name;
            ViewBag.User = db.AspNetUsers.Find(userId).Email;
            return View(aspNetRole);
        }

        // POST: Admin/AspNetRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string roleId, string userId)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(userId);
            AspNetRole aspNetRole = db.AspNetRoles.Find(roleId);
            var check = aspNetRole;
            check.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Index", "AspNetRoles");
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
