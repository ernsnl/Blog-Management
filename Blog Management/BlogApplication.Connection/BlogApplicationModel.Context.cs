﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlogApplication.Connection
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using BlogApplication.Entity;
    using BlogApplication.Data.Blog;
    using BlogApplication.Data.Visa;
    using BlogApplication.Data.General;
    using BlogApplication.Data.Translation;
    using BlogApplication.Data.MainSystem;
    
    public partial class BlogApplicationEntities : DbContext, IUnitOfWork
    {
        public BlogApplicationEntities()
            : base("name=BlogApplicationEntities")
        {
    		this.Configuration.LazyLoadingEnabled = false;
    		this.Configuration.ProxyCreationEnabled = false;
    		this.Configuration.ValidateOnSaveEnabled = false;
    		Debug.WriteLine("BlogApplicationEntities created with " + this.nInstanceID);
        }
    
    	private string nInstanceID = "";
        public string InstanceID {
            get {
                return this.nInstanceID;
            }
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    	
    	public new void SaveChanges()
        {
            base.SaveChanges();
        }
    
        public DbSet<BlogContent> BlogContents { get; set; }
        public DbSet<BlogSEOInformation> BlogSEOInformations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomPageContent> CustomPageContents { get; set; }
        public DbSet<CustomPageSEOInformation> CustomPageSEOInformations { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<SEOInformation> SEOInformations { get; set; }
        public DbSet<BlogTranslation> BlogTranslations { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Common> Commons { get; set; }
        public DbSet<CustomPageTranslation> CustomPageTranslations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BlogReview> BlogReviews { get; set; }
        public DbSet<NavigationEditor> NavigationEditors { get; set; }
        public DbSet<DomainSocial> DomainSocials { get; set; }
        public DbSet<MailList> MailLists { get; set; }
        public DbSet<LanguageList> LanguageLists { get; set; }
    }
}
