using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class IdentityType : KeywordParser
    {
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            return new Master(util).GetMasterOptions(BaseKeyword);
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            new BusinessOfficerMaster(util).UpdateIdentityParserDetail(BaseKeyword, value, serviceBusinessId, officerStepId);
        }
    }
    
}
