using System;
using System.Linq;
using System.Web.Mvc;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.WebFramework.Controllers;

namespace BlogApplication.WebFramework.ActionFilter
{
    public class AllowedUserTypeFilter : ActionFilterAttribute, IActionFilter
    {

        public UserType[] UserTypes { get; set; }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Controller = (BaseController)filterContext.Controller;
            if (Controller.Client.Services.CurrentUser != null)
            {
                if (!UserTypes.Contains((UserType) Controller.Client.Services.CurrentUser.UserType))
                {
                    var Url = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result =
                        new RedirectResult(Url.Action("Index", "Home"));
                }
            }
        }
    }
}