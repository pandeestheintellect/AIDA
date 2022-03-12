using DigiSigner.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocCore.Models
{
    public class ServicesEntry
    {
        public int ServiceBusinessId { get; set; }
        public int OfficerStepId { get; set; }
        public int StepNo { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string DocumentName { get; set; }
        public string OfficerName { get; set; }
        public string OfficerRole { get; set; }
        public string BusinessProfileName { get; set; }
        public string Status { get; set; }
        public string GeneratedFileName { get; set; }

        public List<ServicesEntryFieldModel> Fields { get; set; }
    }
    public class ServicesEntryFieldModel
    {
        public int OfficerStepId { get; set; }
        public string Type { get; set; }
        public string InputType { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string ControlNumber { get; set; }
        public string FxSizePer { get; set; }
        public string[] Options { get; set; }
    }
    public class ServicesEntrySave
    {
        public int ServiceBusinessId { get; set; }
        public int OfficerStepId { get; set; }
        public string Elements { get; set; }
        public Dictionary<string, string> ElementObject { get; set; }
    }

    public class ServicesSignSave
    {
        public int ServiceBusinessId { get; set; }
        public string OfficerStepIds { get; set; }
    }

    public class SignerCallback
    {
        public string event_time { get; set; }
        public string event_type { get; set; }
        public SignatureRequest signature_request { get; set; }
    }

    public class ServiceRegistraionDocumentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string DocumentNames { get; set; }
        public string FilePaths { get; set; }
        public string Status { get; set; }
        public int BusinessProfileId { get; set; }
        public string DocumentType { get; set; }
    }

    public class UploadedDocumentModel
    {
        public string Created { get; set; }
        public string ServiceName { get; set; }
        public string OfficerName { get; set; }
        public string ActualFileName { get; set; }
        public string ServiceCode { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public int DocumentId { get; set; }
        public int BusinessProfileId { get; set; }
    }


    public class DocumentFillingOTP
    {
        public string InviteKey{ get; set; }
        public string OTP { get; set; }
    }

    public class DocumentFillingStart: ResponseModel
    {
        public int ServiceBusinessId { get; set; }
        public int OfficerId { get; set; }
    }
}
