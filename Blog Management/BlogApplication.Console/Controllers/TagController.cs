using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    [AllowedUserTypeFilter(UserTypes = new []{UserType.Admin, UserType.Editor, UserType.SystemAdmin, UserType.Writer})]
    public class TagController : BaseController
    {
        // GET: Tag
        public ActionResult TagList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.BlogContent.Tag.GetTags("",page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(Result);
        }

        [HttpGet]
        public ActionResult EditTag(long ID = 0)
        {
            Tag model = new Tag();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.BlogContent.Tag.GetTag(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("TagList");
                }
                model = Result.Data;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTag(Tag nTag)
        {
            if (Request["StatusID"] == "on")
                nTag.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                nTag.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                nTag.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            var Result = this.Client.Services.ServiceController.BlogContent.Tag.EditTag(nTag);

            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(nTag);
            }
            return RedirectToAction("TagList");
        }
    }

    
}