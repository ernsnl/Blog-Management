namespace BlogApplication.Framework.PleskAPI.NodeModels
{
    public class DomainInfoModel
    {
        public string siteName { get; set; }

        public bool isHostingInfo { get; set; }

        public bool isStatInfo { get; set; }

        public bool isPrefsInfo { get; set; }

        public bool isDiskUsageInfo { get; set; }

        public bool isGenInfo { get; set; }
    }
}