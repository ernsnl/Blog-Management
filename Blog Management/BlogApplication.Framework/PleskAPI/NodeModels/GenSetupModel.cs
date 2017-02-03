using System.ComponentModel;
using System.Web.Management;

namespace BlogApplication.Framework.PleskAPI.NodeModels
{
    public class GenSetupModel
    {
        public string name { get; set; }

        public string htype { get; set; }

        public string status { get; set; }

        public string webspace_name { get; set; }

        public string webspace_id { get; set; }

        public string webspace_guid { get; set; }

        public int parent_site_id { get; set; }

        public string parent_site_name { get; set; }

        public string parent_site_guid { get; set; }
    }
}