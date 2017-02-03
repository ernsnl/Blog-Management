namespace BlogApplication.Framework.PleskAPI.NodeModels
{
    public class VRHostModel
    {
        public string runTimeVersion = "4.5";

        public string rootDirectory { get; set; }

        public bool aspSupport = true;

        public bool aspNetSupport = true;

        public bool webStatProtected = true;

        public bool webDeploy = true;

        public string webStat { get; set; }
    }
}