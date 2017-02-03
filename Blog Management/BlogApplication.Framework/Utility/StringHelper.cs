using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BlogApplication.Framework.Cryptography;

namespace BlogApplication.Framework.Utility
{
    public static class StringHelper
    {

        public static bool checkFormat(string input, Regex reg)
        {
            return reg.Match(input).Success;
        }

        public static bool validateURL(string urlName)
        {
            return Uri.IsWellFormedUriString(urlName, UriKind.RelativeOrAbsolute);
        }

        public static string CreateTodayFileFormat()
        {
            return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
        }

        public static string addRandomToEnd(string fileName)
        {
            return fileName + "_" + RandomHelper.GenerateRandomNumber(10);
        }

        public static string FormatFileName(string fileName)
        {
            fileName = fileName.ToLower().Trim().Replace(" ", "_");
            fileName = Regex.Replace(fileName, @"[^\u0000-\u007F]", string.Empty);
            return fileName;
        }

        public static string HttpDecode(string text)
        {
           return  HttpUtility.HtmlDecode(text);
        }

        public static string HttpEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

    }
}
