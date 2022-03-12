using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class Executor : KeywordParser
    {
        private string userRole = "";
        public Executor()
        {
            
        }
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            userRole = BaseKeyword + "-";
            userRole = userRole.Replace("P:", "");
            userRole = userRole.Substring(0, userRole.IndexOf("-"));
            return new BusinessOfficerMaster(util).GetSignatoryCodeAndName(serviceBusinessId, userRole);
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            new BusinessOfficerMaster(util).UpdateSignatoryParserDetail(BaseKeyword, value, serviceBusinessId, officerStepId);
        }
    }
}
