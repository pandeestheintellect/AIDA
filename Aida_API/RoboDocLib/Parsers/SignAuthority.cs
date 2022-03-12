using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class SignAuthority : KeywordParser
    {
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            return new BusinessOfficerMaster(util).GetSignatoryCodeAndName(serviceBusinessId);
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            new BusinessOfficerMaster(util).UpdateSignatoryParserDetail(BaseKeyword, value, serviceBusinessId, officerStepId);
        }
    }
}
