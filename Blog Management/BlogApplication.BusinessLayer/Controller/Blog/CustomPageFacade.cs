using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BlogApplication.Connection;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.Translation;
using BlogApplication.Entity.Blog;
using BlogApplication.Entity.Translation;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.BusinessLayer.Controller.Blog
{
    public class CustomPageFacade : BusinessLayer.Facade
    {
        #region CustomPage

        public CollectionResult<CustomPageContent> GetCustomPageList(int page = 1, int pageSize = 25)
        {
            CollectionResult<CustomPageContent> Result = new CollectionResult<CustomPageContent>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICustomPageContentRepository>();

                List<CustomPageContent> persistent = new List<CustomPageContent>();
                persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID &&
                op.StatusID != VariableValue.DeletedStatusID).OrderByDescending(op => op.UpdatedDate).ToList();
                Result.SetData(persistent.Count, persistent.Paginate(page, pageSize).ToList());
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
            finally
            {
                RepositoryFactory.Dispose();
            }

        }

        public ObjectResult<CustomPageContent> GetCustomPage(long ID)
        {
            ObjectResult<CustomPageContent> Result = new ObjectResult<CustomPageContent>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICustomPageContentRepository>();
                if (datasource.GetQuery().Where(
                        op => op.DomainID == this.Client.CurrentDomainID
                        && op.ID == ID && VariableValue.DeletedStatusID != op.StatusID).Any())
                {
                    CustomPageContent persistent = new CustomPageContent();
                    persistent = datasource.GetQuery()
                            .Where(op => op.StatusID != VariableValue.DeletedStatusID && op.ID == ID)
                            .Include(op => op.CustomPageTranslations).Include(op => op.CustomPageSEOInformations)
                            .FirstOrDefault();
                    Result.SetData(persistent);
                }
                else
                {
                    Result.Fail("U2", "Custom Page Content Does Not Exist");
                }
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
            finally
            {
                RepositoryFactory.Dispose();
            }

        }

        public ObjectResult<CustomPageContent> GetCustomPage(string Url)
        {
            ObjectResult<CustomPageContent> Result = new ObjectResult<CustomPageContent>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICustomPageContentRepository>();
                if (datasource.GetQuery().Where(
                        op => op.DomainID == this.Client.CurrentDomainID && op.CustomPageUrl.Equals(Url) && VariableValue.DeletedStatusID != op.StatusID).Any())
                {
                    CustomPageContent persistent = new CustomPageContent();
                    persistent = datasource.GetQuery()
                            .Where(op => op.StatusID != VariableValue.DeletedStatusID
                            && op.CustomPageUrl.Equals(Url) && op.DomainID == this.Client.CurrentDomainID)
                            .Include(op => op.CustomPageTranslations).Include(op => op.CustomPageSEOInformations)
                            .FirstOrDefault();
                    Result.SetData(persistent);
                }
                else
                {
                    Result.Fail("U2", "Custom Page Content Does Not Exist");
                }
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
            finally
            {
                RepositoryFactory.Dispose();
            }

        }

        public ObjectResult<CustomPageContent> EditCustomPageContent(CustomPageContent customPageContent)
        {
            ObjectResult<CustomPageContent> Result = new ObjectResult<CustomPageContent>();
            try
            {
                if (VariableValue.DeletedStatusID != customPageContent.StatusID)
                {
                    if (customPageContent == null)
                        Result.Fail("U2", "Category Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(customPageContent.CustomPageUrl))
                        Result.Fail("U2", "Blog Title Cannot Be Empty");
                    if (!Result.HasFailed && !StringHelper.checkFormat(customPageContent.CustomPageUrl, new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$")))
                        Result.Fail("U2", "Custom Page URL must only include alpha-numeric characters");

                }

                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<ICustomPageContentRepository>();
                    if (!datasource.GetQuery().Where(
                            op => op.DomainID == this.Client.CurrentDomainID && op.ID != customPageContent.ID
                            && op.CustomPageUrl.Equals(customPageContent.CustomPageUrl)
                            && VariableValue.DeletedStatusID != customPageContent.StatusID).Any())
                    {
                        CustomPageContent Persistent = new CustomPageContent();
                        if (customPageContent.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID && op.ID == customPageContent.ID)
                                    .FirstOrDefault();

                        if (VariableValue.DeletedStatusID != customPageContent.StatusID)
                        {
                            Persistent.CustomPageUrl = customPageContent.CustomPageUrl;
                            Persistent.DomainID = this.Client.CurrentDomainID;
                            Persistent.HtmlContent = customPageContent.HtmlContent;
                            Persistent.Title = customPageContent.Title;


                        }
                        Persistent.StatusID = customPageContent.StatusID;
                        Persistent.UpdatedDate = DateTime.Now;
                        Persistent.UpdatedUser = this.CurrentUser.ID;

                        if (customPageContent.ID == 0)
                        {
                            Persistent.CreatedDate = DateTime.Now;
                            Persistent.CreatedUser = this.CurrentUser.ID;
                            datasource.Add(Persistent);
                        }

                        datasource.SaveChanges();
                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "Custom Page Url Already Exist");
                    }
                }

                return Result;

            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
            finally
            {
                this.ServiceController.Caching.Blog.Categories.DropCache();
            }
        }

        public ObjectResult<CustomPageTranslation> EditCustomPageTranslation(CustomPageTranslation CustomPageTranslation)
        {
            ObjectResult<CustomPageTranslation> Result = new ObjectResult<CustomPageTranslation>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICustomPageTranslationRepository>();

                #region Controls

                if (CustomPageTranslation.CustomPageID == 0)
                    Result.Fail("U2", "Blog Info Cannot Be Empty");
                if (!Result.HasFailed && CustomPageTranslation.LanguageID == 0)
                    Result.Fail("U2", "Language Info Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(CustomPageTranslation.TranslationHtml))
                    Result.Fail("U2", "Translation Content Cannot Be Empty");



                if (!Result.HasFailed &&
    datasource.GetQuery().Include(op => op.CustomPageContent)
        .Where(op => op.CustomPageContent.DomainID == this.Client.CurrentDomainID && op.ID != CustomPageTranslation.ID
        && op.CustomPageID == CustomPageTranslation.CustomPageID
                && op.LanguageID == CustomPageTranslation.LanguageID).ToList().Count > 0)
                    Result.Fail("U2", "Custom Page Translation Already Exists. Please edit that information");

                #endregion



                if (!Result.HasFailed)
                {
                    CustomPageTranslation persistent = new CustomPageTranslation();
                    bool isNew = true;
                    if (datasource.GetQuery().Include(op => op.CustomPageContent)
                        .Where(op =>
                            op.CustomPageContent.DomainID == this.Client.CurrentDomainID &&
                            op.CustomPageID == CustomPageTranslation.CustomPageID &&
                            op.LanguageID == CustomPageTranslation.LanguageID)
                        .Any())
                    {
                        isNew = false;
                        persistent = datasource.GetQuery()
                            .Where(op =>
                                op.CustomPageID == CustomPageTranslation.CustomPageID &&
                                op.LanguageID == CustomPageTranslation.LanguageID).FirstOrDefault();
                    }

                    persistent.CustomPageID = CustomPageTranslation.CustomPageID;
                    persistent.LanguageID = CustomPageTranslation.LanguageID;
                    persistent.TranslationHtml = CustomPageTranslation.TranslationHtml;
                    persistent.TranslationTitle = CustomPageTranslation.TranslationTitle;

                    if (isNew)
                        datasource.Add(persistent);

                    datasource.SaveChanges();
                    Result.SetData(persistent);
                }
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages,
                    true);
                return Result;
            }
            finally
            {
                RepositoryFactory.Dispose();
            }
        }

        public ObjectResult<CustomPageSEOInformation> EditCustomPageSEOInformation(CustomPageSEOInformation CustomPageSEOInformation)
        {
            ObjectResult<CustomPageSEOInformation> Result = new ObjectResult<CustomPageSEOInformation>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICustomPageSEOInformationRepository>();

                #region Controls

                if (CustomPageSEOInformation.CustomPageID == 0)
                    Result.Fail("U2", "Blog Info Cannot Be Empty");
                if (!Result.HasFailed && CustomPageSEOInformation.LanguageID == 0)
                    Result.Fail("U2", "Language Info Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(CustomPageSEOInformation.Title))
                    Result.Fail("U2", "Title Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(CustomPageSEOInformation.Description))
                    Result.Fail("U2", "Description Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(CustomPageSEOInformation.Keyword))
                    Result.Fail("U2", "Keyword Cannot Be Empty");



                if (!Result.HasFailed && datasource.GetQuery().Include(op => op.CustomPageContent)
                        .Where(op => op.CustomPageContent.DomainID == this.Client.CurrentDomainID && op.ID != CustomPageSEOInformation.ID
                        && op.CustomPageID == CustomPageSEOInformation.CustomPageID
                        && op.LanguageID == CustomPageSEOInformation.LanguageID).ToList().Count > 0)
                    Result.Fail("U2", "Custom Page SEO Information Already Exists. Please edit that information");

                #endregion



                if (!Result.HasFailed)
                {
                    CustomPageSEOInformation persistent = new CustomPageSEOInformation();
                    bool isNew = true;
                    if (datasource.GetQuery().Include(op => op.CustomPageContent)
                        .Where(op =>
                            op.CustomPageContent.DomainID == this.Client.CurrentDomainID &&
                            op.CustomPageID == CustomPageSEOInformation.CustomPageID &&
                            op.LanguageID == CustomPageSEOInformation.LanguageID)
                        .Any())
                    {
                        isNew = false;
                        persistent = datasource.GetQuery()
                            .Where(op =>
                                op.CustomPageID == CustomPageSEOInformation.CustomPageID &&
                                op.LanguageID == CustomPageSEOInformation.LanguageID).FirstOrDefault();
                    }

                    persistent.CustomPageID = CustomPageSEOInformation.CustomPageID;
                    persistent.LanguageID = CustomPageSEOInformation.LanguageID;
                    persistent.Title = CustomPageSEOInformation.Title;
                    persistent.Description = CustomPageSEOInformation.Description;
                    persistent.Keyword = CustomPageSEOInformation.Keyword;

                    if (isNew)
                        datasource.Add(persistent);

                    datasource.SaveChanges();
                    Result.SetData(persistent);
                }
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages,
                    true);
                return Result;
            }
            finally
            {
                RepositoryFactory.Dispose();
            }
        }

        #endregion

        public CustomPageFacade(Client client) : base(client)
        {

        }
    }
}