using System.Runtime.CompilerServices;

namespace BlogApplication.BusinessLayer.Controller.Cache.Blog
{
    public class Facade
    {
        private Cache.Facade oFacade;
        private TagCache oTags;
        private CategoryCache oCategories;

        public Cache.Facade Caching
        {
            get { return this.oFacade; }
        }

        public TagCache Tags
        {
            get
            {
                if(this.oTags == null)
                    this.oTags = new TagCache(this.Caching);
                return this.oTags;
            }
        }

        public CategoryCache Categories
        {
            get
            {
                if (this.oCategories == null)
                    this.oCategories = new CategoryCache(this.Caching);
                return this.oCategories;
            }
        }

        public Facade(Cache.Facade facade)
        {
            this.oFacade = facade;
        }
    }
}