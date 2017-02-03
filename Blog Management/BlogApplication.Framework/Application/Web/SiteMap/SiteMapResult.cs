using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace BlogApplication.Framework.Application.Web.SiteMap
{

    public class SiteMapResult : ActionResult
    {
        private readonly IEnumerable<ISiteMapItem> items;
        private readonly ISiteMapGenerator generator;

        public SiteMapResult(IEnumerable<ISiteMapItem> items) : this(items, new SiteMapGenerator())
            {
        }

        public SiteMapResult(IEnumerable<ISiteMapItem> items, ISiteMapGenerator generator)
        {
            this.items = items;
            this.generator = generator;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = generator.GenerateSiteMap(items);

                sitemap.WriteTo(writer);
            }
        }
    }

}