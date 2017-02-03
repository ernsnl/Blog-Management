using System.Web.Mvc;
using BlogApplication.Framework.Cryptography;

namespace BlogApplication.WebFramework.HtmlExtensions
{
    public static class DomainExtensions
    {
        public static MvcHtmlString GetFavIcon<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController) htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.FavIcon);
        }

        public static MvcHtmlString GetCDNUrl<TModel>(this HtmlHelper<TModel> htmlHelper, string directoryName)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.CDNUrl + "/images/" + directoryName + "/");
        }

        public static MvcHtmlString GetImgName<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.DomainImgName);
        }

        public static MvcHtmlString GetDisqus<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.DisqusUserName);
        }

        public static MvcHtmlString GetDomainName<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.DomainName);
        }

        public static MvcHtmlString GetFacebookID<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.FacebookAppID);
        }

        public static MvcHtmlString GetTwitterPage<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.TwitterUserName);
        }

        public static MvcHtmlString GetGoogleAnalyticsID<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.GoogleAnalyticsID);
        }

        public static MvcHtmlString GetDomainURL<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(Controller.Client.CurrentDomain.DomainUrl + "/" + Controller.Client.CurrentLanguageShort);
        }

        public static MvcHtmlString GetEncryptedPassword<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            return new MvcHtmlString(CryptoFactory.Encrypt("690216-1200937368461"));
        }
    }
}