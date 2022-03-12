using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class ServiceDefinitionModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string HasOptionalDocument { get; set; }
        public string SessionToken { get; set; }
    }

    public class ServiceSOPModel
    {
        public int StepId { get; set; }
        public int StepNo { get; set; }
        public string Code { get; set; }
        public string Executor { get; set; }
        public string Remarks { get; set; }
        public int DependencyStepNo { get; set; }
        public string DocumentName { get; set; }
        public string VersionNo { get; set; }
        public string FilePath { get; set; }
    }

    public class ServiceRegistrationModel
    {
        public string ServiceCode { get; set; }
        public int BusinessProfileId { get; set; }
        public string StepIds { get; set; }
        public string SessionToken { get; set; }
    }

    public class ServiceRegistrationWorkingModel
    {
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ServiceCode { get; set; }
        public string StepNo { get; set; }
        public string Executor { get; set; }
        public string DocumentCode { get; set; }
        public string Keyword { get; set; }
        public string OfficerId { get; set; }
        public string BusinessProfileId { get; set; }
        public string CapturedSource { get; set; }
        public string CapturedValue { get; set; }
        public string CapturedBy { get; set; }
    }

    public class ServiceRegistrationClientDisplayModel
    {
        public string Id { get; set; }
        public string BusinessProfileName { get; set; }
        public string uen { get; set; }
        public string Status { get; set; }
        public string GeneratedDate { get; set; }
        public string ServiceBusinessId { get; set; }
}

    public class ServiceRegistrationDisplayModel
    {
        public string Created { get; set; }
        public string ServiceName { get; set; }
        public string BusinessProfileName { get; set; }
        public string OfficerName { get; set; }
        public string DocumentName { get; set; }
        public string Executor { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string GeneratedFileName { get; set; }
        public string DownloadedFileName { get; set; }
        public int OfficerId { get; set; }
        public int OfficerStepId { get; set; }
        public int ServiceBusinessId { get; set; }
        
    }

    public class ServiceRegistrationViewModel
    {
        public string Created { get; set; }
        public string ServiceName { get; set; }
        public string OfficerName { get; set; }
        public string Status { get; set; }
        public int ServiceBusinessId { get; set; }
        public string ServiceCode { get; set; }
        public string BusinessProfileId { get; set; }
    }

    public class ServiceSummaryRequestModel
    {
        public int BusinessProfileId { get; set; }
        public string Uen { get; set; }
        public string IncorpDate { get; set; }
        public string OfficerName { get; set; }
        public string DocumentName { get; set; }
        public string ServiceCode { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ServiceSummaryResponseModel
    {
        public int BusinessProfileId { get; set; }
        public string ServiceName { get; set; }
        public string BusinessProfileName { get; set; }
        public string Uen { get; set; }
        public string IncorpDate { get; set; }
        public string OfficerName { get; set; }
        public string DocumentName { get; set; }
        public string ServiceCode { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public int ServiceBusinessId { get; set; }
    }

}
