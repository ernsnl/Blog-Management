using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum DomainLocationSEO
    {
        [AttributeHelper("Home Page")]
        HomePage = 1,

        [AttributeHelper("View News")]
        ViewNews = 2
    }
}