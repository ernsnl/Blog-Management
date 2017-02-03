using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
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
using BlogApplication.Framework.FileHelper;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.BusinessLayer.Controller.Blog
{
    public class BlogFacade : BusinessLayer.Facade
    {

        #region BlogContent

        public CollectionResult<BlogContent> GetBlogContentList(long categoryID = 0, long[] tagIDs = null,
            bool getOnlyPublished = false, bool getOnlyNotPublished = false, bool getOnlyDenied = false, int page = 1, int pageSize = 25)
        {
            CollectionResult<BlogContent> Result = new CollectionResult<BlogContent>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IBlogContentRepository>();

                List<BlogContent> persistent = new List<BlogContent>();
                persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID &&
                op.StatusID != VariableValue.DeletedStatusID)
                .Include(op => op.BlogTranslations)
                .Include(op => op.Tags)
                .OrderByDescending(op => op.UpdatedDate).ToList();

                if (categoryID > 0)
                    persistent = persistent.Where(op => op.CategoryID == categoryID).ToList();
                if (tagIDs != null)
                    persistent = persistent.Where(op => op.Tags.Any(op2 => tagIDs.Contains(op2.ID))).ToList();
                if (getOnlyNotPublished)
                    persistent = persistent.Where(op => op.BlogStatus == VariableValue.WaitingReview).ToList();
                if (getOnlyDenied)
                    persistent = persistent.Where(op => op.BlogStatus == VariableValue.Denied).ToList();
                if (getOnlyPublished)
                    persistent = persistent.Where(op => op.BlogStatus == VariableValue.PublishedStatus).ToList();


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

        public ObjectResult<BlogContent> GetBlogContent(long ID)
        {
            ObjectResult<BlogContent> Result = new ObjectResult<BlogContent>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IBlogContentRepository>();
                if (datasource.GetQuery().Where(
                        op => op.DomainID == this.Client.CurrentDomainID
                        && op.ID == ID
                        && VariableValue.DeletedStatusID != op.StatusID)
                        .Any())
                {
                    BlogContent persistent = new BlogContent();
                    persistent = datasource.GetQuery()
                            .Where(op => op.StatusID != VariableValue.DeletedStatusID && op.ID == ID)
                            .Include(op => op.Tags).Include(op => op.BlogSEOInformations)
                            .Include(op => op.BlogTranslations).Include(op => op.BlogReviews)
                            .FirstOrDefault();
                    Result.SetData(persistent);
                }
                else
                {
                    Result.Fail("U2", "BLog Content Does Not Exist");
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

        public ObjectResult<BlogContent> EditBlogContent(BlogContent BlogContent)
        {
            ObjectResult<BlogContent> Result = new ObjectResult<BlogContent>();
            try
            {
                if (VariableValue.DeletedStatusID != BlogContent.StatusID)
                {
                    if (BlogContent == null)
                        Result.Fail("U2", "Category Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(BlogContent.Name))
                        Result.Fail("U2", "Blog Title Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(BlogContent.BlogCoverImgName) && BlogContent.ID == 0)
                        Result.Fail("U2", "Blog Cover Image Name Cannot Be Empty");
                    if (!Result.HasFailed && BlogContent.CoverImgData == null && BlogContent.ID == 0)
                        Result.Fail("U2", "Blog Cover Image Data Cannot Be Empty");
                    if (!Result.HasFailed && BlogContent.CategoryID == 0)
                        Result.Fail("U2", "Blog Has To Be Assigned To A Category");

                }

                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<IBlogContentRepository>();
                    if (!datasource.GetQuery().Where(
                            op => op.DomainID == this.Client.CurrentDomainID && op.ID != BlogContent.ID && op.Name.Equals(BlogContent.Name) && VariableValue.DeletedStatusID != BlogContent.StatusID).Any())
                    {
                        BlogContent Persistent = new BlogContent();
                        if (BlogContent.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID && op.ID == BlogContent.ID).Include(op => op.Tags)
                                    .FirstOrDefault();

                        if (VariableValue.DeletedStatusID != BlogContent.StatusID)
                        {
                            Persistent.Name = BlogContent.Name;
                            Persistent.DomainID = this.Client.CurrentDomainID;
                            if (!string.IsNullOrEmpty(BlogContent.BlogCoverImgName))
                            {
                                if (BlogContent.CoverImgData != null)
                                {
                                    var uploadResult = FileUploader.FTPUploadImageFile(this.CurrentDomain.CDNUrl, "Blog",
                                        this.CurrentDomain.FTPUsername, this.CurrentDomain.FTPPassword, BlogContent.CoverImgData,
                                        BlogContent.BlogCoverImgName, ImageOptions.BlogOpt);
                                    if (uploadResult.HasFailed)
                                    {
                                        Result.Fail(uploadResult.Messages);
                                        return Result;
                                    }
                                    Persistent.CoverImgName = uploadResult.Data;
                                }
                            }
                            Persistent.HtmlContent = BlogContent.HtmlContent;
                            Persistent.BlogAbstract = BlogContent.BlogAbstract;
                            Persistent.CategoryID = BlogContent.CategoryID;
                            Persistent.BlogStatus = BlogContent.BlogStatus;
                            Persistent.PublishDate = BlogContent.PublishDate;

                        }
                        Persistent.StatusID = BlogContent.StatusID;
                        Persistent.UpdatedDate = DateTime.Now;
                        Persistent.UpdatedUser = this.CurrentUser.ID;

                        if (BlogContent.ID == 0)
                        {
                            Persistent.CreatedDate = DateTime.Now;
                            Persistent.CreatedUser = this.CurrentUser.ID;
                            datasource.Add(Persistent);
                        }

                        datasource.SaveChanges();

                        var tagDatasource = RepositoryFactory.Current.GetRepository<IBlogContentRepository>();
                        if (BlogContent.ID > 0)
                        {
                            if (BlogContent.Tags.Count == 0)
                            {
                                string sqlCommand =
                                    "Delete from [Blog].[BlogTag] where BlogID = " +
                                    BlogContent.ID;
                                tagDatasource.ExecuteNonQuery(sqlCommand);
                            }

                        }
                        if (!string.IsNullOrEmpty(BlogContent.TagIDs))
                        {
                            List<string> tagIDs = BlogContent.TagIDs.Split(',').ToList();
                            foreach (var tagID in tagIDs)
                            {
                                long TagID = Convert.ToInt64(tagID);
                                string tagCommand =
                                    "Insert into [Blog].[BlogTag] (BlogID, TagID) Values (" +
                                    Persistent.ID + "," + TagID + ")";
                                tagDatasource.ExecuteNonQuery(tagCommand);

                            }
                        }
                        tagDatasource.SaveChanges();

                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "Blog Title Already Exist");
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

        public ObjectResult<BlogTranslation> EditBlogContentTranslation(BlogTranslation BlogTranslation)
        {
            ObjectResult<BlogTranslation> Result = new ObjectResult<BlogTranslation>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IBlogTranslationRepository>();

                #region Controls

                if (BlogTranslation.BlogID == 0)
                    Result.Fail("U2", "Blog Info Cannot Be Empty");
                if (!Result.HasFailed && BlogTranslation.LanguageID == 0)
                    Result.Fail("U2", "Language Info Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogTranslation.AbstractTranslation))
                    Result.Fail("U2", "Translation Abstract Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogTranslation.TranslationTitle))
                    Result.Fail("U2", "Translation Title Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogTranslation.TranslationHtml))
                    Result.Fail("U2", "Translation Content Cannot Be Empty");



                if (!Result.HasFailed &&
    datasource.GetQuery().Include(op => op.BlogContent)
        .Where(op => op.ID != BlogTranslation.ID && op.BlogContent.DomainID == this.Client.CurrentDomainID && op.BlogID == BlogTranslation.BlogID && op.LanguageID == BlogTranslation.LanguageID).ToList().Count > 1)
                    Result.Fail("U2", "Blog Translation Already Exists. Please edit that information");

                #endregion



                if (!Result.HasFailed)
                {
                    BlogTranslation persistent = new BlogTranslation();
                    bool isNew = true;
                    if (datasource.GetQuery().Include(op => op.BlogContent)
                        .Where(op =>
                            op.BlogContent.DomainID == this.Client.CurrentDomainID &&
                            op.BlogID == BlogTranslation.BlogID &&
                            op.LanguageID == BlogTranslation.LanguageID)
                        .Any())
                    {
                        isNew = false;
                        persistent = datasource.GetQuery()
                            .Where(op =>
                                op.BlogID == BlogTranslation.BlogID &&
                                op.LanguageID == BlogTranslation.LanguageID).FirstOrDefault();
                    }

                    persistent.BlogID = BlogTranslation.BlogID;
                    persistent.LanguageID = BlogTranslation.LanguageID;
                    persistent.TranslationHtml = BlogTranslation.TranslationHtml;
                    persistent.AbstractTranslation = BlogTranslation.AbstractTranslation;
                    persistent.TranslationTitle = BlogTranslation.TranslationTitle;

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

        public ObjectResult<BlogSEOInformation> EditBlogSEOInformation(BlogSEOInformation BlogSEOInformation)
        {
            ObjectResult<BlogSEOInformation> Result = new ObjectResult<BlogSEOInformation>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IBlogSEOInformationRepository>();

                #region Controls

                if (BlogSEOInformation.BlogID == 0)
                    Result.Fail("U2", "Blog Info Cannot Be Empty");
                if (!Result.HasFailed && BlogSEOInformation.LanguageID == 0)
                    Result.Fail("U2", "Language Info Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogSEOInformation.Title))
                    Result.Fail("U2", "Title Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogSEOInformation.Description))
                    Result.Fail("U2", "Description Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogSEOInformation.Keyword))
                    Result.Fail("U2", "Keyword Cannot Be Empty");



                if (!Result.HasFailed && datasource.GetQuery().Include(op => op.BlogContent)
                        .Where(op => op.BlogContent.DomainID == this.Client.CurrentDomainID && op.ID != BlogSEOInformation.ID
                        && op.BlogID == BlogSEOInformation.BlogID && op.LanguageID == BlogSEOInformation.LanguageID).ToList().Count > 0)
                    Result.Fail("U2", "Blog Translation Already Exists. Please edit that information");

                #endregion



                if (!Result.HasFailed)
                {
                    BlogSEOInformation persistent = new BlogSEOInformation();
                    bool isNew = true;
                    if (datasource.GetQuery().Include(op => op.BlogContent)
                        .Where(op =>
                            op.BlogContent.DomainID == this.Client.CurrentDomainID &&
                            op.BlogID == BlogSEOInformation.BlogID &&
                            op.LanguageID == BlogSEOInformation.LanguageID)
                        .Any())
                    {
                        isNew = false;
                        persistent = datasource.GetQuery()
                            .Where(op =>
                                op.BlogID == BlogSEOInformation.BlogID &&
                                op.LanguageID == BlogSEOInformation.LanguageID).FirstOrDefault();
                    }

                    persistent.BlogID = BlogSEOInformation.BlogID;
                    persistent.LanguageID = BlogSEOInformation.LanguageID;
                    persistent.Title = BlogSEOInformation.Title;
                    persistent.Description = BlogSEOInformation.Description;
                    persistent.Keyword = BlogSEOInformation.Keyword;

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

        public ObjectResult<BlogReview> WriteReview(BlogReview BlogReview)
        {
            ObjectResult<BlogReview> Result = new ObjectResult<BlogReview>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IBlogReviewRepository>();

                #region Controls

                if (BlogReview.BlogID == 0)
                    Result.Fail("U2", "Blog Info Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(BlogReview.ReviewMessage))
                    Result.Fail("U2", "Message Cannot Be Empty");

                #endregion



                if (!Result.HasFailed)
                {
                    BlogReview persistent = new BlogReview();
                    persistent.BlogID = BlogReview.BlogID;
                    persistent.ReviewMessage = BlogReview.ReviewMessage;
                    persistent.CreatedUser = this.CurrentUser.ID;
                    persistent.CreatedDate = DateTime.Now;

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

        public BlogFacade(Client client) : base(client)
        {
        }
    }
}