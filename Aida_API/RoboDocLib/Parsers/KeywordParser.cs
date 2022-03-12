using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public abstract class KeywordParser
    {
        public string BaseKeyword { get; set; }
        public abstract List<string> GetList(ControllerUtil util, int ServiceBusinessId);
        public abstract void UpdateDetail(ControllerUtil util, string value, int ServiceBusinessId, int officerStepId);
    }
}
