using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Framework.Application.Web;
using BlogApplication.MainSite.Controllers.Base;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.MainSite.Controllers
{
    public class ErrorPageController : BaseController
    {
        // GET: Error
        [HandleError]
        public ActionResult Oops(int Code = 0)
        {
            HttpError Model = new HttpError();
            if (Code > 0)
            {
                Model.ErrorCode = Code;
                if (Code == 404)
                    Model.ErrorMessage = "The Page You Are Looking For Went Fishing";
                if (Code == 403)
                    Model.ErrorMessage = "";
                if (Code == 500)
                    Model.ErrorMessage = "";
            }
            return View(Model);
        }
    }
}