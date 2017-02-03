using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum ReviewStatusResponse
    {
        [AttributeHelper("Deny")]
        Deny = 0,

        [AttributeHelper("Accept")]
        Accept = 1,

        [AttributeHelper("Additional Note")]
        Additional = 2
    }
}