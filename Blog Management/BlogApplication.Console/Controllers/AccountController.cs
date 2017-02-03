using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Visa;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.MailHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.WebFramework.ActionFilter;
using BlogApplication.WebFramework.Models;

namespace BlogApplication.Console.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        [AuthorizedFilter]
        public ActionResult ChangePassword()
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel Model)
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];

            if (CryptoFactory.GetSHA512Hash(Model.OldPassword, this.Client.Services.CurrentUser.PaswordSalt)
                !=
                this.Client.Services.CurrentUser.Password)
            {
                ViewBag.Messages = new List<ResultMessage>()
                { new ResultMessage() {Code = "U2" , Description = "Old Password Does Not Match"} };
                return View(Model);
            }
            if (Model.ConfirmPassword == Model.NewPassword)
            {
                User user = this.Client.Services.CurrentUser;
                user.Password = Model.ConfirmPassword;
                var Result = this.Client.Services.ServiceController.Visa.EditUser(user);
                if (Result.HasFailed)
                {
                    ViewBag.Messages = Result.Messages;
                    return View(Model);
                }
            }

            TempData["Messages"] = new List<ResultMessage>()
                { new ResultMessage() {Code = "S1" , Description = "Password Updated Correctly."} };

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.Client.Logout();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        [NotAuthorizedFilter]
        public ActionResult Login()
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [NotAuthorizedFilter]
        public ActionResult Login(User model, string returnUrl)
        {
            ObjectResult<User> Result = this.Client.Login(model.Username, model.Password);
            if (!Result.HasFailed)
            {
                this.Client.Services.CurrentUser = Result.Data;

                if(!string.IsNullOrEmpty(returnUrl))
                    return new RedirectResult(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Messages = Result.Messages;
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [NotAuthorizedFilter]
        public ActionResult ResetPassword(string Code, string Email)
        {
            Email = CryptoFactory.Decrypt(Email.Replace('#', '+'), ConfigurationParameter.EncryptKey);
            ObjectResult<Code> Result = this.Client.Services.ServiceController.General.Code.CheckCode(Code,Email);
            if (Result.HasFailed || Result.Data == null)
            {
                TempData["Messages"] = new List<ResultMessage>()
                { new ResultMessage() {Code = "U2" , Description = "Code Does Not Exist"} };
                return RedirectToAction("Login");
            }
            return View(Result.Data);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [NotAuthorizedFilter]
        public ActionResult ResetPassword(Code model)
        {
            var Result = this.Client.Services.ServiceController.Visa.GetUser(Convert.ToInt64(model.UserID));
            var Password = this.Request["Password"].ToString();
            User updateUser = Result.Data;
            updateUser.Password = Password;
            var updateUserResult = this.Client.Services.ServiceController.Visa.EditUser(updateUser);

            //Code will be updated
            if (updateUserResult.HasFailed)
            {
                ViewBag.Messages = updateUserResult.Messages;
                return View(model);
            }

            var deactiveCode = this.Client.Services.ServiceController.General.Code.DeactiveCode(model.Code1);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [NotAuthorizedFilter]
        public ActionResult ForgotPassword()
        {
            if (TempData["Messages"] != null)
                ViewBag.Messages = (List<ResultMessage>)TempData["Messages"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [NotAuthorizedFilter]
        public ActionResult ForgotPassword(ForgotPasswordModel Model)
        {
            var Result = this.Client.Services.ServiceController.Visa.GetUser(Model.Email);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }

            var codeResult = this.Client.Services.ServiceController.General.Code.CreateCode(Result.Data.ID,
                CodeType.Reset);
            if (Result.HasFailed)
            {
                ViewBag.Messages = Result.Messages;
                return View(Model);
            }

            var domainResult = this.Client.Services.ServiceController.MainSystem.GetDomain(Convert.ToInt64(Result.Data.DomainID)).Data;

            var HtmlBody = "<p>Please click this <a href='{url}'>URL</a> to set a new password</p>";
            HtmlBody = HtmlBody.Replace("{url}",
                domainResult.ConsoleDomainUrl + "/Account/ResetPassword" + "?Code="
                + codeResult.Data.Code1 + "&Email="
                + CryptoFactory.Encrypt(Result.Data.Email, ConfigurationParameter.EncryptKey).Replace('+', '#'));
            var mailResult = MailHelper.SendEmail(domainResult.Username, Result.Data.Email,
                CryptoFactory.Decrypt(domainResult.SmtpPassword), "Reset Password Email", HtmlBody, domainResult.SmtpPort,
                domainResult.SmtpHost);
            if (mailResult.HasFailed)
            {
                TempData["Messages"] = mailResult.Messages;
                return View(Model);
            }
            TempData["Messages"] = new List<ResultMessage>()
                {
                    new ResultMessage() {Code = "U1", Description = "Reset Mail is Sent. Please check your inbox"}
                };

            return RedirectToAction("Login");
        }
    }
}