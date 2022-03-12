using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib
{
    public class ControllerUtil 
    {
        public string ConnectionString { get; set; }
        public string ClientIP { get; set; }
        public string DocPath { get; set; }
        public bool isLive{ get; set; }
    }
}
