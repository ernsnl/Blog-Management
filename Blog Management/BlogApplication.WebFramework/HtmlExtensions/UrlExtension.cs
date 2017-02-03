using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Extensions.StringExtensions;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.WebFramework.HtmlExtensions
{
    public static class UrlExtension
    {
        public static MvcHtmlString GetLanguageShort<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentLanguageShort);
        }

        public static string GetLanguageShort()
        {
            if (HttpContext.Current.Session == null ||  HttpContext.Current.Session["CurrentLanguageShort"] == null)
                return ConfigurationParameter.GetParameter("CurrentLanguageShort");
            else
                return HttpContext.Current.Session["CurrentLanguageShort"].ToString();
        }

        public static MvcHtmlString ChangeLanguage<TModel>(this HtmlHelper<TModel> htmlHelper, string languageISOCode)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            var currentURL = Controller.Client.Request.Url.LocalPath;
            if (!string.IsNullOrEmpty(currentURL) && !string.IsNullOrEmpty(currentURL.Replace("/", "")))
            {
                return new MvcHtmlString(currentURL.Replace(currentURL.Split('/')[1], languageISOCode));
            }
            return new MvcHtmlString(currentURL  +languageISOCode);
        }

        public static MvcHtmlString PanelEditorLink<TModel>(this HtmlHelper<TModel> htmlHelper, string name, int typeID,
            long pageID)
        {
            Controllers.BaseController Controller = (Controllers.BaseController) htmlHelper.ViewContext.Controller;
            string url = "<li><a href='";
            if (typeID == 1)
            {
                var blogContent = Controller.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(pageID).Data;
                url += Controller.HttpContext.Request.RawUrl.Split('/')[0]
                       + "/" + Controller.Client.CurrentLanguageShort
                       + "/" + "Blog"
                       + "/" + blogContent.ID
                       + "/" + (blogContent.BlogTranslations.Where(
                        op => op.LanguageID == Convert.ToInt64(Controller.Client.Session["CurrentLanguageID"])).Any()
                        ? blogContent.BlogTranslations.Where(
                            op => op.LanguageID == Convert.ToInt64(Controller.Client.Session["CurrentLanguageID"]))
                            .FirstOrDefault()
                            .TranslationTitle.ReplaceTurkishCharactes()
                            .RemoveSpace()
                        : blogContent.Name.ReplaceTurkishCharactes().RemoveSpace());
            }
            else
            {
                var customPageContent = Controller.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPage(pageID).Data;
                url += Controller.HttpContext.Request.RawUrl.Split('/')[0]
                       + "/" + Controller.Client.CurrentLanguageShort
                       + "/" + "Page"
                       + "/" + customPageContent.CustomPageUrl;
            }
            url += "'>" + htmlHelper.GetWord(name) + "</a></li>";
            return new MvcHtmlString(url);
        }

        public static MvcHtmlString SocialIcon<TModel>(this HtmlHelper<TModel> htmlHelper, byte typeID)
        {
            if (typeID == (byte) SocialTypes.Facebook)
                return new  MvcHtmlString("<i class='fa fa-facebook-official'></i>");
            if (typeID == (byte)SocialTypes.Twitter)
                return new MvcHtmlString("<i class='fa fa-twitter'></i>");
            if (typeID == (byte)SocialTypes.Youtube)
                return new MvcHtmlString("<i class='fa fa-youtube-play'></i>");
            if (typeID == (byte)SocialTypes.Instagram)
                return new MvcHtmlString("<i class='fa fa-instagram'></i>");
            if (typeID == (byte)SocialTypes.Tumblr)
                return new MvcHtmlString("<i class='fa fa-tumblr'></i>");
            if (typeID == (byte)SocialTypes.LinkedIn)
                return new MvcHtmlString("<i class='fa fa-linkein'></i>");
            if (typeID == (byte)SocialTypes.Twitter)
                return new MvcHtmlString("<i class='fa fa-pinterest'></i>");
            if (typeID == (byte)SocialTypes.Patreon)
                return new MvcHtmlString("<i class='fa fa-facebook-official'></i>");
            return new MvcHtmlString("");
        }

        public static string InsertHttpToLink<TModel>(this HtmlHelper<TModel> htmlHelper, string Link)
        {
            if (Link.IndexOf("http") > -1)
                return Link;
            else
                return "http://" + Link; 
        }
    }
}