using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using BlogApplication.Connection;
using BlogApplication.Data.Visa;
using BlogApplication.Entity.Visa;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Data.GlobalTypes;
using BlogApplication.Framework.Application.Web;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.FunctionHelper;
using BlogApplication.Framework.Extensions.ListExtensions;
using BlogApplication.Framework.FileHelper;
using BlogApplication.Framework.Utility;
using UserType = BlogApplication.Data.GlobalTypes.UserType;

namespace BlogApplication.BusinessLayer.Controller.Visa
{
    public class VisaFacade : Facade
    {
        #region Visa

        public CollectionResult<User> GetUsers(int page = 1, int pageSize = 25)
        {
            CollectionResult<User> Result = new CollectionResult<User>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                var Persistent = new List<User>();
                Persistent = datasource.GetQuery().Where(op => op.DomainID == this.Client.CurrentDomainID &&
                op.StatusID != VariableValue.DeletedStatusID).ToList();

                Result.SetData(Persistent.Count, Persistent.Paginate(page, pageSize).ToList());
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

        public ObjectResult<User> GetUser(long ID)
        {
            ObjectResult<User> Result = new ObjectResult<User>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                var Persistent = new User();
                Persistent = datasource.GetQuery().Where(op => op.StatusID != VariableValue.DeletedStatusID && op.ID == ID).FirstOrDefault();
                Result.SetData(Persistent);
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

        public ObjectResult<User> GetUser(string Email)
        {
            ObjectResult<User> Result = new ObjectResult<User>();
            try
            {
                var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                var Persistent = new User();
                Persistent = datasource.GetQuery()
                    .Where(op => op.Email.Equals(Email) && op.StatusID != VariableValue.DeletedStatusID).FirstOrDefault();
                if (Persistent == null)
                    Result.Fail("U2", "User does not exist.");
                else
                    Result.SetData(Persistent);
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

        public ObjectResult<User> EditUser(User user)
        {
            ObjectResult<User> Result = new ObjectResult<User>();

            #region Controls

            if (user.StatusID != VariableValue.DeletedStatusID)
            {
                if (user == null)
                    Result.Fail("U2", "User Cannot Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(user.FirstName))
                    Result.Fail("U2", "First Name Cannot Be Empty");
                if (!Result.HasFailed && string.IsNullOrEmpty(user.LastName))
                    Result.Fail("U2", "Last Name Cannot Be Empty");
                if (user.ID == 0)
                {
                    if (!Result.HasFailed && string.IsNullOrEmpty(user.Username))
                        Result.Fail("U2", "Username Cannot Be Empty");
                    if (!Result.HasFailed && !StringHelper.checkFormat(user.Username, new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$")))
                        Result.Fail("U2", "Username must only include alpha-numeric characters");
                    if (!Result.HasFailed && string.IsNullOrEmpty(user.Email))
                        Result.Fail("U2", "Email Cannot Be Empty");
                    if (!Result.HasFailed &&
                        !StringHelper.checkFormat(user.Email,
                            new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")))
                        Result.Fail("U2", "Email is not in correct format");
                }

            }

            #endregion

            try
            {
                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                    if (
                        !datasource.GetQuery()
                            .Where(op => op.StatusID != VariableValue.DeletedStatusID && op.ID != user.ID && (op.Email.Equals(user.Email) || op.Username.Equals(user.Username)))
                            .Any())
                    {
                        var Persistent = new User();

                        if (user.ID > 0)
                            Persistent = datasource.GetQuery().Where(op => op.ID == user.ID && op.StatusID != VariableValue.DeletedStatusID)
                                    .FirstOrDefault();

                        if (user.StatusID != VariableValue.DeletedStatusID)
                        {
                            Persistent.FirstName = user.FirstName;
                            Persistent.LastName = user.LastName;
                            Persistent.MainLanguageID = user.MainLanguageID;
                            Persistent.UserProfilePic = user.UserProfilePic;
                            Persistent.UserType = user.UserType;
                            Persistent.Email = user.Email;
                            Persistent.Username = user.Username;
                            Persistent.PaswordSalt = RandomHelper.GenerateRandomPassword(15);
                            Persistent.Password = CryptoFactory.GetSHA512Hash(user.Password, Persistent.PaswordSalt);
                            Persistent.DomainID = this.Client.CurrentDomainID;

                        }

                        Persistent.StatusID = user.StatusID;

                        if (user.ID == 0)
                            datasource.Add(Persistent);


                        datasource.SaveChanges();
                        Result.SetData(Persistent);
                    }
                    else
                    {
                        Result.Fail("U2", "User  already exists");
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
                RepositoryFactory.Dispose();
            }
        }

        public ObjectResult<User> EditProfile(User oldUser, User newUser)
        {
            ObjectResult<User> Result = new ObjectResult<User>();

            #region Controls


            if (newUser == null)
                Result.Fail("U2", "User Cannot Empty");
            if (!Result.HasFailed && string.IsNullOrEmpty(newUser.FirstName))
                Result.Fail("U2", "First Name Cannot Be Empty");
            if (!Result.HasFailed && string.IsNullOrEmpty(newUser.LastName))
                Result.Fail("U2", "Last Name Cannot Be Empty");


            #endregion

            try
            {
                if (!Result.HasFailed)
                {
                    var datasource = RepositoryFactory.Current.GetRepository<IUserRepository>();
                    var Persistent = new User();

                    if (oldUser.ID > 0)
                        Persistent = datasource.GetQuery().Where(op => op.ID == oldUser.ID && op.DomainID == this.Client.CurrentDomainID && op.StatusID != VariableValue.DeletedStatusID)
                                .FirstOrDefault();

                    Persistent.FirstName = newUser.FirstName;
                    Persistent.LastName = newUser.LastName;
                    if (!string.IsNullOrEmpty(newUser.FileName))
                    {
                        var picResult = FileUploader.FTPUploadImageFile(this.CurrentDomain.CDNUrl, "User",
                                        this.CurrentDomain.FTPUsername, this.CurrentDomain.FTPPassword, newUser.UploadedLogoData, newUser.FileName, ImageOptions.CategoryOpt);
                        if (picResult.HasFailed)
                        {
                            Result.Fail(picResult.Messages);
                            return Result;
                        }
                        Persistent.UserProfilePic = picResult.Data;
                    }
                    Persistent.MainLanguageID = newUser.MainLanguageID;

                    datasource.SaveChanges();
                    Result.SetData(Persistent);
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
                RepositoryFactory.Dispose();
            }
        }



        #endregion

        #region Account 

        public ObjectResult<User> Login(string username, string password)
        {
            ObjectResult<User> Result = new ObjectResult<User>();
            try
            {
                User persistent = new User();
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    persistent = RepositoryFactory.Current.GetRepository<IUserRepository>().
                        GetQuery()
                        .Where(op => op.Username.Equals(username) && op.StatusID == VariableValue.ActiveStatusID)
                        .FirstOrDefault();
                    if (persistent != null)
                    {
                        if (persistent.Password.Equals(CryptoFactory.GetSHA512Hash(password, persistent.PaswordSalt)))
                        {
                            Result.SetData(persistent);
                        }
                        else
                        {
                            Result.Fail("U2", "Password Is Not Correct");
                        }
                    }
                    else
                    {
                        Result.Fail("U2", "User does not exist");
                    }
                }
                else
                {
                    Result.Fail("U2", "Missing Information");
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
                RepositoryFactory.Dispose();
            }
        }

        #endregion

        public VisaFacade(ControllerFacade facade) : base(new Client())
        {

        }
    }
}