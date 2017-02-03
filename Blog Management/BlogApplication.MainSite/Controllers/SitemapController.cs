using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.Application.Web.SiteMap;
using BlogApplication.Framework.Extensions.StringExtensions;
using BlogApplication.Framework.Extensions.WebExtensions;
using BlogApplication.MainSite.Controllers.Base;

namespace BlogApplication.MainSite.Controllers
{
    public class SitemapController : BaseController
    {
        // GET: Sitemap
        public ActionResult Index()
        {
            var customPages = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPageList(1, int.MaxValue).Data;
            var blogPages = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, true, false, false,
                1, Int32.MaxValue).Data;
            var categoryList = this.Client.Services.ServiceController.BlogContent.Category.GetCategoryList(1, Int32.MaxValue).Data;
            var tagList = this.Client.Services.ServiceController.BlogContent.Tag.GetTags("", 1, Int32.MaxValue).Data;
            var languageList = this.Client.Services.ServiceController.General.Language.GetLanguageList(1, Int32.MaxValue).Data;



            var sitemapItems = new List<SiteMapItem>();

            foreach (var lang in languageList.Where(op => op.StatusID == VariableValue.ActiveStatusID).ToList())
            {
                sitemapItems.Add(new SiteMapItem(Url.QualifiedAction("Index", "Home", new {lg = lang.CodeISO}),
                    ChangeFrequency.Always, 1.0, DateTime.Now));
                sitemapItems.Add(new SiteMapItem(Url.QualifiedAction("BlogList", "Blog", new { lg = lang.CodeISO }),
                    ChangeFrequency.Always, 1.0, DateTime.Now));

                if (customPages != null && customPages.Count > 0)
                {
                    foreach (var customPage in customPages)
                    {
                        sitemapItems.Add(
                            new SiteMapItem(
                                Url.QualifiedAction("CustomPageHandler", "CustomPage",
                                    new {name = customPage.CustomPageUrl.ToString().ReplaceTurkishCharactes().RemoveSpace() , lg = lang.CodeISO}), ChangeFrequency.Weekly, 1.0, DateTime.Now));
                    }
                }

                if (blogPages != null && blogPages.Count > 0)
                {
                    foreach (var blogPage in blogPages)
                    {
                        sitemapItems.Add(
                            new SiteMapItem(
                                Url.QualifiedAction("ViewBlog", "Blog", new {ID = blogPage.ID, name = blogPage.Name.ReplaceTurkishCharactes().RemoveSpace(), lg = lang.CodeISO }),
                                ChangeFrequency.Daily, 1.0, DateTime.Now));
                    }
                }

                if (categoryList != null && categoryList.Count > 0)
                {
                    foreach (var category in categoryList)
                    {
                        sitemapItems.Add(
                            new SiteMapItem(
                                Url.QualifiedAction("ByCategory", "Blog", new { ID = category.ID, name = category.Name.ReplaceTurkishCharactes().RemoveSpace(), lg = lang.CodeISO }),
                                ChangeFrequency.Hourly, 1.0, DateTime.Now));
                    }
                }

                if (tagList != null && tagList.Count > 0)
                {
                    foreach (var tag in tagList)
                    {
                        sitemapItems.Add(
                            new SiteMapItem(
                                Url.QualifiedAction("ByTag", "Blog", new { ID = tag.ID, name = tag.Name.ReplaceTurkishCharactes().RemoveSpace(), lg = lang.CodeISO }),
                                ChangeFrequency.Hourly, 1.0, DateTime.Now));
                    }
                }
            }
            return new SiteMapResult(sitemapItems);
        }

    }
}