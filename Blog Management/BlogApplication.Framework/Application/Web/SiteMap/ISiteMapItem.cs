using System;

namespace BlogApplication.Framework.Application.Web.SiteMap
{
    public interface ISiteMapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        ChangeFrequency? ChangeFrequency { get; }
        double? Priority { get; }
    }
}