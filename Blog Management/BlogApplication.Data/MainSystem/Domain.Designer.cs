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
    public partial class Domain
    {
        public Domain()
        {
            this.SEOInformations = new HashSet<SEOInformation>();
            this.NavigationEditors = new HashSet<NavigationEditor>();
            this.DomainSocials = new HashSet<DomainSocial>();
            this.MailLists = new HashSet<MailList>();
            this.LanguageLists = new HashSet<LanguageList>();
        }
    
    	[DataMember]
        public long ID { get; set; }
    	[DataMember]
        public string DomainName { get; set; }
    	[DataMember]
        public string DomainUrl { get; set; }
    	[DataMember]
        public string DomainImgName { get; set; }
    	[DataMember]
        public string SmtpHost { get; set; }
    	[DataMember]
        public string SmtpPassword { get; set; }
    	[DataMember]
        public string SmtpPort { get; set; }
    	[DataMember]
        public string Username { get; set; }
    	[DataMember]
        public Nullable<bool> EnableSSL { get; set; }
    	[DataMember]
        public Nullable<byte> StatusID { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> CreatedDate { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    	[DataMember]
        public Nullable<long> CreatedUser { get; set; }
    	[DataMember]
        public Nullable<long> UpdatedUser { get; set; }
    	[DataMember]
        public Nullable<long> DefaultLanguage { get; set; }
    	[DataMember]
        public string AdminEmail { get; set; }
    	[DataMember]
        public string AdminUsername { get; set; }
    	[DataMember]
        public string AdminPassword { get; set; }
    	[DataMember]
        public string ConsoleDomainUrl { get; set; }
    	[DataMember]
        public string DisqusUserName { get; set; }
    	[DataMember]
        public string FavIcon { get; set; }
    	[DataMember]
        public string CDNUrl { get; set; }
    	[DataMember]
        public string FTPUsername { get; set; }
    	[DataMember]
        public string FTPPassword { get; set; }
    	[DataMember]
        public string GoogleAnalyticsID { get; set; }
    	[DataMember]
        public string FacebookAppID { get; set; }
    	[DataMember]
        public string TwitterUserName { get; set; }
    
    	[DataMember]
        public virtual ICollection<SEOInformation> SEOInformations { get; set; }
    	[DataMember]
        public virtual ICollection<NavigationEditor> NavigationEditors { get; set; }
    	[DataMember]
        public virtual ICollection<DomainSocial> DomainSocials { get; set; }
    	[DataMember]
        public virtual ICollection<MailList> MailLists { get; set; }
    	[DataMember]
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
    }
}
