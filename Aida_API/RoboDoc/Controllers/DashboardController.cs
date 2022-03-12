using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class DashboardController  : APIController
    {
        [HttpGet]
        [Route("api/dashboard-enquiry/{status}")]
        public List<StatusCountModel> GetNewEnquiryStatusWise(string status)
        {
            return new Dashboard(Util).GetNewEnquiryStatusWise(status);
        }

        [HttpGet]
        [Route("api/dashboard-services-status/{status}")]
        public List<ServiceStatusCountModel> GetServiceStatus(string status)
        {
            return new Dashboard(Util).GetServiceStatus(status);
        }

        [HttpGet]
        [Route("api/dashboard-services-performance/{status}")]
        public List<ServicePerformanceReportModel> GetStatusWisePeformance(string status)
        {
            return new Dashboard(Util).GetStatusWisePeformance(status);
        }

        [HttpGet]
        [Route("api/dashboard-services-entity-summary")]
        public List<ServiceEntityCountModel> GetServiceStatusEntityWise()
        {
            return new Dashboard(Util).GetServiceStatusEntityWise();
        }

        [HttpGet]
        [Route("api/dashboard-active-profile")]
        public List<ActiveProfileModel> GetActiveProfile()
        {
            return new Dashboard(Util).GetActiveProfile();
        }
        [HttpGet]
        [Route("api/dashboard-events-info/{eventDate}")]
        public List<EventInfoModel> GetEvents(string eventDate)
        {
            return new Dashboard(Util).GetEvents(eventDate);
        }
    }
}
