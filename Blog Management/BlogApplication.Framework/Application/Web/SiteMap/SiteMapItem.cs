using System;

namespace BlogApplication.Framework.Application.Web.SiteMap
{
    public class SiteMapItem : ISiteMapItem
    {

        public SiteMapItem(string url)
        {
            Url = url;
        }

        public SiteMapItem(string url, ChangeFrequency ChangeFrequency, double Priority, DateTime LastModified) : this(url)
        {
            this.ChangeFrequency = ChangeFrequency;
            this.Priority = Priority;
            this.LastModified = LastModified;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public double? Priority { get; set; }
    }
}