using System;
using System.Diagnostics;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.General;
using BlogApplication.Entity.Visa;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.BusinessLayer.Controller.General
{
    public class CodeFacade : BusinessLayer.Facade
    {

        #region Code 

        public ObjectResult<Code> CreateCode(long UserID, CodeType CodeType)
        {
            ObjectResult<Code> Result = new ObjectResult<Code>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICodeRepository>();

                Code nCode = new Code();
                nCode.Code1 = RandomHelper.GenerateRandomNumber(15);
                nCode.StatusID = VariableValue.ActiveStatusID;
                nCode.ExpireDate = DateTime.Now.AddHours(12);
                nCode.Type = Convert.ToByte(CodeType);
                nCode.UserID = UserID;

                datasource.Add(nCode);
                datasource.SaveChanges();

                Result.SetData(nCode);
                return Result;

            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()),
                    Result.Messages, true);
                return Result;
            }
        }

        public ObjectResult<Code> CheckCode(string Code, string Email)
        {
            ObjectResult<Code> Result = new ObjectResult<Code>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICodeRepository>();
                Code persistent =
                    datasource.GetQuery()
                        .Where(op => op.Code1.Equals(Code) && op.ExpireDate > DateTime.Now)
                        .FirstOrDefault();
                if (persistent == null)
                {
                    Result.Fail("U2", "Code is expired. Please get a new code");
                    return Result;
                }
                var userDatasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                var userResult =
                    userDatasource.GetQuery()
                        .Where(op => op.ID == persistent.UserID && op.Email.Equals(Email) &&
                                op.StatusID == VariableValue.ActiveStatusID).FirstOrDefault();
                if (userResult == null)
                {
                    Result.Fail("U2", "User does not exist");
                    return Result;
                }
                Result.SetData(persistent);
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()),
                    Result.Messages, true);
                return Result;
            }
        }

        public ObjectResult<Code> DeactiveCode(string Code)
        {
            ObjectResult<Code> Result = new ObjectResult<Code>();
            var datasource = RepositoryFactory.Current.GetRepository<ICodeRepository>();
            Code persistent =
                    datasource.GetQuery()
                        .Where(op => op.Code1.Equals(Code) && op.ExpireDate > DateTime.Now)
                        .FirstOrDefault();
            persistent.StatusID = VariableValue.DeletedStatusID;
            datasource.SaveChanges();
            Result.SetData(persistent);
            return Result;
        }

        #endregion


        public CodeFacade(Client client) : base(client)
        {
        }
    }
}