using Newtonsoft.Json;
using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class AdminController : APIController
    {

        [Route("api/login-otp")]
        [HttpPost]
        public ResponseModel GetLoginOTP(LoginModel login)
        {
            return new EmployeeMaster(Util).GetLoginOTP(login);
        }
        [Route("api/login")]
        [HttpPost]
        public LoginResponseModel GetLoginResponse(LoginModel login)
        {
            return new EmployeeMaster(Util).GetLoginResponse(login,
                ConfigurationManager.AppSettings["RoboDocPath"]);
        }

        [HttpGet]
        [Route("api/admin-menus/{role}")]
        public List<AppMenuModel> GetMenu(string role)
        {
            string allText = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["RoboDocPath"] + @"\menu\" + role+ @".json");

            List<AppMenuModel> menuModels =  JsonConvert.DeserializeObject<List<AppMenuModel>>(allText);

            return menuModels;
            //return new MenuMaster(Util).GetMenu(role);
        }
    }
}
