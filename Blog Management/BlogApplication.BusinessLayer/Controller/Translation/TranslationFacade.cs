using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.General;
using BlogApplication.Data.Translation;
using BlogApplication.Entity.Translation;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.BusinessLayer.Controller.Translation
{
    public class TranslationFacade : Facade
    {
        #region Common 

        public ObjectResult<Common> GetTranslation(string keyWord, long languageID)
        {
            ObjectResult<Common> Result = new ObjectResult<Common>();
            try
            {
                if (this.ServiceController.Caching.Translation.Translations.List.Where(
                        op => op.Keyword.Equals(keyWord) && op.LanguageID == languageID).Any())
                {
                    Common persistent = new Common();

                    persistent = this.ServiceController.Caching.Translation.Translations.List
                        .Where(op => op.Keyword.Equals(keyWord) && op.LanguageID == languageID).FirstOrDefault();

                    Result.SetData(persistent);
                }

                else
                {
                    Result.Fail("U2","TranslationDoesNotExist");
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
        }

        public CollectionResult<Common> GetTranslationsOfTheKey(string keyWord)
        {
            CollectionResult<Common> Result = new CollectionResult<Common>();
            try
            {
                if (this.ServiceController.Caching.Translation.Translations.List.Where(op => op.Keyword.ToLower().Equals(keyWord.ToLower())).Any())
                {
                    List<Common> persistent = new List<Common>();
                    persistent = this.ServiceController.Caching.Translation.Translations.List.Where(op => op.Keyword.ToLower().Equals(keyWord.ToLower())).ToList();
                    Result.SetData(persistent);

                }
                else
                {

                    Result.Fail("U2","KeywordDoesNotExist");
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
        }

        public CollectionResult<Common> GetTranlations(long languageID = 0, int page = 1, int pageSize = 25)
        {
            CollectionResult<Common> Result = new CollectionResult<Common>();
            try
            {
                List<Common> persistent = new List<Common>();
                persistent = this.ServiceController.Caching.Translation.Translations.List;

                if (languageID > 0)
                    persistent = persistent.Where(op => op.LanguageID == languageID).ToList();

                persistent = persistent.OrderBy(op => op.Keyword).ToList();
                Result.SetData(persistent.Count, persistent.Paginate(page,pageSize).ToList());

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

        public ObjectResult<bool> DeleteLanguage(long languageID)
        {
            ObjectResult<bool> Result = new ObjectResult<bool>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICommonRepository>();
                List<Common> translationList = new List<Common>();

                translationList = datasource.GetQuery().Where(op => op.LanguageID == languageID).ToList();
                foreach (var translation in translationList)
                {
                    datasource.Delete(translation);
                }
                datasource.SaveChanges();

                Result.SetData(true);

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
                this.ServiceController.Caching.Translation.Translations.DropCache();
            }
        }

        public ObjectResult<Common> EditTranslation(Common nTranslation)
        {
            ObjectResult<Common> Result = new ObjectResult<Common>();
            try
            {
                #region Controls

                
                if (nTranslation == null)
                    Result.Fail("U2", "TranslationCannotBeEmpty");
                if (!Result.HasFailed && string.IsNullOrEmpty(nTranslation.Keyword))
                    Result.Fail("U2", "KeywordCannotBeEmpty");
                if (!Result.HasFailed && string.IsNullOrEmpty(nTranslation.Translation))
                    Result.Fail("U2", "TranslationCannotBeEmpty");
                if (!Result.HasFailed && nTranslation.LanguageID == 0)
                    Result.Fail("U2", "LanguageCannotBeEmpty");


                #endregion

                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<ICommonRepository>();
                   
                        Common persistent = new Common();
                        if (!string.IsNullOrEmpty(nTranslation.Keyword))
                            persistent =
                                datasource.GetQuery()
                                    .Where(op => op.Keyword.Equals(nTranslation.Keyword) && op.LanguageID == nTranslation.LanguageID)
                                    .FirstOrDefault();

                        persistent.Keyword = nTranslation.Keyword;
                        persistent.LanguageID = nTranslation.LanguageID;
                        persistent.Translation = nTranslation.Translation;
                        datasource.SaveChanges();
                        Result.SetData(persistent);
                    
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
                this.ServiceController.Caching.Translation.Translations.DropCache();
            }

        }

        public ObjectResult<Common> AddTranslation(string keyWord)
        {
            ObjectResult<Common> Result = new ObjectResult<Common>();
            try
            {
                List<Language> languageList = this.ServiceController.Caching.General.Lanugages.List;
                var datasource = RepositoryFactory.Current.GetRepository<ICommonRepository>();
                foreach (var lang in languageList)
                {
                    if (!datasource.GetQuery().Where(op => op.Keyword.Equals(keyWord) && op.LanguageID == lang.ID).Any())
                    {
                        Common persistent = new Common();
                        persistent.Keyword = keyWord;
                        persistent.Translation = keyWord;
                        persistent.LanguageID = lang.ID;
                        datasource.Add(persistent);
                    }
                }          
                datasource.SaveChanges();
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
                this.ServiceController.Caching.Translation.Translations.DropCache();
            }
        }

        public ObjectResult<Common> AddTranslationToOneLanguage(string keyWord, long lanugageID)
        {
            ObjectResult<Common> Result = new ObjectResult<Common>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICommonRepository>();

                Common persistent = new Common();
                persistent.Keyword = keyWord;
                persistent.Translation = keyWord;
                persistent.LanguageID = lanugageID;
                datasource.Add(persistent);

                Result.SetData(persistent);
                datasource.SaveChanges();

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
                this.ServiceController.Caching.Translation.Translations.DropCache();
            }
        }

        #endregion

        public TranslationFacade(ControllerFacade facade) : base(new Client())
        {

        }
    }
}