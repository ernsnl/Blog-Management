using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BlogApplication.Connection;
using BlogApplication.Data.General;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.General;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.BusinessLayer.Controller.General
{
    public class LanguageFacade : BusinessLayer.Facade
    {
        #region Language

        public ObjectResult<Language> EditLanugage(Language nLanguage)
        {
            ObjectResult<Language> Result = new ObjectResult<Language>();
            try
            {

                #region Controls

                if (nLanguage.StatusID != VariableValue.DeletedStatusID)
                {
                    if (nLanguage == null)
                        Result.Fail("U2", "LanguageCannotBeEmpty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nLanguage.Name))
                        Result.Fail("U2", "LanguageNameCannotBeEmpty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nLanguage.CodeISO))
                        Result.Fail("U2", "ISOCodeCannotBeEmpty");
                    if (!Result.HasFailed &&
                        !StringHelper.checkFormat(nLanguage.CodeISO, new Regex(@"^[a-z]{2}-[A-Z]{2}$")))
                        Result.Fail("U2", "ISOCodeIsNotInCorrectFormat");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nLanguage.CodeISO_3))
                        Result.Fail("U2", "ISOCode3CannotBeEmpty");
                    if (!Result.HasFailed &&
                        !StringHelper.checkFormat(nLanguage.CodeISO_3, new Regex(@"^[a-z]{3}-[A-Z]{3}$")))
                        Result.Fail("U2", "ISOCode3IsNotInCorrectFormat");
                }

                #endregion


                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<ILanguageRepository>();
                    if (!datasource.GetQuery().Where(op => op.Name.Equals(nLanguage.Name)
                                                           && nLanguage.ID != op.ID
                                                           && VariableValue.DeletedStatusID != op.StatusID).Any())
                    {
                        Language persistent = new Language();

                        if (nLanguage.ID > 0)
                            persistent = datasource.GetQuery().Where(op => op.ID == nLanguage.ID).FirstOrDefault();

                        if (nLanguage.StatusID != VariableValue.ConvertStatusTypesByte(StatusType.Deleted))
                        {
                            persistent.Name = nLanguage.Name;
                            persistent.CodeISO = nLanguage.CodeISO;
                            persistent.CodeISO_3 = nLanguage.CodeISO_3;
                        }
                        else
                        {
                            this.ServiceController.Translation.DeleteLanguage(nLanguage.ID);
                        }
                        persistent.StatusID = nLanguage.StatusID;

                        if (nLanguage.ID == 0)
                        {
                            persistent.CreatedDate = DateTime.Now;
                            persistent.CreatedUser = 0;
                            datasource.Add(persistent);
                        }
                        else
                        {
                            persistent.UpdatedDate = DateTime.Now;
                            persistent.UpdatedUser = 0;
                        }

                        datasource.SaveChanges();

                        Result.SetData(persistent);

                    }
                    else
                    {
                        Result.Fail("U2", "LanguageNameAlreadyExists");
                    }
                }

                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()),
                    Result.Messages, true);
                return Result;

            }
            finally
            {
                this.ServiceController.Caching.General.Lanugages.DropCache();
            }
        }

        public CollectionResult<Language> GetLanguageList(int page = 1, int pageSize = 25)
        {
            CollectionResult<Language> Result = new CollectionResult<Language>();
            try
            {
                List<Language> persistent = new List<Language>();
                persistent = this.ServiceController.Caching.General.Lanugages.List.OrderBy(op => op.ID).ToList();
                Result.SetData(persistent.Count, persistent.Paginate(page, pageSize).ToList());
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

        public ObjectResult<Language> GetLanguage(long languageID)
        {
            ObjectResult<Language> Result = new ObjectResult<Language>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ILanguageRepository>();

                Language persistent = new Language();
                persistent = this.ServiceController.Caching.General.Lanugages.List
                    .Where(op => op.ID == languageID && op.StatusID != VariableValue.DeletedStatusID)
                        .FirstOrDefault();
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

        public ObjectResult<Language> GetLanguage(string codeISO)
        {
            ObjectResult<Language> Result = new ObjectResult<Language>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ILanguageRepository>();

                Language persistent = new Language();
                persistent = this.ServiceController.Caching.General.Lanugages.List
                    .Where(op => op.CodeISO.Equals(codeISO) && op.StatusID != VariableValue.DeletedStatusID)
                        .FirstOrDefault();
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

        #endregion

        public LanguageFacade(Client client) : base(client)
        {

        }
    }
}