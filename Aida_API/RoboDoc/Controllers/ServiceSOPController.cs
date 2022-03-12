using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;


namespace RoboDoc.Controllers
{
    public class ServiceSOPController : APIController
    {
        [HttpGet]
        [Route("api/service-sop/{serviceCode}")]
        public List<ServiceSOPModel> GetServicesSOP(string serviceCode)
        {
            return new ServiceSOPMaster(Util).GetServiceSOP(serviceCode);
        }

        [Route("api/service-sop/{serviceCode}/{executor}")]
        [HttpGet]
        public List<DocumentModel> GetServicesSOPSubscription(string serviceCode, string executor)
        {
            return new ServiceSOPMaster(Util).GetServiceSOPSubscription(serviceCode, executor);
        }
        
        [Route("api/service-sop/{serviceCode}/{executor}")]
        [HttpPut]
        public ResponseModel PutServiceSOP(string serviceCode, string executor, List<DocumentModel> documents)
        {
            return new ServiceSOPMaster(Util).PutServiceSOP(serviceCode, executor, documents);
        }
    }
}
