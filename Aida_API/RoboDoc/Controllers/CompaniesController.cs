using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class CompaniesController : APIController
    {
        [HttpGet]
        [Route("api/companies")]
        public List<CompanyModel> GetCompanies()
        {
            return new CompanyMaster(Util).GetCompany();
        }

        [Route("api/companies")]
        [HttpPost]
        public ResponseModel PostCompany(CompanyModel company)
        {
            return new CompanyMaster(Util).PostCompany(company);
        }

        [Route("api/companies")]
        [HttpPut]
        public ResponseModel PutCompany(CompanyModel company)
        {
            return new CompanyMaster(Util).PutCompany(company); 
        }

        [Route("api/companies/{companyId}")]
        [HttpDelete]
        public ResponseModel DeleteCompany(int companyId)
        {
            return new CompanyMaster(Util).DeleteCompany(companyId);
        }


    }
}
