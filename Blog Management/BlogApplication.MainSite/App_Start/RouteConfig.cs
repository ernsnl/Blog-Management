using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BlogApplication.Framework.Configuration;
using BlogApplication.WebFramework.HtmlExtensions;
using Microsoft.Ajax.Utilities;

namespace BlogApplication.MainSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Ajax",
            url: "Ajax/{action}",
            defaults: new { action = "Test" }
                );

            routes.MapRoute(
                name: "CustomPageRoute",
                url: "{lg}/Page/{name}",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "CustomPage", action = "CustomPageHandler", name = UrlParameter.Optional },
                constraints: new { name = @"[a-zA-Z]+", lg = @"^[a-z]{2}-[A-Z]{2}$" }
            );

            routes.MapRoute(
                name: "Category",
                url: "{lg}/Category/{id}-{name}",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "Blog", action = "ByCategory", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Tag",
                url: "{lg}/Tag/{id}-{name}",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "Blog", action = "ByTag", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ViewNews",
                url: "{lg}/Blog/{id}-{name}",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "Blog", action = "ViewBlog", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "News",
                url: "{lg}/News",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "Blog", action = "BlogList", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultLanguage",
                url: "{lg}/{controller}/{action}/{id}",
                defaults: new { lg = UrlExtension.GetLanguageShort(), controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Redirect", id = UrlParameter.Optional }
            );
        }
    }
}

