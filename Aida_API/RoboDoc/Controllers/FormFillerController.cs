using RoboDocCore.Models;
using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace RoboDoc.Controllers
{
    public class FormFillerController  : APIController
    {

        [HttpGet]
        [Route("api/form-filling/{serviceBusinessId}/{officerId}")]
        public HttpResponseMessage GetForm(int serviceBusinessId, int officerId)
        {

            string htmlString = new HTMLDocumentManager(Util)
                .GetForm(ConfigurationManager.AppSettings["RoboDocPath"],

                serviceBusinessId, officerId, "HTML");

            var response = new HttpResponseMessage();
            response.Content = new StringContent(htmlString);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
        [HttpGet]
        [Route("api/form-pdf/{serviceBusinessId}/{officerId}")]
        public string GetPDFForm(int serviceBusinessId, int officerId)
        {

            return new HTMLDocumentManager(Util)
                .GetPDFForm(ConfigurationManager.AppSettings["RoboDocPath"],
                ConfigurationManager.AppSettings["MyPath"],
                serviceBusinessId, officerId);
        }

        [HttpGet]
        [Route("api/form-preview-pdf/{serviceBusinessId}/{officerId}")]
        public string GetPDFPreviewForm(int serviceBusinessId, int officerId)
        {

            return  new HTMLDocumentManager(Util)
                .GetPDFForm(ConfigurationManager.AppSettings["RoboDocPath"],
                "PREVIEW",
                serviceBusinessId, officerId);
        }
        [Route("api/form-pdf/signing")]
        [HttpPost]
        public ResponseModel DispatchDocument(ServicesSignSave serviceSign)
        {
            return new HTMLDocumentManager(Util)
                .DispatchDocument(ConfigurationManager.AppSettings["RoboDocPath"],
                serviceSign);
        }
        [HttpPost]
        [Route("api/form-filling/{serviceBusinessId}/{officerId}")]
        public async Task<string> PostRawForm(int serviceBusinessId, int officerId)
        {
            string result = await Request.Content.ReadAsStringAsync();
            new HTMLDocumentManager(Util)
            .PostForm(result, serviceBusinessId, officerId);
            
            return "Form submitted";
        }
        [HttpPost]
        [Route("api/form-pdf/{serviceBusinessId}/{officerId}")]
        public async Task<HttpResponseMessage> PostRawPDFForm(int serviceBusinessId, int officerId)
        {
            string result = await Request.Content.ReadAsStringAsync();
            new HTMLDocumentManager(Util)
            .PostPDFForm(result, serviceBusinessId, officerId);
            var response = new HttpResponseMessage();
            //response.Content = new StringContent("<script language=javascript>\n" +
            //        "function redirect(){\n" +
            //        "  window.top.location.href = \"https://www.google.come\";\n" +
            //        "}\n" +
            //        "</script>\n" +
            //        "\n" +
            //        "<body onload=\"redirect()\">\n" +
            //        "\n" +
            //        "</body>");
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.StatusCode = HttpStatusCode.NoContent;
            return response;
        }

        [HttpGet]
        [Route("api/import-client/{filepath}")]
        public BusinessProfileModel ImportBusinessProfile(string filepath)
        {

            return new HTMLDocumentManager(Util)
                .ImportBusinessProfile(ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\" +
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filepath)));
        }
        [HttpGet]
        [Route("api/import-entity/{filepath}")]
        public EntityShareholderModel ImportEntityProfile(string filepath)
        {

            return new HTMLDocumentManager(Util)
                .ImportEntityProfile(ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\" +
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filepath)));
        }
        [HttpGet]
        [Route("api/import-officer/{filepath}")]
        public BusinessOfficerModel ImportBusinessOfficer(string filepath)
        {

            return new HTMLDocumentManager(Util)
                .ImportBusinessOfficer(ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\" +
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filepath)));
        }
        [HttpGet]
        [Route("api/import-registration/{serviceBusinessId}/{officerId}/{filepath}")]
        public string PostOfflineForm(int serviceBusinessId, int officerId,string filepath)
        {
            new HTMLDocumentManager(Util)
            .PostOfflineForm(ConfigurationManager.AppSettings["RoboDocPath"] + @"\Upload\" +
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filepath)), serviceBusinessId, officerId);

            return "Form submitted";
        }
    }
}
