using System.Collections.Generic;
using System.Linq;
using BlogApplication.BusinessLayer.Controller.Cache.General;
using BlogApplication.Connection;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.Blog;

namespace BlogApplication.BusinessLayer.Controller.Cache.Blog
{
    public class TagCache : CacheFacade<Tag>
    {
        protected override string Key
        {
            get { return "Tags"; }
        }

        protected override List<Tag> GetData()
        {
            var datasource = RepositoryFactory.Current.GetRepository<ITagRepository>();
            return datasource.GetQuery().Where(op => op.StatusID != VariableValue.DeletedStatusID).ToList();
        }

        public TagCache(Cache.Facade Facade) : base(Facade)
        {

        }
    }
}