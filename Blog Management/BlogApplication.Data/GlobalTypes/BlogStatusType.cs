using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum BlogStatusType
    {
        [AttributeHelper("Published")]
        Published = 1,

        [AttributeHelper("InReview")]
        InReview = 2,

        [AttributeHelper("Denied")]
        Denied = 3,

        [AttributeHelper("Waiting Review")]
        WaitingReview = 4
    }
}