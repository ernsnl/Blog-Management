namespace BlogApplication.BusinessLayer.Controller.Cache.Translation
{
    public class Facade
    {
        private Cache.Facade oFacade;

        private TranslationCache oTranslations;

        public Cache.Facade Caching
        {
            get { return this.oFacade; }
        }

        public TranslationCache Translations
        {
            get
            {
                if(this.oTranslations == null)
                    this.oTranslations = new TranslationCache(this.Caching);
                return this.oTranslations;
            }
        }

        public Facade(Cache.Facade facade)
        {
            this.oFacade = facade;
        }


    }
}