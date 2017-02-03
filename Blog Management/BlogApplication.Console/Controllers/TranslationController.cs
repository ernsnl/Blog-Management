using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Translation;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin, UserType.Translator })]
    public class TranslationController : BaseController
    {
        public ActionResult TranslationList(long languageID = 0, int page = 1)
        {
            var List = this.Client.Services.ServiceController.Translation.GetTranlations(0, page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(List);
        }

        [HttpGet]
        public ActionResult EditTranslation(string keyword, long languageID)
        {
            Common model = new Common();
            var Result = this.Client.Services.ServiceController.Translation.GetTranslation(keyword, languageID);
            if (Result.HasFailed)
            {
                TempData["Messages"] = Result.Messages;
                return RedirectToAction("TranslationList");
            }
            model = Result.Data;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTranslation(Common model)
        {
            var Result = this.Client.Services.ServiceController.Translation.EditTranslation(model);

            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(model);
            }
            return RedirectToAction("TranslationList");
        }

    }
}