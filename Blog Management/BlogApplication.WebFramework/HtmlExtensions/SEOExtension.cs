using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;

namespace BlogApplication.WebFramework.HtmlExtensions
{
    public static class SEOExtension
    {
        public static MvcHtmlString GetSEOTitle<TModel>(this HtmlHelper<TModel> htmlHelper, BlogContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.BlogSEOInformations.Count > 0)
            {
                if (Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Title);
            }
            else if (Content.BlogTranslations.Count > 0)
            {
                if (Content.BlogTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.BlogTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().TranslationTitle);
            }
            return new MvcHtmlString(Content.Name);
        }

        public static MvcHtmlString GetSEODecsription<TModel>(this HtmlHelper<TModel> htmlHelper, BlogContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.BlogSEOInformations.Count > 0)
            {
                if (Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Description);
            }
            else if (Content.BlogTranslations.Count > 0)
            {
                if (Content.BlogTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.BlogTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().AbstractTranslation);
            }
            return new MvcHtmlString(Content.BlogAbstract);
        }

        public static MvcHtmlString GetSEOKeywords<TModel>(this HtmlHelper<TModel> htmlHelper, BlogContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.BlogSEOInformations.Count > 0)
            {
                if (Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.BlogSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Keyword);
                else
                    return new MvcHtmlString(Content.BlogSEOInformations.FirstOrDefault().Keyword);

            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString GetSEOTitle<TModel>(this HtmlHelper<TModel> htmlHelper, CustomPageContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.CustomPageSEOInformations.Count > 0)
            {
                if (Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Title);
            }
            else if (Content.CustomPageTranslations.Count > 0)
            {
                if (Content.CustomPageTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.CustomPageTranslations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().TranslationTitle);
            }
            return new MvcHtmlString(Content.Title);
        }

        public static MvcHtmlString GetSEODecsription<TModel>(this HtmlHelper<TModel> htmlHelper, CustomPageContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.CustomPageSEOInformations.Count > 0)
            {
                if (Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Description);
                else
                    return new MvcHtmlString(Content.CustomPageSEOInformations.FirstOrDefault().Description);
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString GetSEOKeywords<TModel>(this HtmlHelper<TModel> htmlHelper, CustomPageContent Content)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;

            if (Content.CustomPageSEOInformations.Count > 0)
            {
                if (Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).Any())
                    return new MvcHtmlString(Content.CustomPageSEOInformations.Where(op => op.LanguageID == Controller.Client.CurrentLanguageID).FirstOrDefault().Keyword);
                else
                    return new MvcHtmlString(Content.CustomPageSEOInformations.FirstOrDefault().Keyword);
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString GetSEOTitle<TModel>(this HtmlHelper<TModel> htmlHelper,  DomainLocationSEO LocationSEO)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            if (
                Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).Any())
            {
                return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Title);
            }

            return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                    op => op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Title);
        }

        public static MvcHtmlString GetSEODecsription<TModel>(this HtmlHelper<TModel> htmlHelper, DomainLocationSEO LocationSEO)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            if (
                Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).Any())
            {
                return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Description);
            }

            return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                   op => op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Description);
        }

        public static MvcHtmlString GetSEOKeywords<TModel>(this HtmlHelper<TModel> htmlHelper, DomainLocationSEO LocationSEO)
        {
            Controllers.BaseController Controller = (Controllers.BaseController)htmlHelper.ViewContext.Controller;
            if (
                Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).Any())
            {
                return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                    op =>
                        op.LanguageID == Controller.Client.CurrentLanguageID &&
                        op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Keyword);
            }

              return new MvcHtmlString(Controller.Client.CurrentDomain.SEOInformations.Where(
                    op => op.LocationID == Convert.ToByte(LocationSEO)).FirstOrDefault().Keyword);
        }
    }
}