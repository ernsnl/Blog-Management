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

namespace BlogApplication.Data.Blog
{
    [DataContract(IsReference = true)]
    public partial class BlogReview
    {
    	[DataMember]
        public long ID { get; set; }
    	[DataMember]
        public Nullable<long> BlogID { get; set; }
    	[DataMember]
        public string ReviewMessage { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    	[DataMember]
        public Nullable<long> CreatedUser { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    	[DataMember]
        public Nullable<long> UpdatedUser { get; set; }
    
    	[DataMember]
        public virtual BlogContent BlogContent { get; set; }
    }
}
