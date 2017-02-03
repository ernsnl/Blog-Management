using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Visa;
using BlogApplication.Framework.Extensions.StreamExtensions;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    public class VisaController : BaseController
    {
        // GET: Visa

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin})]
        public ActionResult EditUser(long ID = 0)
        {
            User model = new User();
            if (ID > 0)
            {
                var Result = this.Client.Services.ServiceController.Visa.GetUser(ID);
                if (Result.HasFailed)
                {
                    TempData["Messages"] = Result.Messages;
                    return RedirectToAction("UserList");
                }
                if (this.Client.Services.CurrentUser.UserType == VariableValue.ConvertUserTypesByte(UserType.Admin))
                {
                    if (Result.Data.UserType == VariableValue.ConvertUserTypesByte(UserType.SystemAdmin))
                    {
                        TempData["Messages"] = new List<ResultMessage> { new ResultMessage()
                        {
                            Code = "U2",
                            Description = "You are not authorized"
                        } };
                        return RedirectToAction("UserList");
                    }
                }

                model = Result.Data;
            }
            model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            return View(model);
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult EditUser(User Model)
        {
            if (Request["StatusID"] == "on")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                Model.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            var Result = this.Client.Services.ServiceController.Visa.EditUser(Model);

            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }
            return RedirectToAction("UserList");
        }

        [AllowedUserTypeFilter(UserTypes = new[] { UserType.Admin, UserType.SystemAdmin })]
        public ActionResult UserList(int page = 1)
        {
            var Result = this.Client.Services.ServiceController.Visa.GetUsers(page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            ViewBag.CurrentUserType = this.Client.Services.CurrentUser.UserType;
            return View(Result);
        }

        public ActionResult ViewProfile()
        {
            var Result = this.Client.Services.CurrentUser;
            return View(Result);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var Result = this.Client.Services.CurrentUser;
            return View(Result);
        }

        [HttpPost]
        public ActionResult EditProfile(User Model)
        {
            if (Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                Model.FileName = "profilePhoto_" + Path.GetExtension(Request.Files[0].FileName).ToLower();
                Model.UploadedLogoData = Request.Files[0].InputStream.ReadFully(0);
            }

            var Result = this.Client.Services.ServiceController.Visa.EditProfile(this.Client.Services.CurrentUser, Model);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }
            this.Client.Services.CurrentUser = Result.Data;
            return RedirectToAction("ViewProfile");

        }
    }
}