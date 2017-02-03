namespace BlogApplication.Framework.PleskAPI.NodeModels
{
    public class HostingModel
    {
        public bool isVRTHost { get; set; }

        public bool isStdFwd { get; set; }

        public bool isFrmFwd { get; set; }

        public DomainFFHostingModel domainFF { get; set; }

        public DomainSFHostingModel domainSF { get; set; }

        public VRHostModel vrHost { get; set; }
    }
}