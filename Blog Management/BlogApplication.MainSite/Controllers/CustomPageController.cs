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
    public class CustomPageController : BaseController
    {
        // GET: CustomPage
        public ActionResult CustomPageHandler(string name)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPage(name);
            if (Result.HasFailed || Result.Data == null)
            {
                return RedirectToAction("Oops","ErrorPage", new { Code = "404"});
            }
            return View(Result.Data);
        }
    }
}