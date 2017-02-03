using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
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

namespace BlogApplication.BusinessLayer.Controller.Blog
{
    public class CategoryFacade : BusinessLayer.Facade
    {

        #region Category

        public CollectionResult<Category> GetCategoryList(int page = 1, int pageSize = 25)
        {
            CollectionResult<Category> Result = new CollectionResult<Category>();
            try
            {
                List<Category> persistent = this.ServiceController.Caching.Blog.Categories.List.Where(op => op.DomainID == this.Client.CurrentDomainID && op.StatusID != VariableValue.DeletedStatusID).ToList();
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

        public ObjectResult<Category> GetCategory(long ID)
        {
            ObjectResult<Category> Result = new ObjectResult<Category>();
            try
            {
                if (this.ServiceController.Caching.Blog.Categories.List.Where(
                        op => op.DomainID == this.Client.CurrentDomainID && op.ID == ID && VariableValue.DeletedStatusID != op.StatusID).Any())
                {
                    Category persistent = new Category();
                    persistent =
                        this.ServiceController.Caching.Blog.Categories.List.Where(op => op.ID == ID).FirstOrDefault();
                    Result.SetData(persistent);
                }
                else
                {
                    Result.Fail("U2", "Category Does Not Exist");
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

        public ObjectResult<Category> EditCategory(Category Category)
        {
            ObjectResult<Category> Result = new ObjectResult<Category>();
            try
            {
                if (VariableValue.DeletedStatusID != Category.StatusID)
                {
                    if (Category == null)
                        Result.Fail("U2", "Category Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(Category.Name))
                        Result.Fail("U2", "Category Name Cannot Be Empty");

                }

                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<ICategoryRepository>();
                    if (!datasource.GetQuery().Where(
                            op => op.DomainID == this.Client.CurrentDomainID && op.ID != Category.ID && op.Name.Equals(Category.Name) && VariableValue.DeletedStatusID != op.StatusID).Any())
                    {
                        Category Persistent = new Category();
                        if (Category.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID && op.ID == Category.ID)
                                    .FirstOrDefault();

                        if (VariableValue.DeletedStatusID != Category.StatusID)
                        {
                            Persistent.Name = Category.Name;
                            Persistent.DomainID = this.Client.CurrentDomainID;
                            if (!string.IsNullOrEmpty(Category.Filename))
                            {
                                if (Category.FileData != null)
                                {
                                    var uploadResult = FileUploader.FTPUploadImageFile(this.CurrentDomain.CDNUrl, "Category",
                                        this.CurrentDomain.FTPUsername, this.CurrentDomain.FTPPassword, Category.FileData,
                                        Category.Filename, ImageOptions.CategoryOpt);
                                    if (uploadResult.HasFailed)
                                    {
                                        Result.Fail(uploadResult.Messages);
                                        return Result;
                                    }
                                    Persistent.ImgName = uploadResult.Data;
                                }
                            }
                        }
                        Persistent.StatusID = Category.StatusID;

                        if (Category.ID == 0)
                            datasource.Add(Persistent);

                        datasource.SaveChanges();
                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "Category Name Already Exist");
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

        public ObjectResult<CategoryTranslation> EditCategoryTranslation(CategoryTranslation CategoryTranslation)
        {
            ObjectResult<CategoryTranslation> Result = new ObjectResult<CategoryTranslation>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<ICategoryTranslationRepository>();

                if (CategoryTranslation.CategoryID == 0)
                    Result.Fail("U2", "Category Info Cannot Be Empty");
                if (!Result.HasFailed && CategoryTranslation.LanguageID == 0)
                    Result.Fail("U2", "Language Info Cannot Be Empty");
                if (!Result.HasFailed &&
                    datasource.GetQuery().Include(op => op.Category)
                        .Where(op => op.Category.DomainID == this.Client.CurrentDomainID && op.CategoryID == CategoryTranslation.CategoryID && op.LanguageID == op.LanguageID)
                        .Any())
                    Result.Fail("U2", "Category Translation Already Exists. Please edit that information");

                CategoryTranslation persistent = new CategoryTranslation();
                bool isNew = true;
                if (datasource.GetQuery().Include(op => op.Category)
                    .Where(op =>
                    op.Category.DomainID == this.Client.CurrentDomainID &&
                        op.CategoryID == CategoryTranslation.CategoryID &&
                        op.LanguageID == CategoryTranslation.LanguageID)
                    .Any())
                {
                    isNew = false;
                    persistent = datasource.GetQuery()
                        .Where(op =>
                            op.CategoryID == CategoryTranslation.CategoryID &&
                            op.LanguageID == CategoryTranslation.LanguageID).FirstOrDefault();
                }

                persistent.CategoryID = CategoryTranslation.CategoryID;
                persistent.LanguageID = CategoryTranslation.LanguageID;
                persistent.Translation = CategoryTranslation.Translation;

                if (isNew)
                    datasource.Add(persistent);

                datasource.SaveChanges();
                Result.SetData(persistent);
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
                var datasource = RepositoryFactory.Current.GetRepository<ICategoryRepository>();
                var Persistent =
                    datasource.GetQuery()
                        .Where(op => op.ID == CategoryTranslation.CategoryID)
                        .Include(op => op.CategoryTranslations).FirstOrDefault();
                this.ServiceController.Caching.Blog.Categories.Update(Persistent);
            }
        }

        #endregion


        public CategoryFacade(Client client) : base(client)
        {

        }
    }
}