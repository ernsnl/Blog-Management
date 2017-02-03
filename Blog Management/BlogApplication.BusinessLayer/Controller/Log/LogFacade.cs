using System;
using System.Collections.Generic;
using System.Diagnostics;
using BlogApplication.Connection;
using BlogApplication.Entity.General;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.BusinessLayer.Controller.Log
{
    public class LogFacade : Facade
    {
        public ObjectResult<Data.General.Log> SendLog(string funtcionName, List<ResultMessage> Messages, bool isFailed)
        {
            ObjectResult<Data.General.Log> Result = new ObjectResult<Data.General.Log>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ILogRepository>();
                var Persistent = new Data.General.Log();
                Persistent.CreatedDate = DateTime.Today;
                Persistent.LogContent = funtcionName;
                Persistent.LogContent += " " + (isFailed ? "hasFailed" : "hasSucceed");
                foreach (var mes in Messages)
                {
                    Persistent.LogContent += mes.Code + ":" + mes.Description;
                }
                datasource.Add(Persistent);
                datasource.SaveChanges();
                Result.SetData(Persistent);
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                return Result;
            }
        }

        public LogFacade(ControllerFacade facade) : base(new Client())
        {

        }
    }
}