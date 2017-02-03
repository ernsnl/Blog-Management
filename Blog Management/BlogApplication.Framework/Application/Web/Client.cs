using System;
using System.Web;
using System.Web.SessionState;
using BlogApplication.Framework.Configuration;
using BlogApplication.Framework.Extensions.WebExtensions;

namespace BlogApplication.Framework.Application.Web
{
    public class Client
    {
        private string sSessionID;
        private string sUserHostAddress = string.Empty;
        public string ApplicationName { get; set; }
        protected HttpContext Context
        {
            get { return HttpContext.Current; }
        }
        public HttpApplicationState Application
        {
            get { return this.Context.Application; }
        }
        public HttpSessionState Session
        {
            get { return this.Context.Session; }
        }
        public HttpResponse Response
        {
            get { return this.Context.Response; }
        }
        public HttpRequest Request
        {
            get { return this.Context.Request; }
        }
        public string ComputerName
        {
            get { return System.Net.Dns.GetHostName(); }
        }
        public string UserHostAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this.sUserHostAddress)) this.sUserHostAddress = this.Request.GetUserHostAddress();
                return this.sUserHostAddress;
            }
        }
        public string UserAgent
        {
            get
            {
                return this.Context.Request.UserAgent;
            }
        }
        public string SessionID
        {
            get
            {
                if (string.IsNullOrEmpty(this.sSessionID)) this.sSessionID = this.Session.GetSessionID();
                return this.sSessionID;
            }
        }

        public long LoggedInUserCount
        {
            get
            {
                if (this.Application != null)
                {
                    if (this.Application["LoggedInUserCount"] == null)
                        this.Application["LoggedInUserCount"] = 0;
                    return Convert.ToInt64(this.Application["LoggedInUserCount"]);
                }
                return 0;
            }
            set
            {
                if (this.Application != null)
                    this.Application["LoggedInUserCount"] = value;
            }
        }

        public long DomainID
        {
            get { return Convert.ToInt64(ConfigurationParameter.GetParameter("DomainID")); }
        }

        public virtual int CurrentDomainID
        {
            get
            {
                if (this.Session != null)
                {
                    if (this.Session["CurrentDomainID"] == null)
                        this.Session["CurrentDomainID"] = DomainID;
                    return Convert.ToInt32(this.Session["CurrentDomainID"]);
                }
                return 0;
            }
            set
            {
                if (this.Session != null)
                {
                    this.Session["CurrentDomainID"] = value;
                }
            }
        }

        public virtual int CurrentLanguageID
        {
            get
            {
                if (this.Session != null)
                {
                    if (this.Session["CurrentLanguageID"] == null)
                        this.Session["CurrentLanguageID"] = 0;
                    return Convert.ToInt32(this.Session["CurrentLanguageID"]);
                }
                return 0;
            }
            set
            {
                if (this.Session != null)
                {
                    this.Session["CurrentLanguageID"] = value;
                }
            }
        }

        public virtual void Disconnect()
        {

        }
    }
}