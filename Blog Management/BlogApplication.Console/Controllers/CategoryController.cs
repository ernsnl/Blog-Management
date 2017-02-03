using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Translation;
using BlogApplication.Framework.Extensions.StreamExtensions;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin, UserType.Editor })]
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult CategoryList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.Category.GetCategoryList(page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(Result);
        }

        [HttpGet]
        public ActionResult EditCategory(long ID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            Category model = new Category();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.BlogContent.Category.GetCategory(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("CategoryList");
                }
                model = Result.Data;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCategory(Category Model)
        {
            if (Request["StatusID"] == "on")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            if (Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                Model.Filename = "categoryPhoto_" + Path.GetExtension(Request.Files[0].FileName).ToLower();
                Model.FileData = Request.Files[0].InputStream.ReadFully(0);
            }

            var Result = this.Client.Services.ServiceController.BlogContent.Category.EditCategory(Model);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public ActionResult EditCategoryTranslation(long CategoryID = 0, long LanguageID = 0)
        {
            if (CategoryID > 0)
            {
                Category categoryModel = this.Client.Services.ServiceController.BlogContent.Category.GetCategory(CategoryID).Data;
                CategoryTranslation model = new CategoryTranslation()
                {
                    CategoryID = CategoryID,
                    Category = categoryModel
                };
                if (LanguageID > 0)
                {
                    model = this.Client.Services.ServiceController.BlogContent.Category.GetCategory(CategoryID)
                        .Data.CategoryTranslations.Where(op => op.LanguageID == LanguageID)
                        .FirstOrDefault();
                }
                return View(model);
            }
            else
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Category Cannot Be Empty" } };
                return RedirectToAction("CategoryList");
            }
        }

        [HttpPost]
        public ActionResult EditCategoryTranslation(CategoryTranslation Model)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.Category.EditCategoryTranslation(Model);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }
            return RedirectToAction("EditCategory", "Category", new { ID = Result.Data.CategoryID });
        }

    }
}