using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.FTPHelper;
using BlogApplication.Framework.ResultHelper;
using BlogApplication.Framework.Utility;

namespace BlogApplication.Framework.FileHelper
{
    public static class FileUploader
    {

        public static ObjectResult<string> FTPUploadImageFile(
            string ftpURL, string directoryName, string username, string password,
            byte[] Data, string fileName, Dictionary<string, ImageOptions.widthHeight> options, bool dontResize = false)
        {
            ObjectResult<string> Result = new ObjectResult<string>();
            try
            {
                if (Data != null)
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string[] acceptedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif", ".ico" };
                        var extension = Path.GetExtension(fileName).ToLower();
                        if (acceptedExtensions.Contains(extension))
                        {
                            var FormatFileName =
                                StringHelper.addRandomToEnd(fileName.Replace(extension.ToLower(), ""));
                            foreach (var option in options)
                            {

                                FtpWebRequest req =
                               (FtpWebRequest)
                                   WebRequest.Create(ftpURL.Replace("http", "ftp") + "/" + FTPProperties.InitailDirectory + "/" + directoryName + "/" +
                                                     FormatFileName + "_" + option.Key + extension);
                                req.UseBinary = true;
                                req.Method = WebRequestMethods.Ftp.UploadFile;
                                req.Credentials = new NetworkCredential(username, CryptoFactory.Decrypt(password));

                                var imageStream = new MemoryStream();
                                if (!dontResize)
                                    ImageCrop.CropImage(Data, option.Value.width, option.Value.width).Save(imageStream, ImageFormat.Png);
                                else
                                    ResizeImage.ResizeImageFromByte(Data, option.Value.width, option.Value.width).Save(imageStream, ImageFormat.Png);

                                Stream streamObj = new MemoryStream(imageStream.ToArray());

                                byte[] buffer = new byte[imageStream.Length];
                                streamObj.Read(buffer, 0, buffer.Length);
                                req.ContentLength = imageStream.Length;
                                Stream reqStream = req.GetRequestStream();
                                reqStream.Write(buffer, 0, buffer.Length);

                                reqStream.Close();
                                streamObj.Close();
                            }

                            Result.SetData(FormatFileName + extension);
                        }
                        else
                        {
                            Result.Fail("U2", "Allowed extensions are .png, .jpeg, .jpg or .gif");
                        }
                    }
                    else
                    {
                        Result.Fail("U2", "FileName cannot be empty");
                    }

                }
                else
                {
                    Result.Fail("U2", "Data Is Empty");
                }
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                return Result;
            }
            finally
            {
            }
        }
    }
}
