using BlogApplication.Framework.Attribute;

namespace BlogApplication.Data.GlobalTypes
{
    public enum SocialTypes : byte
    {
        [AttributeHelper("Facebook")]
        Facebook = 1,

        [AttributeHelper("Twitter")]
        Twitter = 2,

        [AttributeHelper("Youtube")]
        Youtube = 3,

        [AttributeHelper("Tumblr")]
        Tumblr = 4,

        [AttributeHelper("Patreon")]
        Patreon = 5,

        [AttributeHelper("LinkedIn")]
        LinkedIn = 6,

        [AttributeHelper("Pinterest")]
        Pinterest = 7,

        [AttributeHelper("Instagram")]
        Instagram = 8,
    }
}