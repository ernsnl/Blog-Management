using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum CodeType : byte
    {
        [AttributeHelper("Welcome")]
        Welcome = 1,

        [AttributeHelper("Reset")]
        Reset = 2
    }
}