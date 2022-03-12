using NLog;
using RoboDocLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace RoboDoc.Controllers
{
    public class APIController : ApiController
    {
        public ControllerUtil Util { get; private set; }

        public APIController()
        {
            Util = new ControllerUtil();

            Util.ConnectionString = ConfigurationManager.ConnectionStrings["RoboDocDB"].ConnectionString;
            Util.DocPath = ConfigurationManager.AppSettings["RoboDocPath"];
            if(ConfigurationManager.AppSettings["isLive"].Equals("YES"))
            {
                Util.isLive = true;
            }
            else
                Util.isLive = false;
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                Util.ClientIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
    }
}
