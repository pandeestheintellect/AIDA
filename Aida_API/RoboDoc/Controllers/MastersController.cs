using RoboDocCore.Models;
using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class MastersController : APIController
    {

        [Route("api/masters/dropdown/{master}")]
        [HttpGet]
        public List<DropDownModel> GetMasterDropDown(string master)
        {
            return new Master(Util).GetMasterDropDown(master);
        }

        [Route("api/masters/add")]
        [HttpPost]
        public ResponseModel AddMaster(DropDownModel values)
        {
            return new Master(Util).AddMaster(values);
        }

        [Route("api/masters/delete/{master}/{masterValue}")]
        [HttpDelete]
        public ResponseModel DeleteMaster(string master, string masterValue)
        {
            return new Master(Util).DeleteMaster(master, masterValue);
        }
    }
}
