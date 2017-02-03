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
    public partial class BlogContent
    {
        public BlogContent()
        {
            this.BlogSEOInformations = new HashSet<BlogSEOInformation>();
            this.BlogTranslations = new HashSet<BlogTranslation>();
            this.Tags = new HashSet<Tag>();
            this.BlogReviews = new HashSet<BlogReview>();
        }
    
    	[DataMember]
        public long ID { get; set; }
    	[DataMember]
        public string Name { get; set; }
    	[DataMember]
        public string CoverImgName { get; set; }
    	[DataMember]
        public string HtmlContent { get; set; }
    	[DataMember]
        public Nullable<long> CategoryID { get; set; }
    	[DataMember]
        public Nullable<byte> StatusID { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    	[DataMember]
        public Nullable<long> CreatedUser { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    	[DataMember]
        public Nullable<long> UpdatedUser { get; set; }
    	[DataMember]
        public Nullable<byte> BlogStatus { get; set; }
    	[DataMember]
        public Nullable<long> DomainID { get; set; }
    	[DataMember]
        public string BlogAbstract { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> PublishDate { get; set; }
    
    	[DataMember]
        public virtual Category Category { get; set; }
    	[DataMember]
        public virtual ICollection<BlogSEOInformation> BlogSEOInformations { get; set; }
    	[DataMember]
        public virtual ICollection<BlogTranslation> BlogTranslations { get; set; }
    	[DataMember]
        public virtual ICollection<Tag> Tags { get; set; }
    	[DataMember]
        public virtual ICollection<BlogReview> BlogReviews { get; set; }
    }
}