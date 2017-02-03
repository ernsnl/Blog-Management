using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.MainSite.Controllers.Base;
using Newtonsoft.Json.Linq;

namespace BlogApplication.MainSite.Controllers
{
    public class LayoutController :BaseController
    {
        // GET: Layout
        public PartialViewResult NavMenu()
        {
            var LanguageList = this.Client.Services.ServiceController.General.Language.GetLanguageList(1, 5).Data;
            return PartialView(LanguageList);
        }

        public PartialViewResult Editor()
        {
            JObject returnString = new JObject();
            var baseModel =
                this.Client.CurrentDomain.NavigationEditors.Where(
                    op => op.EditorLocation == Convert.ToByte(EditorPanelType.NavigationPanel)).FirstOrDefault();
            if (baseModel != null)
            {
                returnString = JObject.Parse(baseModel.EditorData);
                

            }
            return PartialView(returnString);
        }

        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public PartialViewResult SiteMap()
        {
            JObject returnString = new JObject();
            var baseModel =
                this.Client.CurrentDomain.NavigationEditors.Where(
                    op => op.EditorLocation == Convert.ToByte(EditorPanelType.SiteMapPanel)).FirstOrDefault();
            if (baseModel != null)
            {
                returnString = JObject.Parse(baseModel.EditorData);


            }
            return PartialView(returnString);
        }

        public PartialViewResult Social()
        {
            return PartialView(this.Client.CurrentDomain.DomainSocials.ToList());
        }
    }
}