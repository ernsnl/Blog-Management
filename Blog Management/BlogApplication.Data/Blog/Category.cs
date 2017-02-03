using System.Runtime.Serialization;

namespace BlogApplication.Data.Blog
{
    public partial class Category
    {
        [DataMember]
        public string Filename { get; set; }

        [DataMember]
        public byte[] FileData { get; set; }
    }
}