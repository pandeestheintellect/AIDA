using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class ServiceRegistrationController : APIController
    {
        [HttpGet]
        [Route("api/service-registration/{serviceCode}/{companyId}/{status}")]
        public List<ServiceRegistrationDisplayModel> GetServiceRegistration(string serviceCode, int companyId, string status)
        {
            return new ServiceRegistrationMaster(Util).GetServiceRegistration(serviceCode, companyId, status);
        }
        [HttpGet]
        [Route("api/service-registration/{serviceCode}/{companyId}/{status}/{serviceBusinessId}")]
        public List<ServiceRegistrationDisplayModel> GetServiceRegistration(string serviceCode, int companyId, string status,int serviceBusinessId)
        {
            if (serviceBusinessId>0)
                return new ServiceRegistrationMaster(Util).GetServiceRegistrationById(serviceBusinessId);
            else
                return new ServiceRegistrationMaster(Util).GetServiceRegistration(serviceCode, companyId, status);
        }
        [HttpGet]
        [Route("api/service-registration/{serviceCode}/{companyId}/{status}/{startDate}/{endDate}/{entity}")]
        public List<ServiceRegistrationClientDisplayModel> GetServiceRegistrationForDateRange(string serviceCode, int companyId, string status, string startDate, string endDate,string entity)
        {
            return new ServiceRegistrationMaster(Util).GetServiceRegistrationForDateRange(serviceCode, companyId, status,startDate, endDate, entity);
        }
        [HttpGet]
        [Route("api/service-registration/{serviceBusinessId}/{officerId}")]
        public List<ServiceRegistrationDisplayModel> GetServiceRegistrationForOfficer(int serviceBusinessId, int officerId)
        {
            return new ServiceRegistrationMaster(Util).GetServiceRegistrationForOfficer(serviceBusinessId, officerId);
        }
        [HttpGet]
        [Route("api/service-registration-view/{businessProfileId}")]
        public List<ServiceRegistrationViewModel> GetServiceRegistrationForClient(int businessProfileId)
        {
            return new ServiceRegistrationMaster(Util).GetServiceRegistrationForClient(businessProfileId);
        }
        [Route("api/service-registration")]
        [HttpPut]
        public ResponseModel PutServiceRegistration(ServiceRegistrationModel serviceRegistration)
        {
            return new ServiceRegistrationMaster(Util).PutServiceRegistration(serviceRegistration);
        }
        [Route("api/service-registration/{serviceBusinessId}")]
        [HttpDelete]
        public ResponseModel DeleteRegistration(int serviceBusinessId)
        {
            return new ServiceRegistrationMaster(Util).DeleteRegistration(serviceBusinessId);
        }

        [HttpGet]
        [Route("api/service-documents/{serviceCode}")]
        public List<DocumentModel> GetServiceDocument(string serviceCode)
        {
            return new ServiceRegistrationMaster(Util).GetServiceDocument(serviceCode);
        }

        [HttpPost]
        [Route("api/service-summary")]
        public List<ServiceSummaryResponseModel> GetServiceSummary(ServiceSummaryRequestModel serviceSummary)
        {
            return new ServiceRegistrationMaster(Util).GetServiceSummary(serviceSummary);
        }
    }
}
