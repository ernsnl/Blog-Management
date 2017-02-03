using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using BlogApplication.Framework.PleskAPI.Connection;

namespace BlogApplication.Framework.PleskAPI.RequestHandler
{
    public static class Request
    {
        private static PleskConnectionInfo connectionInfo
        {
            get { return new PleskConnectionInfo(); }
        }

        private static ValidationEventHandler XmlSchemaValidation = null;

        private static XmlDocument uploadFile(string uploadFile)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(connectionInfo.AgentEntryPoint);

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

            request.Headers.Add("HTTP_AUTH_LOGIN", connectionInfo.Login);
            request.Headers.Add("HTTP_AUTH_PASSWD", connectionInfo.Password);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";

            // Build up the post message header
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("sampfile");
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(uploadFile));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");

            string postHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

            // Build the trailing boundary string as a byte array
            // ensuring the boundary appears on a line by itself
            byte[] boundaryBytes =
              Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            FileStream fileStream = new FileStream(uploadFile,
                   FileMode.Open, FileAccess.Read);
            long length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            request.ContentLength = length;
            Stream stream = request.GetRequestStream();

            stream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            // Write out the file contents
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                stream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary
            stream.Write(boundaryBytes, 0, boundaryBytes.Length);

            XmlDocument result = GetResponse(request);
            return result;
        }

        private static HttpWebRequest SendRequest(string message)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(connectionInfo.AgentEntryPoint);

            request.Method = "POST";
            request.Headers.Add("HTTP_AUTH_LOGIN", connectionInfo.Login);
            request.Headers.Add("HTTP_AUTH_PASSWD", connectionInfo.Password);
            request.ContentType = "text/xml";
            request.ContentLength = message.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] buffer = encoding.GetBytes(message);

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, message.Length);
            }

            return request;
        }

        private static XmlDocument GetResponse(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (TextReader reader = new StreamReader(stream))
            {
                return ParseAndValidate(reader, connectionInfo.OutputValidationSchema);
            }
        }

        private static XmlDocument ParseAndValidate(TextReader xml, string schemaUri)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, schemaUri);

            XmlReaderSettings settings = new XmlReaderSettings();
            if (XmlSchemaValidation != null)
                settings.ValidationEventHandler += new ValidationEventHandler(XmlSchemaValidation);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.Schemas = schemas;

            XmlDocument document = new XmlDocument();

            using (XmlReader reader = XmlTextReader.Create(xml, settings))
            {
                document.Load(reader);
            }

            return document;
        }

        private static bool RemoteCertificateValidation(object sender,
              X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors != SslPolicyErrors.RemoteCertificateNotAvailable)
                return true;

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public static XmlDocument Send(string packet)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidation);
            HttpWebRequest request = SendRequest(packet);
            XmlDocument result = GetResponse(request);
            return result;
        }

        public static XmlDocument UploadFile(string uploadFile)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidation);
            XmlDocument result = Request.uploadFile(uploadFile);

            return result;
        }
    }
}