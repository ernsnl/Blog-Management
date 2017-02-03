using System.Web;
using System.Web.SessionState;

namespace BlogApplication.Framework.Extensions.WebExtensions
{
    public static class HttpSessionExtension
    {
        public static string GetSessionID(this HttpSessionState session)
        {
            string sessionId = string.Empty;
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                sessionId = Utility.RandomHelper.GenerateRandomPassword(10);
            else
                sessionId = HttpContext.Current.Session.SessionID;
            return sessionId;
        }
    }

}