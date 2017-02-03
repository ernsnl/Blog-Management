using System.Web;
using System.Web.Mvc;

namespace BlogApplication.Data.Blog
{
    public partial class CustomPageContent
    {
        [AllowHtml]
        public string CustomPageText { get; set; }

        public string CustomPageEncoded
        {
            get { return HttpUtility.HtmlEncode(CustomPageText); }
        }

        public string CustomPageDecoded
        {
            get { return HttpUtility.HtmlDecode(HtmlContent); }
        }
    }
}