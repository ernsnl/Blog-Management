using System.Web.Mvc;

namespace BlogApplication.Framework.Extensions.WebExtensions
{
    public static class UrlHelperExtension
    {
        public static string QualifiedAction(this UrlHelper url, string actionName, string controllerName, object routeValues = null)
        {
            return url.Action(actionName, controllerName, routeValues, url.RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}