using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class BusinessActivityController  : APIController
    {

        [HttpGet]
        [Route("api/business-activity/{businessProfileId}")]
        public List<BusinessActivityModel> GetBusinessProfile(int businessProfileId)
        {
            return new BusinessActivityMaster(Util).GetBusinessActivity(businessProfileId);
        }

        [Route("api/business-activity")]
        [HttpPut]
        public ResponseModel PutBusinessActivity(List<BusinessActivityModel> activities)
        {
            return new BusinessActivityMaster(Util).PutBusinessActivity(activities);
        }
    }
}
