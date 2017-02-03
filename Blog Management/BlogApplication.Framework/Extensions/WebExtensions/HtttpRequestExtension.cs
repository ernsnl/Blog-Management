using System.Web;

namespace BlogApplication.Framework.Extensions.WebExtensions
{
    public static class HtttpRequestExtension
    {
        public static string GetUserHostAddress(this HttpRequest request)
        {
            string userHostAddress = string.Empty;
            if (HttpContext.Current != null)
            {
                userHostAddress = !string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? request.ServerVariables["HTTP_X_FORWARDED_FOR"] : request.ServerVariables["REMOTE_ADDR"];
                string[] ArrayAddress = userHostAddress.Split(',');
                if (ArrayAddress.Length > 1)
                    userHostAddress = ArrayAddress[0];
            }
            return userHostAddress;
        }
    }
}