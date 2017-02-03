using System.Web;
using System.Web.Mvc;

namespace BlogApplication.Data.Translation
{
    public partial class BlogTranslation
    {
        [AllowHtml]
        public string TranslationText { get; set; }

        public string TranslationTextEncoded
        {
            get { return HttpUtility.HtmlEncode(TranslationText); }
        }

        public string TranslationTextDecoded
        {
            get { return HttpUtility.HtmlDecode(TranslationHtml); }
        }
    }
}