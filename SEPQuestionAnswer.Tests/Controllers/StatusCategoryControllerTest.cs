using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SEPQuestionAnswer.Models;
using SEPQuestionAnswer.Areas.Admin.Controllers;
using System.Transactions;

namespace SEPQuestionAnswer.Tests.Controllers
{
    [TestClass]
    public class StatusCategoryControllerTest
    {
        StatusCategoriesController controller = new StatusCategoriesController();
        SEP24Team10Entities db = new SEP24Team10Entities();
        [TestMethod]
        public void TestIndex()
        {
            var list = controller.Index() as ViewResult;
            Assert.IsNotNull(list);

            var model = list.Model as List<StatusCategory>;
            Assert.IsNotNull(model);

        }
        [TestMethod]
        public void TestDetail()
        {
            var db = new SEP24Team10Entities();
            var controller = new StatusCategoriesController();
            var result = controller.Details(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var statusCategory = db.StatusCategories.FirstOrDefault();
            var result1 = controller.Details(statusCategory.ID) as ViewResult;
            Assert.IsNotNull(result);

            var model = result1.Model as StatusCategory;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.StatusName, statusCategory.StatusName);
        }

        [TestMethod]
        public void TestCreateView()
        {
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestCreateS()
        {
            var rand = new Random();
            var controller = new StatusCategoriesController();
            var status = new StatusCategory
            {
                StatusName = rand.Next().ToString()
            };
            using (var scope = new TransactionScope())
            {
                var result = controller.Create(status) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }


            status.StatusName = "Hoạt động";
            var check = db.StatusCategories.AsNoTracking().FirstOrDefault(s => s.StatusName == status.StatusName);
            var result3 = controller.Create(status) as ViewResult;
            Assert.AreEqual("Tên trạng thái đã tồn tại", controller.ModelState["StatusName"].Errors[0].ErrorMessage);
            Assert.IsNotNull(result3);

            status.StatusName = null;
            controller.ModelState.Clear();

            var result1 = controller.Create(status) as ViewResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Tên trạng thái không được để trống hoặc nhập kí tự khoảng trắng", controller.ModelState["StatusName"].Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void TestEditView()
        {
            var db = new SEP24Team10Entities();
            var controller = new StatusCategoriesController();
            var result = controller.Edit(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);
            var statusCategory = db.StatusCategories.FirstOrDefault();
            var result1 = controller.Edit(statusCategory.ID) as ViewResult;
            Assert.IsNotNull(result);

            var model = result1.Model as StatusCategory;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.StatusName, statusCategory.StatusName);
        }

        [TestMethod]
        public void TestEditS()
        {
            var db = new SEP24Team10Entities();
            var status = db.StatusCategories.AsNoTracking().First();
            var rand = new Random();
            var controller = new StatusCategoriesController();
            using (var scope = new TransactionScope())
            {
                status.StatusName = "Hoạt động";
                var result = controller.Edit(status) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            status.StatusName = rand.Next().ToString();
            controller.ModelState.Clear();
            using (var scope = new TransactionScope())
            {
                status.StatusName = rand.Next().ToString();
                var result = controller.Edit(status) as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }


            status = db.StatusCategories.AsNoTracking().OrderByDescending(x => x.ID).First();
            using (var scope = new TransactionScope())
            {
                status.StatusName = "Hoạt động";
                var result = controller.Edit(status) as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Tên trạng thái đã tồn tại", controller.ModelState["StatusName"].Errors[0].ErrorMessage);
            }

            status.StatusName = null;
            controller.ModelState.Clear();

            var result1 = controller.Edit(status) as ViewResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual("Tên trạng thái không được để trống hoặc nhập kí tự khoảng trắng", controller.ModelState["StatusName"].Errors[0].ErrorMessage);

        }
        [TestMethod]
        public void TestDeleteView()
        {
            var db = new SEP24Team10Entities();
            var controller = new StatusCategoriesController();
            var result = controller.Delete(0) as HttpNotFoundResult;
            Assert.IsNotNull(result);

            var statusCategory = db.StatusCategories.FirstOrDefault();
            var result1 = controller.Delete(statusCategory.ID) as ViewResult;
            Assert.IsNotNull(result);

            var model = result1.Model as StatusCategory;
            Assert.IsNotNull(model);
            Assert.AreEqual(model.StatusName, statusCategory.StatusName);
        }
        [TestMethod]
        public void TestDeleteS()
        {
            var db = new SEP24Team10Entities();
            var controller = new StatusCategoriesController();

            //Status exist
            var status = db.StatusCategories.AsNoTracking().FirstOrDefault();
            var check = db.Categories.Where(x => x.StatusCategory_ID == status.ID).Count();
            using (var scope = new TransactionScope())
            {
                if (check == 0)
                {
                    var delete = controller.DeleteConfirmed(status.ID) as RedirectToRouteResult;
                    Assert.IsNotNull(delete);
                    Assert.AreEqual("Index", delete.RouteValues["action"]);
                    var entity = db.StatusCategories.Find(status.ID);
                    Assert.IsNull(entity);
                }
                else
                {
                    var delete = controller.DeleteConfirmed(status.ID) as ViewResult;
                    Assert.IsNotNull(delete);
                    Assert.AreEqual("Tình trạng danh mục đang được sử dụng", controller.ModelState["StatusName"].Errors[0].ErrorMessage);
                    var entity = db.StatusCategories.Find(status.ID);
                    Assert.IsNotNull(entity);
                }
            }

            //Status not exist
            status = db.StatusCategories.AsNoTracking().OrderByDescending(x => x.ID).FirstOrDefault();
            check = db.Categories.Where(x => x.StatusCategory_ID == status.ID).Count();
            using (var scope = new TransactionScope())
            {
                if (check == 0)
                {
                    var delete = controller.DeleteConfirmed(status.ID) as RedirectToRouteResult;
                    Assert.IsNotNull(delete);
                    Assert.AreEqual("Index", delete.RouteValues["action"]);
                    var entity = db.StatusCategories.Find(status.ID);
                    Assert.IsNull(entity);
                }
                else
                {
                    var delete = controller.DeleteConfirmed(status.ID) as ViewResult;
                    Assert.IsNotNull(delete);
                    Assert.AreEqual("StatusName tồn tại trong hệ thống", controller.ModelState["StatusName"].Errors[0].ErrorMessage);
                    var entity = db.StatusCategories.Find(status.ID);
                    Assert.IsNotNull(entity);
                }
            }
        }
    }
}
