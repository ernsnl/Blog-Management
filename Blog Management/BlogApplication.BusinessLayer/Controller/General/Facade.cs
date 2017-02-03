using BlogApplication.BusinessLayer.Controller.Blog;
using BlogApplication.Framework.Application.Web;

namespace BlogApplication.BusinessLayer.Controller.General
{
    public class Facade
    {
        private Client oClient;
        private LanguageFacade oLanguageFacade;
        private CodeFacade oCodeFacade;

        public LanguageFacade Language
        {
            get
            {
                if (oLanguageFacade == null)
                    oLanguageFacade = new LanguageFacade(this.oClient);
                return oLanguageFacade;
            }
        }

        public CodeFacade Code
        {
            get
            {
                if (oCodeFacade == null)
                    oCodeFacade = new CodeFacade(this.oClient);
                return oCodeFacade;
            }
        }

        public Facade(Client client)
        {
            oClient = new Client();
        }
    }
}