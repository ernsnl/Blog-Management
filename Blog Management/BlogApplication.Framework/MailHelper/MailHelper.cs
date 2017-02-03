using System;
using System.Net.Mail;
using System.Text;
using BlogApplication.Framework.Cryptography;
using BlogApplication.Framework.ResultHelper;

namespace BlogApplication.Framework.MailHelper
{
    public static class MailHelper
    {
        public static ObjectResult<bool> SendEmail(string fromEmail, string toEmail, string password,
            string subject, string body, string smtpPort,
            string smtpHost, bool enableSSL = false,  int timeout = 10000)
        {
            ObjectResult<bool> Result = new ObjectResult<bool>();
            try
            {
                SmtpClient stmpClient = new SmtpClient();
                stmpClient.Port = Convert.ToInt32(smtpPort);
                stmpClient.Host = smtpHost;
                stmpClient.EnableSsl = enableSSL;
                stmpClient.Timeout = timeout;
                stmpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                stmpClient.UseDefaultCredentials = false;
                stmpClient.Credentials = new System.Net.NetworkCredential(fromEmail, CryptoFactory.Decrypt(password));

                MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                stmpClient.Send(mailMessage);

                Result.SetData(true);
                return Result;
            }
            catch (Exception ex)
            {
                Result.Fail(ex);
                return Result;
            }
            
        }
    }
}