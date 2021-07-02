﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SEPQuestionAnswer.Models;
namespace SEPQuestionAnswer.Areas.Admin.Controllers
{
    public class StudentsController : Controller
    {
        private SEP24Team10Entities db = new SEP24Team10Entities();
        // GET: Admin/Students
        [Authorize(Roles = "BCN")]
        public ActionResult Index()
        {
            var student = db.Students.ToList();
            var question = db.Questions.ToList();
            ViewBag.list = question;
            return View(student);
        }
    }
}