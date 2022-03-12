using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class ServiceDefinitionController : APIController
    {
        [HttpGet]
        [Route("api/service-definitions")]
        public List<ServiceDefinitionModel> GetServiceDefinition()
        {
            return new ServiceDefinitionMaster(Util).GetServiceDefinition();
        }
        [HttpGet]
        [Route("api/service-definitions/{serviceCode}")]
        public ServiceDefinitionModel GetServiceDefinitionForServiceCode(string serviceCode)
        {
            return new ServiceDefinitionMaster(Util).GetServiceDefinitionForServiceCode(serviceCode);
        }
        [Route("api/service-definitions")]
        [HttpPost]
        public ResponseModel PostServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return new ServiceDefinitionMaster(Util).PostServiceDefinition(serviceDefinition);
        }

        [Route("api/service-definitions")]
        [HttpPut]
        public ResponseModel PutServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return new ServiceDefinitionMaster(Util).PutServiceDefinition(serviceDefinition);
        }

        [Route("api/service-definitions/{serviceCode}")]
        [HttpDelete]
        public ResponseModel DeleteServiceDefinition(string serviceCode)
        {
            return new ServiceDefinitionMaster(Util).DeleteServiceDefinition(serviceCode);
        }

        [HttpGet]
        [Route("api/service-definitions-documents/{serviceCode}")]
        public List<DropDownModel> GetServiceDocuments(string serviceCode)
        {
            return new ServiceDefinitionMaster(Util).GetServiceDocuments(serviceCode);
        }

        [Route("api/service-definitions-documents")]
        [HttpPut]
        public ResponseModel PutServiceDocuments(List<DropDownModel> servicesDocuments)
        {
            return new ServiceDefinitionMaster(Util).PutServiceDocuments(servicesDocuments);
        }
    }
}
