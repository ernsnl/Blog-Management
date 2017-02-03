using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogApplication.Console.Controllers.Base;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Extensions.StreamExtensions;
using BlogApplication.Framework.Extensions.StringExtensions;
using BlogApplication.Framework.FileHelper;
using BlogApplication.WebFramework.ActionFilter;
using Newtonsoft.Json;
using TweetSharp;

namespace BlogApplication.Console.Controllers
{
    [AuthorizedFilter]
    public class AjaxController : BaseController
    {
        public JsonResult UploadImage()
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                byte[] FileData = Request.Files[0].InputStream.ReadFully(0);
                var MemoryStream = new MemoryStream();
                ImageCrop.CropImage(FileData, 250, 250).Save(MemoryStream, ImageFormat.Png);
                var Result = FileUploader.FTPUploadImageFile(this.Client.CurrentDomain.CDNUrl, "Blog",
                                        this.Client.CurrentDomain.FTPUsername, this.Client.CurrentDomain.FTPPassword, MemoryStream.ToArray(), "blogPhoto_" + Session.SessionID + Path.GetExtension(Request.Files[0].FileName).ToLower(), ImageOptions.BlogOpt);
                if (!Result.HasFailed)
                    return Json(new { link = Client.CurrentDomain.CDNUrl + "/images/" + "Blog/" + (Result.Data.Split('.')[0] + "__s" + "." + Result.Data.Split('.')[1]) }, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public JsonResult GetTags(string q = "")
        {
            if (!string.IsNullOrEmpty(q))
            {
                var List = this.Client.Services.ServiceController.BlogContent.Tag.GetTags(q, 1, 15).Data;
                var Items = new List<object>();
                foreach (var item in List)
                {
                    Items.Add(new { id = item.ID, name = item.Name });
                }
                return Json(Items, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMapInfo(string text = "")
        {
            text = text.Trim().Replace(' ', '+').ReplaceTurkishCharactes();
            WebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/place/textsearch/json" +
                              "?query=" + text + "&key=AIzaSyBcRKynxIw7xb8XOtgtDu0ztVQ5zq1nQKM");

            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            //get response-stream, and use a streamReader to read the content
            using (Stream s = request.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    var jsonData = sr.ReadToEnd();
                    //decode jsonData with javascript serializer
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetBlogList()
        {
            var List = this.Client.Services.ServiceController.BlogContent.Blog.GetBlogContentList(0, null, true, false, false, 1, Int32.MaxValue).Data;
            var Items = new List<object>();
            foreach (var item in List)
            {
                Items.Add(new { id = item.ID, name = item.Name });
            }
            return Json(Items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomPageList()
        {
            var List = this.Client.Services.ServiceController.BlogContent.CustomPage.GetCustomPageList(1, Int32.MaxValue).Data;
            var Items = new List<object>();
            foreach (var item in List)
            {
                Items.Add(new { id = item.ID, name = item.CustomPageUrl });
            }
            return Json(Items, JsonRequestBehavior.AllowGet);
        }

        [AllowedUserTypeFilter(UserTypes = new UserType[] {UserType.SystemAdmin, UserType.Admin, UserType.Editor})]
        public JsonResult DropAllCache()
        {
            var Result = this.Client.Services.ServiceController.ClearCache();

            object result = new object();
            if (Result.HasFailed)
            {
                 result = new { message = "Failed"};
            }
            else
            {
                result = new { message = "OK" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}