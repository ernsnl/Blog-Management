using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BlogApplication.Connection;
using BlogApplication.Data.Blog;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Entity.Blog;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.BusinessLayer.Controller.Blog
{
    public class TagFacade : BusinessLayer.Facade
    {
        #region Tag

        public CollectionResult<Tag> GetTags(string name = "", int page = 1, int pageSize = 25)
        {
            CollectionResult<Tag> Result = new CollectionResult<Tag>();
            try
            {
                List<Tag> persistent = this.ServiceController.Caching.Blog.Tags.List.Where(op => op.DomainID == this.Client.CurrentDomainID).ToList();
                if (!string.IsNullOrEmpty(name))
                    persistent = persistent.Where(op => op.DomainID == this.Client.CurrentDomainID && op.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();
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

            }
        }

        public ObjectResult<Tag> GetTag(long ID)
        {
            ObjectResult<Tag> Result = new ObjectResult<Tag>();
            try
            {
                if (this.ServiceController.Caching.Blog.Tags.List.Where(
                        op => op.DomainID == this.Client.CurrentDomainID && op.ID == ID && op.StatusID != VariableValue.DeletedStatusID).Any())
                {
                    Result.SetData(this.ServiceController.Caching.Blog.Tags.List.Where(op => op.ID == ID && op.StatusID != VariableValue.DeletedStatusID).FirstOrDefault());
                }
                else
                {
                    Result.Fail("U2", "Tag does not exist");
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

            }
        }

        public ObjectResult<Tag> EditTag(Tag nTag)
        {
            ObjectResult<Tag> Result = new ObjectResult<Tag>();
            try
            {
                #region Controls

                if (nTag.StatusID != VariableValue.DeletedStatusID)
                {
                    if (nTag == null)
                        Result.Fail("U2", "Tag Cannot Be Empty");
                    if (!Result.HasFailed && string.IsNullOrEmpty(nTag.Name))
                        Result.Fail("U2", " Name Cannot Be Empty");
                }

                #endregion

                var datasource = RepositoryFactory.Current.GetRepository<ITagRepository>();
                if (
                    !datasource.GetQuery()
                        .Where(
                            op => op.DomainID == this.Client.CurrentDomainID && op.ID != nTag.ID && op.StatusID != VariableValue.DeletedStatusID &&
                                op.Name.ToLower().Equals(nTag.Name.ToLower()))
                        .Any())
                {
                    Tag persistent = new Tag();
                    if (nTag.ID > 0)
                        persistent = datasource.GetQuery().Where(op => op.ID == nTag.ID).FirstOrDefault();

                    if (VariableValue.DeletedStatusID != nTag.StatusID)
                    {
                        persistent.Name = nTag.Name;
                        persistent.DomainID = this.Client.CurrentDomainID;
                    }
                    persistent.StatusID = nTag.StatusID;

                    if (nTag.ID == 0)
                        datasource.Add(persistent);
                    datasource.SaveChanges();

                    Result.SetData(persistent);
                    return Result;
                }
                else
                {
                    Result.Fail("U2", "Tag Already Exist");
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

        #endregion


        public TagFacade(Client client) : base(client)
        {

        }
    }
}