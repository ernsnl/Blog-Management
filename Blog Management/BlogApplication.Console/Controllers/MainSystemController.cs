using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.MainSystem;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.Extensions.StreamExtensions;
using BlogApplication.Framework.MailHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    public class SystemMainController : BaseController
    {
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin })]
        public ActionResult DomainList(int page = 1)
        {
            var Model = this.Client.Services.ServiceController.MainSystem.GetDomains(page);
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View(Model);
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin })]
        public ActionResult EditDomain(long ID = 0)
        {
            Domain model = new Domain();
            model.StatusID = VariableValue.ActiveStatusID;
            if (ID > 0)
            {
                model = this.Client.Services.ServiceController.MainSystem.GetDomain(ID).Data;
            }

            return View(model);
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin })]
        public ActionResult EditDomain(Domain Domain)
        {
            bool isNew = (Domain.ID == 0);

            if (Request["StatusID"] == "on")
                Domain.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                Domain.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                Domain.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);


            if (Request.Files.Count > 0)
            {
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    Domain.DomainBackgroundImgName = "domainPhoto_" + Path.GetExtension(Request.Files[0].FileName).ToLower();
                    Domain.DomainBackgroundImgData = Request.Files[0].InputStream.ReadFully(0);
                }

                if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                {
                    Domain.DomainFavIcoName = "domainFav_" + Path.GetExtension(Request.Files[1].FileName).ToLower();
                    Domain.DomainFavIcoData = Request.Files[1].InputStream.ReadFully(0);
                }
            }

            var Result = this.Client.Services.ServiceController.MainSystem.EditDomain(Domain, true);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Domain);
            }
            if (isNew)
            {
                var userResult = this.Client.Services.ServiceController.Visa.GetUser(Result.Data.AdminEmail).Data;
                var codeResult = this.Client.Services.ServiceController.General.Code.CreateCode(userResult.ID, CodeType.Welcome);

                var HtmlBody = "<p>Please click this <a href='{url}'>URL</a> to set a new password</p>";
                HtmlBody = HtmlBody.Replace("{url}",
                    Result.Data.ConsoleDomainUrl + "/Welcome" + "?Code="
                    + codeResult.Data.Code1 + "&Email="
                    + CryptoFactory.Encrypt(userResult.Email, ConfigurationParameter.EncryptKey).Replace('+', '#'));
                var mailResult = MailHelper.SendEmail(this.Client.CurrentDomain.Username, Result.Data.AdminEmail,
                    this.Client.CurrentDomain.SmtpPassword, "Welcome Mail", HtmlBody, this.Client.CurrentDomain.SmtpPort,
                    this.Client.CurrentDomain.SmtpHost);
                if (mailResult.HasFailed)
                {
                    TempData["Messages"] = mailResult.Messages;
                    return RedirectToAction("DomainList");
                }
                TempData["Messages"] = new List<ResultMessage>()
                {
                    new ResultMessage() {Code = "U1", Description = "Welcome Mail is Sent. Please check your inbox"}
                };
                return RedirectToAction("DomainList");
            }
            return RedirectToAction("DomainList");
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult UpdateCurrentDomain()
        {
            Domain model = this.Client.CurrentDomain;
            return View(model);
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult UpdateCurrentDomain(Domain Domain)
        {
            Domain.StatusID = VariableValue.ActiveStatusID;
            if (Request.Files.Count > 0)
            {
                if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    Domain.DomainBackgroundImgName = "domainPhoto_" + Path.GetExtension(Request.Files[0].FileName).ToLower();
                    Domain.DomainBackgroundImgData = Request.Files[0].InputStream.ReadFully(0);
                }

                if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                {
                    Domain.DomainFavIcoName = "domainFav_" + Path.GetExtension(Request.Files[1].FileName).ToLower();
                    Domain.DomainFavIcoData = Request.Files[1].InputStream.ReadFully(0);
                }
            }

            var Result = this.Client.Services.ServiceController.MainSystem.EditDomain(Domain, true);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Domain);
            }
            this.Client.CurrentDomain = Result.Data;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditCurrentDomainSEOInformation(long ID = 0)
        {
            SEOInformation model = new SEOInformation();
            if (ID > 0)
            {
                model = this.Client.CurrentDomain.SEOInformations.Where(op => op.ID == ID).FirstOrDefault();
            }
            model.DomainID = this.Client.CurrentDomainID;
            return View(model);
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditCurrentDomainSEOInformation(SEOInformation SEOInformation)
        {
            var Result = this.Client.Services.ServiceController.MainSystem.EditSEOInformation(SEOInformation);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(SEOInformation);
            }
            return RedirectToAction("UpdateCurrentDomain");
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditNavigationPanel(int typeID, long DomainID = 0)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (typeID > 0 && DomainID > 0)
            {
                var Result = this.Client.Services.ServiceController.MainSystem.GetNavigationEditor(typeID, DomainID > 0 && this.Client.Services.CurrentUser.UserType == Convert.ToByte(UserType.SystemAdmin) ? DomainID : this.Client.CurrentDomainID);
                if (!Result.HasFailed)
                    return View(Result.Data);
            }
            TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Type and Domain Cannot Be Empty" } };
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditNavigationPanel(NavigationEditor NavigationEditor)
        {

            var Result = this.Client.Services.ServiceController.MainSystem.EditNavigationEditor(NavigationEditor);
            if (Result.HasFailed)
            {
                TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Type Cannot Be Empty" } };

                return RedirectToAction("EditNavigationPanel",
                    new { typeID = NavigationEditor.EditorLocation, DomainID = NavigationEditor.DomainID });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditDomainSocial(long DomainID, long ID = 0)
        {
            if (DomainID > 0)
            {
                var domainResult = this.Client.Services.ServiceController.MainSystem.GetDomain(this.Client.Services.CurrentUser.UserType == Convert.ToByte(UserType.SystemAdmin) ? DomainID : this.Client.CurrentDomainID);
                if (domainResult.HasFailed || domainResult.Data == null)
                {
                    TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Domain Does Not Exist" } };
                    return RedirectToAction("Index", "Home");
                }
                DomainSocial model = new DomainSocial();
                if (ID > 0)
                    model = domainResult.Data.DomainSocials.Where(op => op.ID == ID).FirstOrDefault();
                else
                {
                    model.StatusID = VariableValue.ActiveStatusID;
                    model.DomainID = domainResult.Data.ID;
                }

                return View(model);
            }
            TempData["Messages"] = new List<ResultMessage>() { new ResultMessage() { Code = "U2", Description = "Domain Cannot Be Empty" } };
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowedUserTypeFilter(UserTypes = new UserType[] { UserType.SystemAdmin, UserType.Admin })]
        public ActionResult EditDomainSocial(DomainSocial DomainSocial)
        {
            if (Request["StatusID"] == "on")
                DomainSocial.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Active);
            else
                DomainSocial.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Passive);

            if (Request["DeleteInfo"] == "1")
                DomainSocial.StatusID = VariableValue.ConvertStatusTypesByte(StatusType.Deleted);

            var domainResult = this.Client.Services.ServiceController.MainSystem.EditDomainSocial(DomainSocial);
            if (domainResult.HasFailed || domainResult.Data == null)
            {
                ViewBag.Messages = domainResult.Messages;
                return View(DomainSocial);
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
