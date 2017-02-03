using System.Collections.Generic;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.General;

namespace BlogApplication.BusinessLayer.Controller.Cache.General
{
    public class LanguageCache : CacheFacade<Language>
    {
        protected override string Key
        {
            get { return "Languages"; }
        }

        protected override List<Language> GetData()
        {
            var datasource = RepositoryFactory.Current.GetRepository<ILanguageRepository>();
            return datasource.GetQuery().Where(op => op.StatusID != VariableValue.DeletedStatusID).ToList();
        }

        public LanguageCache(Cache.Facade Facade) : base (Facade)
        {
        }
    }
}