using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using RoboDocLib.Services;

namespace RoboDoc.Controllers
{
    public class FileAPIController : ApiController
    {
        [HttpGet]
        [Route("api/file-preview/{filename}")]
        public HttpResponseMessage GetPreview(string fileName)
        {
            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            return GetPreviewData(@"\Output\" + fileName);
        }
        [HttpGet]
        [Route("api/http-download/{filename}")]
        public HttpResponseMessage GetDownload(string fileName)
        {

            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            string filePath = ConfigurationManager.AppSettings["RoboDocPath"] + @"\Output\" + fileName + ".html";

            var response = new HttpResponseMessage();
            response.Content = new StringContent(File.ReadAllText(filePath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
        [HttpGet]
        [Route("api/file-download/{filename}")]
        public HttpResponseMessage GetDownloadWithType(string fileName)
        {
            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            return GetPreviewData(@"\Output\" + fileName);
        }
        [HttpGet]
        [Route("api/register-download/{filename}")]
        public HttpResponseMessage GetRegisterDownload(string fileName)
        {
            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            return GetPreviewData(@"\Output\register\" + fileName);
        }
        [HttpGet]
        [Route("api/file-upload-download/{filename}")]
        public HttpResponseMessage GetUploadedFile(string fileName)
        {
            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            return GetPreviewData(fileName);
        }

        [HttpGet]
        [Route("api/file-download/{filename}/{filetype}")]
        public HttpResponseMessage GetDownload(string fileName, string filetype)
        {
            fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
            return GetPreviewData(@"\Output\" + fileName + "." + filetype);
        }

        private HttpResponseMessage GetPreviewData(string fileName)
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //Set the File Path.
            string filePath = ConfigurationManager.AppSettings["RoboDocPath"] +  fileName;

            //Check whether File exists.
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", fileName);
                throw new HttpResponseException(response);
            }

            //Read the File into a Byte Array.
            byte[] bytes = File.ReadAllBytes(filePath);

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = bytes.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            response.Content.Headers.ContentDisposition.FileName = fileName.Substring(fileName.LastIndexOf("\\"));

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
            return response;
        }

        [HttpPost]
        [Route("api/file-upload/{businessId}/{officerId}/{serviceCode}/{documentType}")]
        public HttpResponseMessage UploadFile(int businessId, int officerId,string serviceCode, string documentType)
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                Random rnd = new Random();
                int myRandomNo = rnd.Next(10000000, 99999999);
                

                var docfiles = new List<string>();

                RoboDocLib.ControllerUtil Util = new RoboDocLib.ControllerUtil();

                Util.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RoboDocDB"].ConnectionString;
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    Util.ClientIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string filePath = ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\UP-" + myRandomNo+  postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);

                    new ServiceExecution(Util).PostUploadDocument(
                        businessId, officerId, serviceCode, documentType, 
                            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(@"\Upload\UP-" + myRandomNo + postedFile.FileName)), 
                            postedFile.FileName);

                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [HttpPost]
        [Route("api/file-import/{filefor}")]
        public HttpResponseMessage ImportFile(string filefor)
        {
            HttpResponseMessage result = null;
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string docfiles = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string filePath = ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\" + filefor +"-"+ myRandomNo+ postedFile.FileName;
                    postedFile.SaveAs(filePath);
                    docfiles = filefor + "-" + myRandomNo+ postedFile.FileName;
                    break;
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }


    }

}
