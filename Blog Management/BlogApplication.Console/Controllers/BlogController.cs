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

    public class BlogController : BaseController
    {


        #region Blog

        public ActionResult BlogList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, false, false, false, page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(Result);
        }

        [HttpGet]
        public ActionResult EditBlog(long ID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            BlogContent model = new BlogContent();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("BlogList");
                }
                model = Result.Data;
                model.BlogText = model.BlogDecoded;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditBlog(BlogContent Model)
        {
            if (Request["StatusID"] == "on")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            if (Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                Model.BlogCoverImgName = "blogPhoto_" + Path.GetExtension(Request.Files[0].FileName).ToLower();
                Model.CoverImgData = Request.Files[0].InputStream.ReadFully(0);
            }
            Model.HtmlContent = Model.BlogEncoded;
            Model.BlogStatus = VariableValue.WaitingReview;
            var Result = this.Client.Services.ServiceController.BlogContent.Blog.EditBlogContent(Model);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }

            return RedirectToAction("BlogList");
        }

        [HttpGet]
        public ActionResult EditBlogTranslation(long BlogID = 0, long LanguageID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (BlogID > 0)
            {
                var Blog = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(BlogID);
                if (Blog.HasFailed)
                {
                    TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Blog Cannot Be Empty" } };
                    return RedirectToAction("BlogList");
                }
                BlogTranslation model = new BlogTranslation()
                {
                    BlogContent = Blog.Data,
                    TranslationHtml = Blog.Data.HtmlContent,


                };
                model.TranslationText = model.TranslationTextDecoded;
                if (LanguageID > 0)
                {
                    model = Blog.Data.BlogTranslations.Where(op => op.LanguageID == LanguageID).FirstOrDefault();
                    if (model != null)
                    {
                        model.BlogContent = Blog.Data;
                        model.TranslationText = model.TranslationTextDecoded;
                    }
                    else
                    {
                        return RedirectToAction("EditBlogTranslation", "Blog", new { @BlogID = BlogID });
                    }
                }
                return View(model);
            }
            else
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Category Cannot Be Empty" } };
                return RedirectToAction("BlogList");
            }
        }

        [HttpPost]
        public ActionResult EditBlogTranslation(BlogTranslation Model)
        {
            Model.TranslationHtml = Model.TranslationTextEncoded;
            var Result = this.Client.Services.ServiceController.BlogContent.Blog.EditBlogContentTranslation(Model);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("EditBlogTranslation", "Blog", new { @BlogID = Model.BlogID, @LanguageID = Model.LanguageID });
            }
            return RedirectToAction("EditBlog", "Blog", new { ID = Result.Data.BlogID });
        }

        [HttpGet]
        public ActionResult EditBlogSEOInformation(long BlogID = 0, long LanguageID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (BlogID > 0)
            {
                var Blog = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(BlogID);
                if (Blog.HasFailed)
                {
                    TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Blog Cannot Be Empty" } };
                    return RedirectToAction("BlogList");
                }
                BlogSEOInformation model = new BlogSEOInformation() { BlogContent = Blog.Data };
                if (LanguageID > 0)
                {
                    model = Blog.Data.BlogSEOInformations.Where(op => op.LanguageID == LanguageID).FirstOrDefault();
                    if (model == null)
                        return RedirectToAction("EditBlogTranslation", "Blog", new { @BlogID = BlogID });

                }
                return View(model);
            }
            else
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Blog Cannot Be Empty" } };
                return RedirectToAction("BlogList");
            }
        }

        [HttpPost]
        public ActionResult EditBlogSEOInformation(BlogSEOInformation Model)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.Blog.EditBlogSEOInformation(Model);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("EditBlogSEOInformation", "Blog", new { @BlogID = Model.BlogID, @LanguageID = Model.LanguageID });
            }
            return RedirectToAction("EditBlog", "Blog", new { ID = Result.Data.BlogID });
        }

        [HttpGet]
        public ActionResult BlogReview(long ID)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            BlogContent model = new BlogContent();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("BlogList");
                }
                model = Result.Data;
                model.BlogText = model.BlogDecoded;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        public ActionResult BlogReview(BlogContent BlogContent)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            var Result = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(BlogContent.ID);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("BlogList");
            }
            BlogContent = Result.Data;

            if (Convert.ToByte(Request["ReviewResult"]) == 0)
                BlogContent.BlogStatus = Convert.ToByte(BlogStatusType.Denied);
            else if (Convert.ToByte(Request["ReviewResult"]) == 1)
            {
                BlogContent.BlogStatus = Convert.ToByte(BlogStatusType.Published);
                BlogContent.PublishDate = DateTime.Now;
            }

            var blogResult = this.Client.Services.ServiceController.BlogContent.Blog.EditBlogContent(BlogContent);
            if (blogResult.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return View(BlogContent);
            }

            BlogReview review = new BlogReview();
            review.BlogID = BlogContent.ID;
            review.ReviewMessage = Request["ReviewMessage"];

            var blogReviewResult = this.Client.Services.ServiceController.BlogContent.Blog.WriteReview(review);
            if (blogReviewResult.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return View(BlogContent);
            }

            return RedirectToAction("BlogList");
        }

        #endregion

        #region CustomPage

        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult CustomPageList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPageList(page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(Result);
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditCustomPage(long ID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            CustomPageContent model = new CustomPageContent();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPage(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("BlogList");
                }
                model = Result.Data;
                model.CustomPageText = model.CustomPageDecoded;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditCustomPage(CustomPageContent Model)
        {
            if (Request["StatusID"] == "on")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            Model.HtmlContent = Model.CustomPageEncoded;
            var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.EditCustomPageContent(Model);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }

            return RedirectToAction("CustomPageList");
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditCustomPageTranslation(long CustomPageID = 0, long LanguageID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (CustomPageID > 0)
            {
                var CustomPage = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPage(CustomPageID);
                if (CustomPage.HasFailed)
                {
                    TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Custom Page Cannot Be Empty" } };
                    return RedirectToAction("CustomPageList");
                }
                CustomPageTranslation model = new CustomPageTranslation()
                {
                    CustomPageContent = CustomPage.Data,
                    TranslationHtml = CustomPage.Data.HtmlContent,


                };
                model.CustomPageTranslationText = model.TranslationTextDecoded;
                if (LanguageID > 0)
                {
                    model = CustomPage.Data.CustomPageTranslations.Where(op => op.LanguageID == LanguageID).FirstOrDefault();
                    if (model != null)
                    {
                        model.CustomPageContent = CustomPage.Data;
                        model.CustomPageTranslationText = model.TranslationTextDecoded;
                    }
                    else
                    {
                        return RedirectToAction("EditCustomPage", "Blog", new { @BlogID = CustomPageID });
                    }
                }
                return View(model);
            }
            else
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Custom Page Cannot Be Empty" } };
                return RedirectToAction("CustomPageList");
            }
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditCustomPageTranslation(CustomPageTranslation Model)
        {
            Model.TranslationHtml = Model.TranslationTextEncoded;
            var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.EditCustomPageTranslation(Model);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("EditCustomPageTranslation", "Blog", new { @CustomPageID = Model.CustomPageID, @LanguageID = Model.LanguageID });
            }
            return RedirectToAction("EditCustomPage", "Blog", new { ID = Result.Data.CustomPageID });
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditCustomPageSEOInformation(long CustomPageID = 0, long LanguageID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (CustomPageID > 0)
            {
                var CustomPage = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPage(CustomPageID);
                if (CustomPage.HasFailed)
                {
                    TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "CustomPage Cannot Be Empty" } };
                    return RedirectToAction("BlogList");
                }
                CustomPageSEOInformation model = new CustomPageSEOInformation();
                model.CustomPageContent = CustomPage.Data;
                if (LanguageID > 0)
                {
                    model = CustomPage.Data.CustomPageSEOInformations.Where(op => op.LanguageID == LanguageID).FirstOrDefault();
                    if (model == null)
                        return RedirectToAction("EditCustomPage", "Blog", new { @CustomPage = CustomPageID });

                }
                return View(model);
            }
            else
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "CustomPage Cannot Be Empty" } };
                return RedirectToAction("CustomPageList");
            }
        }

        [HttpPost]
        public ActionResult EditCustomPageSEOInformation(CustomPageSEOInformation Model)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.EditCustomPageSEOInformation(Model);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("EditCustomPageSEOInformation", "Blog", new { @BlogID = Model.CustomPageID, @LanguageID = Model.LanguageID });
            }
            return RedirectToAction("EditCustomPage", "Blog", new { ID = Result.Data.CustomPageID });
        }

        #endregion
    }
}