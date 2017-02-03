using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin, UserType.Translator  })]
    public class GeneralController : BaseController
    {
        // GET: General
        public ActionResult LanguageList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.General.Language.GetLanguageList(page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>) TempData["Messages"];
            return View(Result);
        }

        [HttpGet]
        public ActionResult EditLanguage(long ID = 0)
        {
            Language model = new Language();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.General.Language.GetLanguage(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("LanguageList");
                }
                model = Result.Data;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditLanguage(Language nlanguage)
        {
            if (Request["StatusID"] == "on")
                nlanguage.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                nlanguage.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if(Request["DeleteInfo"] == "1")
                nlanguage.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            var Result = this.Client.Services.ServiceController.General.Language.EditLanugage(nlanguage);
            
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(nlanguage);
            }
            return RedirectToAction("LanguageList");
        }
    }
}