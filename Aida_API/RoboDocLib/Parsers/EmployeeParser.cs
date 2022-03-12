using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class EmployeeParser : KeywordParser
    {
        public override List<string> GetList(ControllerUtil util, int ServiceBusinessId)
        {
            return new EmployeeMaster(util).GetEmployeeCodeAndName();
        }
        public override void UpdateDetail(ControllerUtil util, string value, int ServiceBusinessId, int officerStepId)
        {
            new EmployeeMaster(util).UpdateParserDetail(BaseKeyword, value, ServiceBusinessId, officerStepId);
        }
    }
}
