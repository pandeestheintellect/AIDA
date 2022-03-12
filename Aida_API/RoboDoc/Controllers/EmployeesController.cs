using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class EmployeesController : APIController
    {
        [HttpGet]
        [Route("api/employees")]
        public List<EmployeeModel> GetEmployees()
        {
            return new EmployeeMaster(Util).GetEmployee();
        }

        [HttpGet]
        [Route("api/employees/{code}/{positionOrrole}")]
        public List<DropDownModel> GetPositionOrRole(string code,string positionOrrole)
        {
            return new EmployeeMaster(Util).GetPositionOrRole(code, positionOrrole);
        }

        [Route("api/employees")]
        [HttpPost]
        public ResponseModel PostEmployee(EmployeeModelWrapper employee)
        {
            return new EmployeeMaster(Util).PostEmployee(employee);
        }

        [Route("api/employees")]
        [HttpPut]
        public ResponseModel PutEmployee(EmployeeModelWrapper employee)
        {
            return new EmployeeMaster(Util).PutEmployee(employee);
        }

        [Route("api/employees/{code}")]
        [HttpDelete]
        public ResponseModel DeleteEmployee(string code)
        {
            return new EmployeeMaster(Util).DeleteEmployee(code);
        }
    }
}
