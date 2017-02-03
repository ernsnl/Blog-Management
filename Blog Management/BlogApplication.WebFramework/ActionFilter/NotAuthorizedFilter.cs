using System.Web.Mvc;
using BlogApplication.WebFramework.Controllers;

namespace BlogApplication.WebFramework.ActionFilter
{
    public class NotAuthorizedFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = (BaseController)filterContext.Controller;
            if (Controller.Client.Services.CurrentUser != null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(Url.Action("Index", "Home"));
            }
        }
    }
}