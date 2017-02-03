using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Data.Blog;
using BlogApplication.Framework.Extensions.StringExtensions;
using BlogApplication.MainSite.Controllers.Base;
using BlogApplication.WebFramework.ActionFilter;

namespace BlogApplication.MainSite.Controllers
{
    [LanguageFilter]
    public class BlogController : BaseController
    {
        // GET: Blog
        public ActionResult BlogList(int page = 1)
        {
            var List = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, true, false, false, page, 10);
            return View(List);
        }

        public ActionResult ByCategory(long id, string name , int page = 1)
        {
            var Model = this.Client.Services.ServiceController.BlogContent.Category.GetCategory(id).Data;
            if (!Model.Name.RemoveSpace().ReplaceTurkishCharactes().Equals(name))
            {
                if (!Model.CategoryTranslations.Where(op => op.Translation.RemoveSpace().ReplaceTurkishCharactes().Equals(name)).Any())
                    throw new HttpException(404, "Are you sure you're in the right place?");
            }

            var Model2 = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(id, null, true, false, false, page, 10);
            return View("BlogList", Model2);
        }

        public ActionResult ByTag(long id, string name, int page = 1)
        {
            var Model = this.Client.Services.ServiceController.BlogContent.Tag.GetTag(id).Data;
            if (!Model.Name.RemoveSpace().ReplaceTurkishCharactes().Equals(name))
            {
                    throw new HttpException(404, "Are you sure you're in the right place?");
            }
            List<long> array = new List<long>();
            array.Add(id);

            var Model2 = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, array.ToArray(), true, false, false, page, 10);
            return View("BlogList",Model2);
        }


        public ActionResult ViewBlog(int id = 1, string name = "")
        {
            var Model = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContent(id).Data;
            if (!Model.Name.RemoveSpace().ReplaceTurkishCharactes().Equals(name))
            {
                if(!Model.BlogTranslations.Where(op => op.TranslationTitle.RemoveSpace().ReplaceTurkishCharactes().Equals(name)).Any())
                    throw new HttpException(404, "Are you sure you're in the right place?");
            }
            return View(Model);
        }
    }
}