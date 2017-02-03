using System.Runtime.Serialization;

namespace BlogApplication.Data.Visa
{
    public partial class User
    {
        [DataMember]
        public byte[] UploadedLogoData { get; set; }

        [DataMember]
        public string FileName { get; set; }

    }
}