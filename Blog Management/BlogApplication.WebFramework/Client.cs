using System;
using System.Linq;
using BlogApplication.BusinessLayer;
using BlogApplication.Data.MainSystem;
using BlogApplication.Data.Visa;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.WebFramework
{
    public class Client : Framework.Application.Web.Client
    {
        private Facade oServices;
        private Domain oCurrentDomain;

        public Facade Services
        {
            get
            {
                if (this.oServices == null)
                    this.oServices = new Facade(this);
                return this.oServices;
            }
        }

        public Domain CurrentDomain
        {
            get
            {
                if (oCurrentDomain == null)
                {
                    oCurrentDomain = this.Services.ServiceController.MainSystem.GetDomain(CurrentDomainID).Data;
                    this.Services.CurrentDomain = oCurrentDomain;
                }
                return oCurrentDomain;
            }
            set
            {
                oCurrentDomain = value;
                this.Services.CurrentDomain = oCurrentDomain;
            }
        }

        public override int CurrentDomainID
        {
            get
            {
                if (Services.CurrentUser == null)
                {
                    if (Services.CurrentDomain == null)
                    {
                        return Convert.ToInt32(ConfigurationParameter.GetParameter("DomainID", "1"));
                    }
                    else
                    {
                        return Convert.ToInt32(Services.CurrentDomain.ID);
                    }
                }
                else
                {
                    return Convert.ToInt32(Services.CurrentUser.DomainID);
                }
            }
            set { base.CurrentDomainID = value; }
        }

        public override int CurrentLanguageID
        {
            get
            {
                if (this.Session["CurrentLanguageID"] == null)
                {
                    if (Services.CurrentUser == null)
                    {
                        this.Session["CurrentLanguageID"] = CurrentDomain.DefaultLanguage;
                        this.Session["CurrentLanguageShort"] =
                            this.Services.ServiceController.General.Language.GetLanguage(Convert.ToInt64(CurrentDomain.DefaultLanguage))
                                .Data.CodeISO;
                        return Convert.ToInt32(CurrentDomain.DefaultLanguage);
                    }
                    else
                    {
                        this.Session["CurrentLanguageID"] = Services.CurrentUser.MainLanguageID;
                        this.Session["CurrentLanguageShort"] =
                            this.Services.ServiceController.General.Language.GetLanguage(Convert.ToInt64(CurrentDomain.DefaultLanguage))
                        .Data.CodeISO;
                        return Convert.ToInt32(Services.CurrentUser.MainLanguageID);
                    }                   
                }
                return Convert.ToInt32(this.Session["CurrentLanguageID"]);
            }
            set
            {
                base.CurrentLanguageID = value;
                this.Session["CurrentLanguageID"] = value;
                this.Session["CurrentLanguageShort"] = this.Services.ServiceController.General.Language.GetLanguage(value)
                                .Data.CodeISO;
            }
        }

        public string CurrentLanguageShort
        {
            get
            {
                this.Session["CurrentLanguageID"] = CurrentLanguageID;
                return this.Session["CurrentLanguageShort"].ToString();

            }
        }

        public ObjectResult<User> Login(string username, string password)
        {
            ObjectResult<User> Result = new ObjectResult<User>();
            if (this.Services.CurrentUser == null)
            {
                Result = this.Services.ServiceController.Visa.Login(username, password);
                if (!Result.HasFailed)
                    this.Services.CurrentUser = Result.Data;
            }
            else
            {
                Result.SetData(this.Services.CurrentUser);
            }
            return Result;
        }

        public void Logout()
        {
            this.Services.CurrentUser = null;
            this.Session.Abandon();
        }


    }
}