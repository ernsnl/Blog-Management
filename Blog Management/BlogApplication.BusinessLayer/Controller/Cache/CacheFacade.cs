using System;
using BlogApplication.Framework.Application.Server;

namespace BlogApplication.BusinessLayer.Controller.Cache
{
    public abstract class CacheFacade<TEntity> : Framework.Application.Server.CacheFacade<TEntity> where TEntity : class
    {
        private Facade oFacade;

        public Facade Facade
        {
            get { return this.oFacade; }
        }

        public CacheFacade(Facade facade) : base()
        {
            this.oFacade = facade;
        }
    }
}