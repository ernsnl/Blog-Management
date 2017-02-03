using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.MainSite.Controllers.Base;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.MainSite.Controllers
{
    [LanguageFilter]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Slider(int sliderSize = 3)
        {
            var mainPageBlog = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, true, false, false,1, 3);
            return PartialView(mainPageBlog);
        }

        public PartialViewResult Advertise()
        {
            return PartialView();
        }

        public PartialViewResult Category()
        {
            var categoryResult = this.Client.Services.ServiceController.BlogContent.Category.GetCategoryList(1, Int32.MaxValue);
            return PartialView(categoryResult);
        }

        public PartialViewResult Tag()
        {
            var tagResult = this.Client.Services.ServiceController.BlogContent.Tag.GetTags("", 1, Int32.MaxValue);
            return PartialView(tagResult);
        }

        public ActionResult Redirect()
        {
            var link = RouteData.Values["controller"].ToString() + "/" + RouteData.Values["action"].ToString() + "/" + (RouteData.Values["id"] != null && !string.IsNullOrEmpty(RouteData.Values["id"].ToString() ) ? RouteData.Values["id"].ToString() : "");
            if (RouteData.Values["controller"].ToString() != "ErrorPage")
                link = this.Client.CurrentLanguageShort + "/" + RouteData.Values["controller"].ToString() + "/" + RouteData.Values["action"].ToString() + "/" + (RouteData.Values["id"] != null && !string.IsNullOrEmpty(RouteData.Values["id"].ToString()) ? RouteData.Values["id"].ToString() : "");
            return RedirectPermanent(link);
        }

    }
}