using System.Web.Mvc;
using BlogApplication.MainSite.Controllers.Base;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.MainSite.Controllers
{
    public class AjaxController : BaseController
    {
        [CorsPolicy]
        public JsonResult Test()
        {
            var Result = this.Client.Services.ServiceController.ClearCache();

            object result = new object();
            if (Result.HasFailed)
            {
                result = new { message = "Failed" };
            }
            else
            {
                result = new { message = "OK" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}