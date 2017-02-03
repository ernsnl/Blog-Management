using System.Web.Mvc;

namespace BlogApplication.WebFramework.Controllers
{
    public abstract class BaseController : Controller
    {
        private Client oClient;

        public  Client Client
        {
            get
            {
                if (this.oClient == null)
                    this.oClient = new Client();
                return this.oClient;
            }
        }
    }
}