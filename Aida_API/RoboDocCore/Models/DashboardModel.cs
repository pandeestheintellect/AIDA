using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class StatusCountModel
    {
        public int Periods { get; set; }
        public int Counts { get; set; }
        public string DisplayName { get; set; }
        public string Status { get; set; }
    }

    public class ServiceStatusCountModel
    {
        public string Name { get; set; }
        public string ServiceCode { get; set; }
        public int MonthlyCounts { get; set; }
        public int YearlyCounts { get; set; }
    }

    public class ServicePerformanceCountModel
    {
        public string Name { get; set; }
        public string ServiceCode { get; set; }
        public string Created { get; set; }
        public int Counts { get; set; }
        public int CreatedMonth { get; set; }
    }

    public class ServicePerformanceReportModel 
    {
        public string Name { get; set; }
        public List<ChartDataModel> Series { get; set; }
    }
    public class ChartDataModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ServiceEntityCountModel
    {
        public string Name { get; set; }
        public string ClientType { get; set; }
        public string ServiceCode { get; set; }
        public string Status { get; set; }
        public string Created { get; set; }
        public int Counts { get; set; }
        public int CreatedMonth { get; set; }
    }

    public class ActiveProfileModel
    {
        public int BusinessProfileId { get; set; }
        public int Counts { get; set; }
        public string Name { get; set; }
    }

    public class EventInfoModel
    {
        public string EventName { get; set; }
        public string Name { get; set; }
        public string Entity { get; set; }
        public string EventDate { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
