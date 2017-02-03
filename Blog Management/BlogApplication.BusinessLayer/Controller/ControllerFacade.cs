using System;
using System.Diagnostics;
using System.Net.Configuration;
using BlogApplication.BusinessLayer.Controller.Blog;
using BlogApplication.BusinessLayer.Controller.General;
using BlogApplication.BusinessLayer.Controller.Log;
using BlogApplication.BusinessLayer.Controller.MainSystem;
using BlogApplication.BusinessLayer.Controller.Translation;
using BlogApplication.BusinessLayer.Controller.Visa;
using BlogApplication.Framework.Application.Server;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.BusinessLayer.Controller
{
    public class ControllerFacade
    {
        private Facade oFacade;
        private VisaFacade oVisaFacade;
        private General.Facade oGeneralFacade;
        private TranslationFacade oTranslationFacade;
        private Blog.Facade oBlogFacade;
        private LogFacade oLogFacade;
        private MainSystemFacade oMainSystemFacade;
        private Cache.Facade oCaching;


        public Cache.Facade Caching
        {
            get
            {
                if(oCaching == null)
                    oCaching = new Cache.Facade(this);
                return oCaching;
            }
        }

        public VisaFacade Visa
        {
            get
            {
                if(oVisaFacade == null)
                    oVisaFacade = new VisaFacade(this);
                return oVisaFacade;
            }
        }

        public TranslationFacade Translation
        {
            get
            {
                if(oTranslationFacade == null)
                    oTranslationFacade = new TranslationFacade(this);
                return oTranslationFacade;;
            }
        }

        public General.Facade General
        {
            get
            {
                if (oGeneralFacade == null)
                    oGeneralFacade = new General.Facade(oFacade.Client);
                return oGeneralFacade;
            }
        }

        public Blog.Facade BlogContent {
            get
            {
                if(oBlogFacade == null)
                    oBlogFacade = new Blog.Facade(oFacade.Client);
                return oBlogFacade;
            }
        }
        public MainSystemFacade MainSystem
        {
            get
            {
                if (oMainSystemFacade == null)
                    oMainSystemFacade = new MainSystemFacade(this);
                return oMainSystemFacade;
            }
        }

        public LogFacade Log
        {
            get
            {
                if (oLogFacade == null)
                    oLogFacade = new LogFacade(this);
                return oLogFacade;
            }
        }

        public ObjectResult<bool> ClearCache()
        {
            ObjectResult<bool> Result = new ObjectResult<bool>();
            try
            {
                CacheManager.ClearAll();
                Result.SetData(true);
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
        }

        public ControllerFacade(Facade facade)
        {
            oFacade = facade;
        }
    }
}