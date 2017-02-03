using System.Drawing;
using System.Web;
using System.Web.Mvc;

namespace BlogApplication.Data.Blog
{
    public partial class BlogContent
    {
        [AllowHtml]
        public string BlogText { get; set; }

        public string BlogEncoded {
            get { return HttpUtility.HtmlEncode(BlogText); }
        }

        public string BlogDecoded
        {
            get { return HttpUtility.HtmlDecode(HtmlContent); }
        }

        public string BlogCoverImgName { get; set; }

        public byte[] CoverImgData { get; set; }

        public Bitmap Data { get; set; }

        public string TagIDs { get; set; }
    }
}