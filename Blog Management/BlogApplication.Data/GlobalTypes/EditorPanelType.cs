using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum EditorPanelType : byte
    {
        [AttributeHelper("Navigation Panel Editor")]
        NavigationPanel = 1,

        [AttributeHelper("SiteMap Panel Editor")]
        SiteMapPanel = 2
    }
}