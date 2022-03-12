using RoboDocLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class ExecutorAll : KeywordParser
    {
        /* BaseKeyword = ExecutorAll-Director,ExecutorAll-Shareholder */
        public ExecutorAll()
        {
            
        }
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            return null;
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            new BusinessOfficerMaster(util).UpdateExecuterAll(serviceBusinessId, officerStepId, BaseKeyword);
        }
    }
}
