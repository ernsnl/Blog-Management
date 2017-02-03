namespace BlogApplication.BusinessLayer.Controller.Cache.General
{
    public class Facade
    {
        private Cache.Facade oFacade;
        private LanguageCache oLanguages;


        public Cache.Facade Caching
        {
            get { return this.oFacade; }
        }

        public LanguageCache Lanugages
        {
            get
            {
                if(this.oLanguages == null)
                    this.oLanguages = new LanguageCache(this.Caching);
                return this.oLanguages;
            }
        }


        public Facade(Cache.Facade facade)
        {
            this.oFacade = facade;
        }
    }
}