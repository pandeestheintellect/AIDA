using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class BusinessOfficerController : APIController
    {
        [HttpGet]
        [Route("api/business-officers/{companyId}")]
        public List<BusinessOfficerModel> GetBusinessOfficer(int companyId)
        {
            return new BusinessOfficerMaster(Util).GetBusinessOfficer(companyId);
        }
        [HttpGet]
        [Route("api/business-officers-dropdown/{companyId}")]
        public List<DropDownModel> GetBusinessOfficerDown(int companyId)
        {
            return new BusinessOfficerMaster(Util).GetBusinessOfficerDown(companyId);
        }

        [HttpGet]
        [Route("api/business-authorised-representative/{companyId}")]
        public BusinessOfficerModel GetAuthorisedRepresentative(int companyId)
        {
            return new BusinessOfficerMaster(Util).GetAuthorisedRepresentative(companyId);
        }


        [Route("api/business-officers")]
        [HttpPost]
        public ResponseModel PostBusinessOfficer(BusinessOfficerModel businessOfficer)
        {
            return new BusinessOfficerMaster(Util).PostBusinessOfficer(businessOfficer);
        }

        [Route("api/business-officers")]
        [HttpPut]
        public ResponseModel PutBusinessOfficer(BusinessOfficerModel businessOfficer)
        {
            return new BusinessOfficerMaster(Util).PutBusinessOfficer(businessOfficer);
        }

        [Route("api/business-officers/{officerId}")]
        [HttpDelete]
        public ResponseModel DeleteBusinessOfficer(int officerId)
        {
            return new BusinessOfficerMaster(Util).DeleteBusinessOfficer(officerId);
        }

        
    }
}
