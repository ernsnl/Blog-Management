using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BlogApplication.Connection;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Data.MainSystem;
using BlogApplication.Data.Visa;
using BlogApplication.Entity.MainSystem;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FileHelper;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.BusinessLayer.Controller.MainSystem
{
    public class MainSystemFacade : Facade
    {
        public ObjectResult<Domain> EditDomain(Domain nDomain, bool mailServiceUse)
        {
            ObjectResult<Domain> Result = new ObjectResult<Domain>();
            try
            {
                #region Controls

                if (!Result.HasFailed && nDomain == null)
                    Result.Fail("U01", "DomainInfoCannotBeNull");
                if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.DomainName))
                    Result.Fail("U01", "DomainNameCannotBeEmpty");
                if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.DomainUrl))
                    Result.Fail("U01", "DomainUrlCannotBeEmpty");
                if (!Result.HasFailed && !StringHelper.validateURL(nDomain.DomainUrl))
                    Result.Fail("U01", "DomainUrlMustBeValid");
                if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.ConsoleDomainUrl))
                    Result.Fail("U01", "ConsoleDomainUrlCannotBeEmpty");
                if (!Result.HasFailed && !StringHelper.validateURL(nDomain.ConsoleDomainUrl))
                    Result.Fail("U01", "ConsoleDomainUrlMustBeValid");
                if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.Username))
                    Result.Fail("U01", "StmpEmailCannotBeEmpty");
                if (!Result.HasFailed && !StringHelper.checkFormat(nDomain.Username,
                            new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")))
                    Result.Fail("U01", "StmpEmailMustBeValid");
                if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.CDNUrl))
                    Result.Fail("U01", "CDNUrlCannotBeEmpty");
                if (!Result.HasFailed && !StringHelper.validateURL(nDomain.CDNUrl))
                    Result.Fail("U01", "CDNUrlMustBeValid");
                if (nDomain.ID == 0)
                {
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.AdminEmail))
                        Result.Fail("U01", "AdminEmailCannotBeEmpty");
                    if (!Result.HasFailed &&
                        !StringHelper.checkFormat(nDomain.AdminEmail,
                            new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")))
                        Result.Fail("U01", "AdminEmailIsNotCorrect");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.AdminUsername))
                        Result.Fail("U01", "AdminUserNameCannotBeEmpty");
                    if (!Result.HasFailed &&
                        !StringHelper.checkFormat(nDomain.AdminUsername, new Regex(@"^[a - zA - Z0 - 9] *$")))
                        Result.Fail("U01", "AdminUserNameMustBeAlphaNumeric");
                }
                if (!Result.HasFailed && nDomain.DefaultLanguage == 0)
                    Result.Fail("U01", "LanguageCannotBeEmpty");
                if (mailServiceUse)
                {
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.SmtpHost))
                        Result.Fail("U01", "SmtpHostAddressCannotBeEmpty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.SmtpPassword) && nDomain.ID == 0)
                        Result.Fail("U01", "SmtpPasswordCannotBeEmpty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.Username))
                        Result.Fail("U01", "SmtpUserNameCannotBeEmpty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nDomain.SmtpPort))
                        Result.Fail("U01", "SmtpPortCannotBeEmpty");
                    if (!Result.HasFailed && !(Convert.ToInt32(nDomain.SmtpPort) > 0))
                        Result.Fail("U01", "SmtpPortMustBeBiggerThanZero");
                }

                #endregion



                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<IDomainRepository>();

                    if (!datasource.GetQuery().Where(op => (op.DomainName.Equals(nDomain.DomainName)
                    || op.DomainUrl.Equals(nDomain.DomainUrl)) && op.ID != nDomain.ID && op.StatusID != VariableValue.DeletedStatusID).Any())
                    {
                        Domain persistent = new Domain();
                        if (nDomain.ID > 0)
                            persistent = datasource.GetQuery().Where(op => op.ID == nDomain.ID).FirstOrDefault();

                        if (nDomain.StatusID != VariableValue.DeletedStatusID)
                        {
                            persistent.DomainName = nDomain.DomainName;
                            persistent.DomainUrl = nDomain.DomainUrl;
                            persistent.ConsoleDomainUrl = nDomain.ConsoleDomainUrl;
                            persistent.DefaultLanguage = nDomain.DefaultLanguage;

                            if (nDomain.ID == 0)
                            {
                                persistent.AdminEmail = nDomain.AdminEmail;
                                persistent.AdminUsername = nDomain.AdminUsername;
                            }

                            if (!string.IsNullOrEmpty(nDomain.DomainBackgroundImgName))
                            {
                                var picResult = FileUploader.FTPUploadImageFile(this.CurrentDomain.CDNUrl, "MainSystem",
                                                this.CurrentDomain.FTPUsername, this.CurrentDomain.FTPPassword, nDomain.DomainBackgroundImgData, nDomain.DomainBackgroundImgName, ImageOptions.BackGroundOpt);
                                if (picResult.HasFailed)
                                {
                                    Result.Fail(picResult.Messages);
                                    return Result;
                                }
                                persistent.DomainImgName = picResult.Data;
                            }

                            if (!string.IsNullOrEmpty(nDomain.DomainFavIcoName))
                            {
                                var picResult = FileUploader.FTPUploadImageFile(this.CurrentDomain.CDNUrl, "MainSystem",
                                                this.CurrentDomain.FTPUsername, this.CurrentDomain.FTPPassword, nDomain.DomainFavIcoData, nDomain.DomainFavIcoName, ImageOptions.favIcon);
                                if (picResult.HasFailed)
                                {
                                    Result.Fail(picResult.Messages);
                                    return Result;
                                }
                                persistent.FavIcon = picResult.Data;
                            }

                            persistent.DisqusUserName = nDomain.DisqusUserName;
                            persistent.FacebookAppID = nDomain.FacebookAppID;
                            persistent.TwitterUserName = nDomain.TwitterUserName;
                            persistent.GoogleAnalyticsID = nDomain.GoogleAnalyticsID;

                            if (!string.IsNullOrEmpty(nDomain.SmtpPassword))
                                persistent.SmtpPassword = CryptoFactory.Encrypt(nDomain.SmtpPassword);
                            persistent.SmtpPort = nDomain.SmtpPort;
                            persistent.Username = nDomain.Username;
                            persistent.EnableSSL = nDomain.EnableSSL;
                            persistent.SmtpHost = nDomain.SmtpHost;

                            if (!string.IsNullOrEmpty(nDomain.FTPPassword))
                                persistent.FTPPassword = CryptoFactory.Encrypt(nDomain.FTPPassword);
                            persistent.CDNUrl = nDomain.CDNUrl;
                            persistent.FTPUsername = nDomain.FTPUsername;

                            persistent.CreatedDate = DateTime.Now;
                            persistent.CreatedUser = nDomain.CreatedUser;
                        }
                        persistent.UpdatedDate = DateTime.Now;
                        persistent.UpdatedUser = nDomain.UpdatedUser;
                        persistent.StatusID = nDomain.StatusID;
                        if (nDomain.ID == 0)
                            datasource.Add(persistent);
                        datasource.SaveChanges();

                        if (nDomain.ID == 0)
                        {
                            User nUser = new User();
                            nUser.Email = nDomain.AdminEmail;
                            nUser.Username = nDomain.AdminUsername;
                            nUser.FirstName = nDomain.DomainName;
                            nUser.LastName = "Admin";
                            nUser.UserType = Convert.ToByte(UserType.Admin);
                            nUser.Password = BlogApplication.Framework.Utility.RandomHelper.GenerateRandomPassword(10);
                            nUser.DomainID = persistent.ID;
                            nUser.StatusID = VariableValue.ActiveStatusID;
                            nUser.MainLanguageID = nDomain.DefaultLanguage;
                            var userResult = this.ServiceController.Visa.EditUser(nUser);
                            if (userResult.HasFailed)
                            {
                                Result.Fail(userResult.Messages);
                                return Result;
                            }
                        }


                        Result.SetData(persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "DomainIsInUser");
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
        }

        public ObjectResult<Domain> GetDomain(long ID)
        {
            ObjectResult<Domain> Result = new ObjectResult<Domain>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IDomainRepository>();
                Domain persistent = new Domain();
                persistent =
                    datasource.GetQuery()
                        .Where(op => op.ID == ID && op.StatusID == VariableValue.ActiveStatusID)
                        .Include(op => op.SEOInformations)
                        .Include(op => op.NavigationEditors)
                        .Include(op => op.DomainSocials)
                        .FirstOrDefault();
                Result.SetData(persistent);
                return Result;

            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }

        }

        public CollectionResult<Domain> GetDomains(int page = 1, int pageSize = 25)
        {
            CollectionResult<Domain> Result = new CollectionResult<Domain>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IDomainRepository>();
                List<Domain> persistent = new List<Domain>();
                persistent =
                    datasource.GetQuery()
                        .Where(op => op.StatusID != VariableValue.DeletedStatusID)
                        .ToList();
                Result.SetData(persistent.Count, persistent.Paginate(1, 25).ToList());
                return Result;

            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }

        }

        public ObjectResult<SEOInformation> EditSEOInformation(SEOInformation SEOInformation)
        {
            ObjectResult<SEOInformation> Result = new ObjectResult<SEOInformation>();

            #region Controls 

            if (SEOInformation == null)
                Result.Fail("U2", "Information Cannot Be Empty");
            if (!Result.HasFailed && string.IsNullOrEmpty(SEOInformation.Title))
                Result.Fail("U2", "Title Cannot Be Empty");
            if (!Result.HasFailed && string.IsNullOrEmpty(SEOInformation.Description))
                Result.Fail("U2", "Description Cannot Be Empty");
            if (!Result.HasFailed && string.IsNullOrEmpty(SEOInformation.Keyword))
                Result.Fail("U2", "Keyword Cannot Be Empty");
            if (!Result.HasFailed && SEOInformation.LanguageID == 0)
                Result.Fail("U2", "Language Cannot Be Empty");
            if (!Result.HasFailed && SEOInformation.LocationID == 0)
                Result.Fail("U2", "Location Cannot Be Empty");

            #endregion

            try
            {
                if (!Result.HasFailed)
                {

                    var datasource = RepositoryFactory.Current.GetRepository<ISEOInformationRepository>();
                    if (
                        !datasource.GetQuery()
                            .Where(
                                op =>
                                    op.LocationID == SEOInformation.LocationID &&
                                    op.LanguageID == SEOInformation.LanguageID && op.ID != SEOInformation.ID &&
                                    op.DomainID == this.CurrentDomain.ID)
                            .Any())
                    {

                        SEOInformation Persistent = new SEOInformation();

                        if (SEOInformation.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.ID == SEOInformation.ID).FirstOrDefault();

                        Persistent.Description = SEOInformation.Description;
                        Persistent.Title = SEOInformation.Title;
                        Persistent.Keyword = SEOInformation.Keyword;
                        Persistent.DomainID = SEOInformation.DomainID;
                        Persistent.LanguageID = SEOInformation.LanguageID;
                        Persistent.LocationID = SEOInformation.LocationID;
                        Persistent.StatusID = VariableValue.ActiveStatusID;

                        if (SEOInformation.ID == 0)
                            datasource.Add(Persistent);

                        datasource.SaveChanges();

                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "SEO Information is already exist.");
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
        }

        public ObjectResult<NavigationEditor> GetNavigationEditor(int typeID, long DomainID)
        {
            ObjectResult<NavigationEditor> Result = new ObjectResult<NavigationEditor>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<INavigationEditorRepository>();
                NavigationEditor persistent = new NavigationEditor();
                if (datasource.GetQuery().Where(op => op.DomainID == DomainID && op.EditorLocation == typeID).Any())
                {
                    persistent =
                        datasource.GetQuery()
                            .Where(op => op.DomainID == DomainID && op.EditorLocation == typeID)
                            .FirstOrDefault();
                    Result.SetData(persistent);
                }
                else
                {
                    persistent.DomainID = DomainID;
                    persistent.EditorLocation = Convert.ToByte(typeID);
                    persistent.ID = 0;
                    Result.SetData(persistent);
                }
                return Result;

            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                this.ServiceController.Log.SendLog(FunctionHelper.getFunctionInfo(new StackTrace()), Result.Messages, true);
                return Result;
            }
        }

        public ObjectResult<NavigationEditor> EditNavigationEditor(NavigationEditor NavigationEditor)
        {
            ObjectResult<NavigationEditor> Result = new ObjectResult<NavigationEditor>();
            try
            {

                var datasource = RepositoryFactory.Current.GetRepository<INavigationEditorRepository>();
                if (!datasource.GetQuery()
                        .Where(op => op.DomainID == NavigationEditor.DomainID && op.ID != NavigationEditor.ID && op.EditorLocation == NavigationEditor.EditorLocation)
                        .Any())
                {
                    NavigationEditor persistent = new NavigationEditor();
                    if (NavigationEditor.ID > 0)
                        persistent = datasource.GetQuery().Where(op => op.ID == NavigationEditor.ID).FirstOrDefault();


                    persistent.DomainID = NavigationEditor.DomainID;
                    persistent.EditorLocation = NavigationEditor.EditorLocation;
                    persistent.EditorData = NavigationEditor.EditorData;

                    if (NavigationEditor.ID == 0)
                        datasource.Add(persistent);

                    datasource.SaveChanges();

                    Result.SetData(persistent);
                    return Result;
                }
                else
                {
                    Result.Fail("U2", "Navigation Editor Already Exists");
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
                this.ServiceController.Caching.Blog.Tags.DropCache();
                RepositoryFactory.Dispose();
            }
        }

        public ObjectResult<DomainSocial> EditDomainSocial(DomainSocial DomainSocial)
        {
            ObjectResult<DomainSocial> Result = new ObjectResult<DomainSocial>();
            try
            {
                if (VariableValue.DeletedStatusID != DomainSocial.StatusID)
                {
                    if (DomainSocial == null)
                        Result.Fail("U2", "Category Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(DomainSocial.SocialMediaUrl))
                        Result.Fail("U2", "Social URL Name Cannot Be Empty");
                    if(!Result.HasFailed && !StringHelper.validateURL(DomainSocial.SocialMediaUrl))
                        Result.Fail("U2", "Social URL must be valid");

                }

                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<IDomainSocialRepository>();
                    if (!datasource.GetQuery().Where(
                            op => op.DomainID == this.Client.CurrentDomainID 
                            && op.ID != DomainSocial.ID 
                            && op.SocialMediaType == DomainSocial.SocialMediaType
                            && VariableValue.DeletedStatusID != DomainSocial.StatusID).Any())
                    {
                        DomainSocial Persistent = new DomainSocial();
                        if (DomainSocial.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID && op.ID == DomainSocial.ID)
                                    .FirstOrDefault();

                        if (VariableValue.DeletedStatusID != DomainSocial.StatusID)
                        {
                            Persistent.SocialMediaType = DomainSocial.SocialMediaType;
                            Persistent.SocialMediaUrl = DomainSocial.SocialMediaUrl;
                            Persistent.SocialMediaPitch = DomainSocial.SocialMediaPitch;
                            Persistent.DomainID = DomainSocial.DomainID;                           
                        }
                        Persistent.StatusID = DomainSocial.StatusID;

                        if (DomainSocial.ID == 0)
                            datasource.Add(Persistent);

                        datasource.SaveChanges();
                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "Social Already Exist");
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

        public MainSystemFacade(ControllerFacade facade) : base(new Client())
        {

        }
    }
}