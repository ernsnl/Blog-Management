using System.Configuration;
using BlogApplication.Framework.Configuration;

namespace BlogApplication.Framework.FTPHelper
{
    public static class FTPProperties
    {

        public static string InitailDirectory
        {
            get { return  "images"; }
        }
    }
}