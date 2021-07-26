using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SEPQuestionAnswer.Models;

namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Ban Chủ Nhiệm,Quản Trị Viên")]
    public class RoleManagementController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();


        [Authorize(Roles = "Quản Trị Viên")]
        public ActionResult CreateSVOther()
        {
            return View();
        }

        [Authorize(Roles = "Quản Trị Viên")]
        [HttpPost]
        public ActionResult CreateSVOther(AspNetUser email)
        {
            var countSuccess = 0;
            var countFailE = 0;
            var countExist = 0;
            string user1 = string.Join(",", email.Users);
            string[] userNew = user1.Split(',');
            for(var i = 0; i < userNew.Length; i++)
            {
                var user = userNew[i];
                var condition = db.StudentOthers.FirstOrDefault(s => s.Email == user);
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (re.IsMatch(userNew[i]) && condition == null)
                {
                    countSuccess += 1;
                    StudentOther student = new StudentOther();
                    student.Email = userNew[i];
                    db.StudentOthers.Add(student);
                    db.SaveChanges();
                }
                else if(!re.IsMatch(userNew[i]))
                {
                    countFailE += 1;
                }
                else
                {
                    countExist += 1;
                }
            }
            if(countFailE > 0 && countSuccess > 0 && countExist > 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countFailE + " thành viên sai định dạng Email \n" + countExist + " thành viên đã tồn tại vị trí này \n" + countSuccess + " thành viên đã được thêm vào vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if (countFailE > 0 && countExist > 0 && countSuccess == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countFailE + " thành viên sai định dạng Email \n" + countExist + " thành viên đã tồn tại vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if (countFailE > 0 && countExist == 0 && countSuccess > 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countFailE + " thành viên sai định dạng Email \n" + countSuccess + " thành viên đã được thêm vào vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if (countFailE == 0 && countExist > 0 && countSuccess > 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countExist + " thành viên đã tồn tại vị trí này \n" + countSuccess + " thành viên đã được thêm vào vị trí này`);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if (countFailE == 0 && countExist > 0 && countSuccess == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countExist + " thành viên đã tồn tại vị trí này `);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            else if (countFailE > 0 && countExist == 0 && countSuccess == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert(`" + countFailE + " thành viên sai định dạng Email `);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert(`" + countSuccess + " thành viên đã được thêm thành công vào vị trí này `);window.location.href='http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Admin/AspNetRoles';</script>");
        }


        // GET: Admin/AspNetRoles/Create
        [Authorize(Roles = "Quản Trị Viên")]
        public ActionResult Create()
        {
            ViewBag.User = new SelectList(db.AspNetUsers, "ID", "Email");
            return View();
        }

        // POST: Admin/AspNetRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Quản Trị Viên")]
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
        [Authorize(Roles = "Quản Trị Viên")]
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
        [Authorize(Roles = "Quản Trị Viên")]
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
