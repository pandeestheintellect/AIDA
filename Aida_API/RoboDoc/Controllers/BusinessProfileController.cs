using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class BusinessProfileController : APIController
    {
        [HttpGet]
        [Route("api/business-profile")]
        public List<BusinessProfileModel> GetBusinessProfile()
        {
            return new BusinessProfileMaster(Util).GetBusinessProfile();
        }

        [HttpGet]
        [Route("api/business-profile/{companyId}")]
        public BusinessProfileModel GetBusinessProfile(int companyId)
        {
            return new BusinessProfileMaster(Util).GetBusinessProfile(companyId);
        }

        [Route("api/business-profile")]
        [HttpPost]
        public ResponseModel PostBusinessProfile(BusinessProfileModel businessProfile)
        {
            return new BusinessProfileMaster(Util).PostBusinessProfile(businessProfile);
        }

        [Route("api/business-profile")]
        [HttpPut]
        public ResponseModel PutBusinessProfile(BusinessProfileModel businessProfile)
        {
            return new BusinessProfileMaster(Util).PutBusinessProfile(businessProfile);
        }

        [Route("api/business-profile/{companyId}")]
        [HttpDelete]
        public ResponseModel DeleteBusinessProfile(int companyId)
        {
            return new BusinessProfileMaster(Util).DeleteBusinessProfile(companyId);
        }

        [HttpGet]
        [Route("api/entity-shareholders/{businessProfileId}")]
        public List<EntityShareholderModel> GetEntityShareholder(int businessProfileId)
        {
            return new EntityShareholderMaster(Util).GetEntityShareholder(businessProfileId);
        }

        [Route("api/entity-shareholders")]
        [HttpPost]
        public ResponseModel PostEntityShareholder(EntityShareholderModel entityShareholder)
        {
            return new EntityShareholderMaster(Util).PostEntityShareholder(entityShareholder);
        }

        [Route("api/entity-shareholders")]
        [HttpPut]
        public ResponseModel PutEntityShareholder(EntityShareholderModel entityShareholder)
        {
            return new EntityShareholderMaster(Util).PutEntityShareholder(entityShareholder);
        }

        [Route("api/entity-shareholders/{companyId}")]
        [HttpDelete]
        public ResponseModel DeleteEntityShareholder(int companyId)
        {
            return new EntityShareholderMaster(Util).DeleteEntityShareholder(companyId);
        }

        [Route("api/business-profile-representative/{businessProfileId}")]
        [HttpGet]
        public List<DropDownModel> GetRepresentative(int businessProfileId)
        {
            return new EntityShareholderMaster(Util).GetRepresentative(businessProfileId);
        }
    }
}
