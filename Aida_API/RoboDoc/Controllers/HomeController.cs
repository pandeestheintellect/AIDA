using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoboDoc.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("api/home")]
        public string Index()
        {
            return "Working";
        }

        [HttpPost]
        [Route("api/home")]
        public string Reg(Reg pizza)
        {
            return "Good";
        }
    }

    public class Reg
    {
        public string CReg { get; set; }
        public string CReg1 { get; set; }
        public string CReg2 { get; set; }
        public string CReg3 { get; set; }
    }
}
