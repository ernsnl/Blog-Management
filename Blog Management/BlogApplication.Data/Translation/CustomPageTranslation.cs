using System.Web;
using System.Web.Mvc;

namespace BlogApplication.Data.Translation
{
    public partial class CustomPageTranslation
    {
        [AllowHtml]
        public string CustomPageTranslationText { get; set; }

        public string TranslationTextEncoded
        {
            get { return HttpUtility.HtmlEncode(CustomPageTranslationText); }
        }

        public string TranslationTextDecoded
        {
            get { return HttpUtility.HtmlDecode(TranslationHtml); }
        }
    }
}