using System;
using System.Collections.Generic;

namespace BlogApplication.Framework.FileHelper
{
    public static class ImageOptions
    {
        public class widthHeight
        {
            public int width { get; set; }
            public int height { get; set; }
        }

        public static Dictionary<string, widthHeight> BlogOpt
        {
            get
            {
                Dictionary<string, widthHeight> blogOpt = new Dictionary<string, widthHeight>();
                blogOpt.Add("_s", new widthHeight() {width = 250, height = 250});
                blogOpt.Add("_m", new widthHeight() { width = 500, height = 500 });
                blogOpt.Add("_o", new widthHeight() { width = Int32.MaxValue, height = Int32.MaxValue });
                return blogOpt;
            }
        }

        public static Dictionary<string, widthHeight> favIcon
        {
            get
            {
                Dictionary<string, widthHeight> favIcon = new Dictionary<string, widthHeight>();
                favIcon.Add("", new widthHeight() { width = 10, height = 10 });
                return favIcon;
            }
        }

        public static Dictionary<string, widthHeight> CategoryOpt
        {
            get
            {
                Dictionary<string, widthHeight> CategoryOpt = new Dictionary<string, widthHeight>();
                CategoryOpt.Add("_s", new widthHeight() { width = 250, height = 250 });
                return CategoryOpt;
            }
        }

        public static Dictionary<string, widthHeight> BackGroundOpt
        {
            get
            {
                Dictionary<string, widthHeight> BackGroundOpt = new Dictionary<string, widthHeight>();
                BackGroundOpt.Add("_s", new widthHeight() { width = 250, height = 250 });
                BackGroundOpt.Add("_o", new widthHeight() { width = Int32.MaxValue, height = Int32.MaxValue });
                return BackGroundOpt;
            }
        }
    }
}