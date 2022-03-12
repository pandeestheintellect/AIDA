using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;


namespace RoboDoc.Controllers
{
    public class RegistersController : APIController
    {

        [HttpGet]
        [Route("api/register/download-shareholdings-registers/{businessProfileId}/{officerId}")]
        public string DownloadAuditorsRegister(int businessProfileId,int officerId)
        {
            return new RegistersManager(Util).DownloadShareholdingsRegister(businessProfileId, officerId);
        }

        #region  Applications Allotments

        [HttpGet]
        [Route("api/register/download-applications-allotments/{businessProfileId}/{officerId}")]
        public string DownloadApplicationsAllotments(int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).DownloadApplicationsAllotments(businessProfileId, officerId);
        }

        [Route("api/register/applications-allotments/{businessProfileId}/{officerId}")]
        [HttpGet]
        public List<ApplicationsAllotmentsModel> GetApplicationsAllotments(int businessProfileId,int officerId)
        {
            return new RegistersManager(Util).GetApplicationsAllotments(businessProfileId, officerId);
        }
        [Route("api/register/applications-allotments")]
        [HttpPost]
        public ResponseModel PostApplicationsAllotments(ApplicationsAllotmentsModel allotment)
        {
            return new RegistersManager(Util).PostApplicationsAllotments(allotment);
        }
        [Route("api/register/applications-allotments")]
        [HttpPut]
        public ResponseModel PutApplicationsAllotments(ApplicationsAllotmentsModel allotment)
        {
            return new RegistersManager(Util).PutApplicationsAllotments(allotment);
        }
        [Route("api/register/applications-allotments/{id}")]
        [HttpDelete]
        public ResponseModel DeleteApplicationsAllotments(int id)
        {
            return new RegistersManager(Util).DeleteRegister("ApplicationsAllotments","ApplicationsAllotments deleted",id);
        }

        #endregion

        #region  Members Registers

        [HttpGet]
        [Route("api/register/download-members-registers/{businessProfileId}/{officerId}")]
        public string DownloadMembersRegister(int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).DownloadMembersRegister(businessProfileId, officerId);
        }

        [Route("api/register/members-registers/{businessProfileId}/{officerId}")]
        [HttpGet]
        public List<MembersRegisterModel> GetMembersRegister(int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).GetMembersRegister(businessProfileId, officerId);
        }
        [Route("api/register/members-registers")]
        [HttpPost]
        public ResponseModel PostMembersRegister(MembersRegisterModel members)
        {
            return new RegistersManager(Util).PostMembersRegister(members);
        }
        [Route("api/register/members-registers")]
        [HttpPut]
        public ResponseModel PutMembersRegister(MembersRegisterModel members)
        {
            return new RegistersManager(Util).PutMembersRegister(members);
        }
        [Route("api/register/members-registers/{id}")]
        [HttpDelete]
        public ResponseModel DeleteMembersRegister(int id)
        {
            return new RegistersManager(Util).DeleteRegister("MembersShareRegister", "Members Register deleted", id);
        }

        #endregion

        #region  Transfers Registers

        [HttpGet]
        [Route("api/register/download-transfers-registers/{businessProfileId}/{transferorId}/{transfereeId}")]
        public string DownloadTransfersRegister(int businessProfileId, int transferorId, int transfereeId)
        {
            return new RegistersManager(Util).DownloadTransfersRegister(businessProfileId, transferorId, transfereeId);
        }

        [Route("api/register/transfers-registers/{businessProfileId}/{transferorId}/{transfereeId}")]
        [HttpGet]
        public List<TransfersRegisterModel> GetTransfersRegister(int businessProfileId, int transferorId, int transfereeId)
        {
            return new RegistersManager(Util).GetTransfersRegister(businessProfileId, transferorId, transfereeId);
        }
        [Route("api/register/transfers-registers")]
        [HttpPost]
        public ResponseModel PostTransfersRegister(TransfersRegisterModel transfers)
        {
            return new RegistersManager(Util).PostTransfersRegister(transfers);
        }
        [Route("api/register/transfers-registers")]
        [HttpPut]
        public ResponseModel PutTransfersRegister(TransfersRegisterModel transfers)
        {
            return new RegistersManager(Util).PutTransfersRegister(transfers);
        }
        [Route("api/register/transfers-registers/{id}")]
        [HttpDelete]
        public ResponseModel DeleteTransfersRegister(int id)
        {
            return new RegistersManager(Util).DeleteRegister("TransfersRegister", "Transfers Register deleted", id);
        }

        #endregion

        #region  Officers Registers

        [HttpGet]
        [Route("api/register/download-officers-registers/{userrole}/{businessProfileId}/{officerId}")]
        public string DownloadOfficersRegister(string userrole,int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).DownloadOfficersRegister(userrole, businessProfileId, officerId);
        }

        [Route("api/register/officers-registers/{userrole}/{businessProfileId}/{officerId}")]
        [HttpGet]
        public List<OfficersRegisterModel> GetOfficersRegister(string userrole, int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).GetOfficersRegister(userrole, businessProfileId, officerId);
        }
        [Route("api/register/officers-registers/{userrole}")]
        [HttpPost]
        public ResponseModel PostDirectorsRegister(string userrole, OfficersRegisterModel directors)
        {
            return new RegistersManager(Util).PostOfficersRegister(userrole, directors);
        }
        [Route("api/register/officers-registers/{userrole}")]
        [HttpPut]
        public ResponseModel PutDirectorsRegister(string userrole, OfficersRegisterModel directors)
        {
            return new RegistersManager(Util).PutOfficersRegister(userrole, directors);
        }
        [Route("api/register/officers-registers/{userrole}/{id}")]
        [HttpDelete]
        public ResponseModel DeleteDirectorsRegister(string userrole, int id)
        {
            return new RegistersManager(Util).DeleteRegister("OfficersRegister", userrole+" Register deleted", id);
        }

        #endregion

        #region  Auditors Registers

        [HttpGet]
        [Route("api/register/download-auditors-registers/{businessProfileId}")]
        public string DownloadAuditorsRegister(int businessProfileId)
        {
            return new RegistersManager(Util).DownloadAuditorsRegister(businessProfileId);
        }

        [Route("api/register/auditors-registers/{businessProfileId}")]
        [HttpGet]
        public List<AuditorsRegisterModel> GetAuditorsRegister(int businessProfileId)
        {
            return new RegistersManager(Util).GetAuditorsRegister(businessProfileId);
        }
        [Route("api/register/auditors-registers")]
        [HttpPost]
        public ResponseModel PostAuditorsRegister(AuditorsRegisterModel members)
        {
            return new RegistersManager(Util).PostAuditorsRegister(members);
        }
        [Route("api/register/auditors-registers")]
        [HttpPut]
        public ResponseModel PutAuditorsRegister(AuditorsRegisterModel members)
        {
            return new RegistersManager(Util).PutAuditorsRegister(members);
        }
        [Route("api/register/auditors-registers/{id}")]
        [HttpDelete]
        public ResponseModel DeleteAuditorsRegister(int id)
        {
            return new RegistersManager(Util).DeleteRegister("AuditorsRegister", "Auditors Register deleted", id);
        }

        #endregion

        #region  Mortgages Charges

        [HttpGet]
        [Route("api/register/download-mortgages-charges/{businessProfileId}")]
        public string DownloadMortgagesCharges(int businessProfileId)
        {
            return new RegistersManager(Util).DownloadMortgagesCharges(businessProfileId);
        }

        [Route("api/register/mortgages-charges/{businessProfileId}")]
        [HttpGet]
        public List<MortgagesChargesModel> GetMortgagesCharges(int businessProfileId)
        {
            return new RegistersManager(Util).GetMortgagesCharges(businessProfileId);
        }
        [Route("api/register/mortgages-charges")]
        [HttpPost]
        public ResponseModel PostMortgagesCharges(MortgagesChargesModel members)
        {
            return new RegistersManager(Util).PostMortgagesCharges(members);
        }
        [Route("api/register/mortgages-charges")]
        [HttpPut]
        public ResponseModel PutMortgagesCharges(MortgagesChargesModel members)
        {
            return new RegistersManager(Util).PutMortgagesCharges(members);
        }
        [Route("api/register/mortgages-charges/{id}")]
        [HttpDelete]
        public ResponseModel DeleteMortgagesCharges(int id)
        {
            return new RegistersManager(Util).DeleteRegister("MortgagesCharges", "Mortgages and Charges deleted", id);
        }

        #endregion


        #region  Controllers Corporate Register

        [HttpGet]
        [Route("api/register/download-corporate-register/{businessProfileId}")]
        public string DownloadControllersCorporateRegister(int businessProfileId)
        {
            return new RegistersManager(Util).DownloadControllersCorporateRegister(businessProfileId);
        }

        [Route("api/register/corporate-register/{businessProfileId}")]
        [HttpGet]
        public List<ControllersCorporateRegisterModel> GetControllersCorporateRegister(int businessProfileId)
        {
            return new RegistersManager(Util).GetControllersCorporateRegister(businessProfileId);
        }
        [Route("api/register/corporate-register")]
        [HttpPost]
        public ResponseModel PostControllersCorporateRegister(ControllersCorporateRegisterModel corporate)
        {
            return new RegistersManager(Util).PostControllersCorporateRegister(corporate);
        }
        [Route("api/register/corporate-register")]
        [HttpPut]
        public ResponseModel PutControllersCorporateRegister(ControllersCorporateRegisterModel corporate)
        {
            return new RegistersManager(Util).PutControllersCorporateRegister(corporate);
        }
        [Route("api/register/corporate-register/{id}")]
        [HttpDelete]
        public ResponseModel DeleteControllersCorporateRegister(int id)
        {
            return new RegistersManager(Util).DeleteRegister("ControllersCorporateRegister", "Controllers Corporate Register deleted", id);
        }

        #endregion



        #region  Nominee Registers
        [HttpGet]
        [Route("api/register/nominator-dropdown/{companyId}")]
        public List<DropDownModel> GetNominatorDown(int companyId)
        {
            return new RegistersManager(Util).GetNominatorDown(companyId);
        }

        
        [HttpGet]
        [Route("api/register/download-nominee-registers/{businessProfileId}/{officerId}")]
        public string DownloadNomineeRegister(int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).DownloadNomineeRegister( businessProfileId, officerId);
        }
        
        [Route("api/register/nominee-registers/{businessProfileId}/{officerId}")]
        [HttpGet]
        public List<NomineeRegisterModel> GetNomineeRegister(int businessProfileId, int officerId)
        {
            return new RegistersManager(Util).GetNomineeRegister(businessProfileId, officerId);
        }
        [Route("api/register/nominee-registers")]
        [HttpPost]
        public ResponseModel PostDirectorsRegister(NomineeRegisterModel directors)
        {
            return new RegistersManager(Util).PostNomineeRegister(directors);
        }
        [Route("api/register/nominee-registers")]
        [HttpPut]
        public ResponseModel PutDirectorsRegister(NomineeRegisterModel directors)
        {
            return new RegistersManager(Util).PutNomineeRegister(directors);
        }
        [Route("api/register/nominee-registers/{id}")]
        [HttpDelete]
        public ResponseModel DeleteDirectorsRegister(int id)
        {
            return new RegistersManager(Util).DeleteRegister("NomineeRegister", "Nominee Register deleted", id);
        }

        #endregion

    }
}
