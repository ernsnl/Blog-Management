using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.Blog;

namespace BlogApplication.BusinessLayer.Controller.Cache.Blog
{
    public class CategoryCache : CacheFacade<Category>
    {
        protected override string Key
        {
            get { return "Categories"; }
        }

        protected override List<Category> GetData()
        {
            var datasource = RepositoryFactory.Current.GetRepository<ICategoryRepository>();
            return datasource.GetQuery().Where(op => op.StatusID != VariableValue.DeletedStatusID).Include(op => op.CategoryTranslations).ToList();
        }

        public CategoryCache(Cache.Facade Facade) : base(Facade)
        {

        }
    }
}