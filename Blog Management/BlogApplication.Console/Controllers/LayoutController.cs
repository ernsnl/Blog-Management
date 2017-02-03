using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    public class LayoutController : BaseController
    {
        // GET: Layout
        public PartialViewResult NavMenu()
        {
            return PartialView(this.Client.Services.CurrentUser);
        }

        //Header information will be done in the future
        public PartialViewResult HeaderMenu()
        {
            return PartialView(this.Client.Services.CurrentUser);
        }

        public PartialViewResult FooterMenu()
        {
            return PartialView();
        }
    }
}