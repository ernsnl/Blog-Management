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
    public partial class LanguageList
    {
    	[DataMember]
        public long LanguageID { get; set; }
    	[DataMember]
        public long DomainID { get; set; }
    
    	[DataMember]
        public virtual Domain Domain { get; set; }
    }
}