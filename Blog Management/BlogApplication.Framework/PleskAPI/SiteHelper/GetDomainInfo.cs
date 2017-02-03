using System.Security.Cryptography.X509Certificates;
using BlogApplication.Framework.PleskAPI.NodeModels;
using BlogApplication.Framework.PleskAPI.RequestHandler;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.Framework.PleskAPI.SiteHelper
{
    public static class GetDomainInfo
    {
        private static string createPackage(DomainInfoModel infoModel)
        {
            if (string.IsNullOrEmpty(infoModel.siteName))
                return "";
            string packet = "<packet>" +
                            "<site>" +
                            "<get>" +
                            "<filter>" + 
                            "<name>" + infoModel.siteName + "</name>" +
                            "</filter>" +
                            "<dataset>" +
                            (infoModel.isHostingInfo ? "<hosting/>" : "") +
                            (infoModel.isDiskUsageInfo ? "<disk_usage/>" : "") +
                            (infoModel.isStatInfo ? "<stat/>" : "") +
                            (infoModel.isPrefsInfo ? "<prefs/>" : "") +
                            (infoModel.isGenInfo ?  "<gen_info/>" :"") +
                            "</dataset>" +
                            "</get>" +
                            "</site>" +
                            "</packet>";
            return packet;
        }

        public static  ObjectResult<string> sendRequest(DomainInfoModel infoModel)
        {
            ObjectResult<string> Result = new ObjectResult<string>();
            if (string.IsNullOrEmpty(createPackage(infoModel)))
            {
                Result.Fail("U2", "SiteNameCannotBeEmpty");
                return Result;
            }
            var response = Request.Send(createPackage(infoModel));
            Result.SetData(response.ToString());
            return Result;
        }
    }
}
