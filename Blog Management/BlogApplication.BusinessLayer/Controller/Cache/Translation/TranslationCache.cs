using System.Collections.Generic;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Translation;
using BlogApplication.Entity.General;
using BlogApplication.Entity.Translation;

namespace BlogApplication.BusinessLayer.Controller.Cache.Translation
{
    public class TranslationCache : CacheFacade<Common>
    {
        protected override string Key
        {
            get { return "Translations"; }
        }

        protected override List<Common> GetData()
        {
            var datasource = RepositoryFactory.Current.GetRepository<ICommonRepository>();
            return datasource.GetQuery().ToList();
        }

        public TranslationCache(Cache.Facade Facade) : base (Facade)
        {
        }
    }
}