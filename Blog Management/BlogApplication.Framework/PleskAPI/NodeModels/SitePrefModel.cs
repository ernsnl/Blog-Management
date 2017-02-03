using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Framework.PleskAPI.NodeModels
{
    public class SitePrefModel
    {
        public bool wwwPref { get; set; }

        [Range(0,999999)]
        public int stat_TTL { get; set; }
    }
}