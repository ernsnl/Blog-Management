using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BlogApplication.Framework.Configuration;
using BlogApplication.WebFramework;

namespace BlogApplication.Console.Controllers.Base
{
    public  class BaseController : WebFramework.Controllers.BaseController
    {
        private Client oClient;

        public new Client Client
        {
            get
            {
                if(this.oClient == null)
                    this.oClient = new Client();
                return this.oClient;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        public BaseController()
        {

        }
    }
}