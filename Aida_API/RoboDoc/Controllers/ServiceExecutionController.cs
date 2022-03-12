using Newtonsoft.Json;
using RoboDocCore.Models;
using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class ServiceExecutionController : APIController
    {
        [HttpGet]
        [Route("api/service-execution/{serviceBusinessId}/{officerId}")]
        public List<ServicesEntry> GetServicesEntry(int serviceBusinessId, int officerId)
        {
            return new ServiceExecution(Util).GetServiceEntry(serviceBusinessId, officerId);
        }

        [Route("api/service-execution/preview/{serviceBusinessId}/{officerId}")]
        [HttpGet]
        public string GetDocumentPreview(int serviceBusinessId, int officerId)
        {
            
            return new DocumentManager(Util)
                .PreviewDocument(ConfigurationManager.AppSettings["RoboDocPath"],
                
                serviceBusinessId, officerId,"Template");
 
        }
        
        [Route("api/service-execution")]
        [HttpPost]
        public ResponseModel PostServiceEntry(ServicesEntrySave serviceEntry)
        {
            serviceEntry.ElementObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(serviceEntry.Elements);
            return new ServiceExecution(Util).PostServiceEntry(serviceEntry);
        }

        [Route("api/service-execution")]
        [HttpPut]
        public ResponseModel PutServiceEntry(ServicesEntrySave serviceEntry)
        {
            return new ServiceExecution(Util).PutServiceEntry(serviceEntry);
        }

        [Route("api/service-execution/signing")]
        [HttpPost]
        public ResponseModel DispatchDocument(ServicesSignSave serviceSign)
        {
            return new DocumentManager(Util)
                .DispatchDocument(ConfigurationManager.AppSettings["RoboDocPath"],
                serviceSign);
        }
        [HttpGet]
        [Route("api/digisigner/callback")]
        public HttpResponseMessage GetCallback(SignerCallback document)
        {
            new DocumentManager(Util)
                .ProcessSignerCallback(ConfigurationManager.AppSettings["RoboDocPath"],
                document);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent("DIGISIGNER_EVENT_ACCEPTED", Encoding.Unicode);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;
        }
        
        [HttpPost]
        [Route("api/digisigner/callback")]
        public HttpResponseMessage PostCallback(SignerCallback document)
        {
            new DocumentManager(Util)
                .ProcessSignerCallback(ConfigurationManager.AppSettings["RoboDocPath"],
                document);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent("DIGISIGNER_EVENT_ACCEPTED", Encoding.Unicode);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;
        }

        [Route("api/service-execution/invite")]
        [HttpPost]
        public ResponseModel SendInvite(ServicesSignSave serviceSign)
        {
            return new ServiceExecution(Util).SendInvite(serviceSign);
        }
        /*
        [Route("api/service-execution-initial-document")]
        [HttpPost]
        public ResponseModel SendInitialDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument)
        {
            return new ServiceExecution(Util).SendInitialDocument(serviceRegistraionDocument,
                ConfigurationManager.AppSettings["RoboDocPath"], "Registration");
        }
        */
        [Route("api/service-execution-send-document")]
        [HttpPost]
        public ResponseModel SendFormDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument)
        {
            return new ServiceExecution(Util).SendFormDocument(serviceRegistraionDocument,
                ConfigurationManager.AppSettings["RoboDocPath"]);
        }
        [Route("api/service-execution-send-document/{document}/{start}/{end}")]
        [HttpGet]
        public List<ServiceRegistraionDocumentModel> GetSendFormDocument(string document, string start, string end)
        {
            return new ServiceExecution(Util).GetSendFormDocument(document,start, end);
        }

        [Route("api/service-execution-send-document")]
        [HttpPut]
        public ResponseModel PutSendFormDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument)
        {
            return new ServiceExecution(Util).PutSendFormDocument(serviceRegistraionDocument);
        }

        [Route("api/service-execution-initial-document/{periods}/{status}")]
        [HttpGet]
        public List<ServiceRegistraionDocumentModel> GetInitialDocument(int periods, string status)
        {
            return new ServiceExecution(Util).GetInitialDocument(periods, status);
        }
/*
        [Route("api/service-execution-initial-document")]
        [HttpPut]
        public ResponseModel PutInitialDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument)
        {
            return new ServiceExecution(Util).PutInitialDocument(serviceRegistraionDocument);
        }
*/
        [Route("api/service-execution-uploaded-document/{businessProfileId}")]
        [HttpGet]
        public List<UploadedDocumentModel> GetUploadedDocument(int businessProfileId)
        {
            return new ServiceExecution(Util).GetUploadedDocument(businessProfileId);
        }

        [Route("api/service-execution-uploaded-document/{businessProfileId}")]
        [HttpDelete]
        public ResponseModel DeleteUploadedDocument(int businessProfileId)
        {
            return new ServiceExecution(Util).DeleteUploadedDocument(businessProfileId);
        }


        [Route("api/service-execution-document-otp/{inviteKey}")]
        [HttpGet]
        public ResponseModel GetDocumentFillingOTP(string inviteKey)
        {
            return new ServiceExecution(Util).GetDocumentFillingOTP(inviteKey);
        }

        [Route("api/service-execution-document-otp")]
        [HttpPost]
        public DocumentFillingStart GetDocumentFillingOTP(DocumentFillingOTP inviteOTP)
        {
            return new ServiceExecution(Util).GetDocumentFillingOTP(inviteOTP);
        }
    }
}
