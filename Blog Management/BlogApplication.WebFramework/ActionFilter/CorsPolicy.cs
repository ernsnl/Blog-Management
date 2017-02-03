using System;
using System.Linq;
using System.Web.Mvc;
using BlogApplication.WebFramework.Controllers;

namespace BlogApplication.WebFramework.ActionFilter
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsPolicy : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = (BaseController)filterContext.Controller;

            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin",
                Controller.Client.CurrentDomain.ConsoleDomainUrl);
            base.OnActionExecuting(filterContext);
        }

    }
}
