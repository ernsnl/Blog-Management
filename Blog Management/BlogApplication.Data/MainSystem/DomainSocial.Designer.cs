//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BlogApplication.Data.Visa;
using BlogApplication.Data.Blog;
using BlogApplication.Data.Translation;
using BlogApplication.Data.MainSystem;
using BlogApplication.Data.General;

namespace BlogApplication.Data.MainSystem
{
    [DataContract(IsReference = true)]
    public partial class DomainSocial
    {
    	[DataMember]
        public long ID { get; set; }
    	[DataMember]
        public Nullable<byte> SocialMediaType { get; set; }
    	[DataMember]
        public string SocialMediaUrl { get; set; }
    	[DataMember]
        public string SocialMediaPitch { get; set; }
    	[DataMember]
        public Nullable<long> DomainID { get; set; }
    	[DataMember]
        public Nullable<byte> StatusID { get; set; }
    
    	[DataMember]
        public virtual Domain Domain { get; set; }
    }
}