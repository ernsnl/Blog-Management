using System;
using System.Web.Mvc;
using BlogApplication.WebFramework.Controllers;

namespace BlogApplication.WebFramework.ActionFilter
{
    public class LanguageFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = (BaseController)filterContext.Controller;
            var requestUrl = Controller.Client.Request.Url.LocalPath;
            if (!string.IsNullOrEmpty(requestUrl) && !string.IsNullOrEmpty(requestUrl.Replace("/", "")))
            {
                var requestedLanguage =
                    Controller.Client.Services.ServiceController.General.Language.GetLanguage(requestUrl.Split('/')[1]);
                if (requestedLanguage.Data != null)
                    Controller.Client.CurrentLanguageID = Convert.ToInt32(requestedLanguage.Data.ID);
                else
                {
                    var Url = new UrlHelper(filterContext.RequestContext);
                    
                    filterContext.Result = new RedirectResult(filterContext.HttpContext.Request
                        .RawUrl.Replace(requestUrl.Split('/')[1], Controller.Client.CurrentLanguageShort + "/" + requestUrl.Split('/')[1]));
                }

            }

        }
    }
}
