using System;
using System.Diagnostics;
using BlogApplication.Framework.Application.Server;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.BusinessLayer.Controller.Cache
{
    public class Facade
    {
        private ControllerFacade oFacade;
        private General.Facade oGeneral;
        private Translation.Facade oTranslation;
        private Blog.Facade oBlog;

        public General.Facade General
        {
            get
            {
                if (this.oGeneral == null)
                    this.oGeneral = new General.Facade(this);
                return this.oGeneral;
            }
        }

        public Translation.Facade Translation
        {
            get
            {
                if (this.oTranslation == null)
                    this.oTranslation = new Translation.Facade(this);
                return this.oTranslation;
            }
        }

        public Blog.Facade Blog
        {
            get
            {
                if(this.oBlog == null)
                    this.oBlog = new Blog.Facade(this);
                return this.oBlog;
            }
        }

        internal ControllerFacade API
        {
            get { return this.oFacade; }
        }

        public Facade(ControllerFacade facade)
        {
            this.oFacade = facade;
        }
    }
}