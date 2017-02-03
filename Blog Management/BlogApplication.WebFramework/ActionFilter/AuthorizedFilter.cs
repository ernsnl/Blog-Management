using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.WebFramework.Controllers;

namespace BlogApplication.WebFramework.ActionFilter
{
    public class AuthorizedFilter : ActionFilterAttribute, IActionFilter
    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = (BaseController) filterContext.Controller;
            if (Controller.Client.Services.CurrentUser == null || Controller.Client.Services.CurrentUser.ID == 0)
            {
                //Controller.Client.Login("ernsnl", "3736E25081993e");
                var Url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(Url.Action("Login", "Account", new { returnUrl = filterContext.HttpContext.Request.RawUrl }));
            }
        }
    }
}
