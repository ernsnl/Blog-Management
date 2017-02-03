using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BlogApplication.Framework.Application.Web.SiteMap
{
    public interface ISiteMapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISiteMapItem> items);
    }
}