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
namespace BlogApplication.Connection.MainSystem
{
    
    public partial class DomainRepository : 
    BlogApplication.Connection.Repository<BlogApplication.Data.MainSystem.Domain>,
    BlogApplication.Entity.MainSystem.IDomainRepository
    {
    	public DomainRepository(BlogApplication.Entity.IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }

}