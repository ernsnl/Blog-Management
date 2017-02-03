using System;
using BlogApplication.BusinessLayer.Controller;
using BlogApplication.Data.MainSystem;
using BlogApplication.Data.Visa;
using BlogApplication.Framework.Application.Web;

namespace BlogApplication.BusinessLayer
{
    public class Facade
    {
        private Client oClient;

        private Controller.ControllerFacade oControllerFacade;

        public Client Client
        {
            get { return this.oClient; }
        }

        public User CurrentUser
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null &&
                    System.Web.HttpContext.Current.Session["CurrentUser"] != null)
                    return (User)System.Web.HttpContext.Current.Session["CurrentUser"];
                else
                    return null;
            }
            set
            {
                System.Web.HttpContext.Current.Session["CurrentUser"] = value;
                if (value != null)
                    this.oClient.CurrentLanguageID = Convert.ToInt32(value.MainLanguageID);
            }
        }

        public Domain CurrentDomain
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null &&
                    System.Web.HttpContext.Current.Session["CurrentDomain"] != null)
                    return (Domain)System.Web.HttpContext.Current.Session["CurrentDomain"];
                else
                    return null;
            }
            set
            {
                System.Web.HttpContext.Current.Session["CurrentDomain"] = value;
                if (value != null)
                    this.oClient.CurrentDomainID = Convert.ToInt32(value.ID);
            }
        }

        public bool isAuthenticated
        {
            get { return CurrentUser != null ? CurrentUser.ID > 0 : false; }
        }

        public Controller.ControllerFacade ServiceController
        {
            get
            {
                if (this.oControllerFacade == null)
                    this.oControllerFacade = new ControllerFacade(this);
                return oControllerFacade;
            }
        }

        public Facade(Client client)
        {
            this.oClient = client;
        }
    }
}