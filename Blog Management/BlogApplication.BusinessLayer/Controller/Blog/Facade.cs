using BlogApplication.Framework.Application.Web;

namespace BlogApplication.BusinessLayer.Controller.Blog
{
    public class Facade
    {
        private Client oClient;
        private BlogFacade oBlogFacade;
        private CategoryFacade oCategoryFacade;
        private TagFacade oTagFacade;
        private CustomPageFacade oCustomPageFacade;

        public BlogFacade Blog
        {
            get
            {
                if (oBlogFacade == null)
                    oBlogFacade = new BlogFacade(this.oClient);
                return oBlogFacade;
            }
        }

        public CategoryFacade Category
        {
            get
            {
                if (oCategoryFacade == null)
                    oCategoryFacade = new CategoryFacade(this.oClient);
                return oCategoryFacade;
            }
        }

        public TagFacade Tag
        {
            get
            {
                if(oTagFacade == null)
                    oTagFacade = new TagFacade(this.oClient);
                return oTagFacade;
            }
        }

        public CustomPageFacade CustomPage
        {
            get
            {
                if (oCustomPageFacade == null)
                    oCustomPageFacade = new CustomPageFacade(this.oClient);
                return oCustomPageFacade;
            }
        }

        public Facade(Client client)
        {
            oClient = new Client();
        }
    }
}