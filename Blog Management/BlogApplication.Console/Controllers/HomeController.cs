using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(this.Client.Services.CurrentUser);
        }

        public PartialViewResult BlogReview()
        {
            var mainPageBlog = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, false, true, false,1, Int32.MaxValue);
            return PartialView(mainPageBlog);
        }

        public PartialViewResult BlogNeedFixing()
        {
            var mainPageBlog = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, false, false, true, 1, Int32.MaxValue);
            return PartialView(mainPageBlog);
        }
    }
}