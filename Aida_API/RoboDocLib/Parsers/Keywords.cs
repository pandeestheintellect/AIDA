using RoboDocLib.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Parsers
{
    public class Keywords
    {
        public Dictionary<string, KeywordParser> KeywordParser { get; set; }

        public Keywords()
        {
            KeywordParser = new Dictionary<string, KeywordParser>();
            KeywordParser.Add("P:Approved", new EmployeeParser() { BaseKeyword = "P:Approved" });

            KeywordParser.Add("P:Preparation", new EmployeeParser() { BaseKeyword = "P:Preparation" });
            KeywordParser.Add("P:Sign-Authority-1", new SignAuthority() { BaseKeyword = "P:Sign-Authority-1" });
            KeywordParser.Add("P:Sign-Authority-2", new SignAuthority() { BaseKeyword = "P:Sign-Authority-2" });
            KeywordParser.Add("P:Sign-Authority-3", new SignAuthority() { BaseKeyword = "P:Sign-Authority-3" });
            KeywordParser.Add("P:Shareholder", new Executor() { BaseKeyword = "P:Shareholder" });
            KeywordParser.Add("P:Shareholder-1", new Executor() { BaseKeyword = "P:Shareholder-1" });
            KeywordParser.Add("P:Shareholder-2", new Executor() { BaseKeyword = "P:Shareholder-2" });
            KeywordParser.Add("P:Shareholder-3", new Executor() { BaseKeyword = "P:Shareholder-3" });
            KeywordParser.Add("P:Shareholder-4", new Executor() { BaseKeyword = "P:Shareholder-4" });
            KeywordParser.Add("P:Director-1", new Executor() { BaseKeyword = "P:Director-1" });
            KeywordParser.Add("P:Director-2", new Executor() { BaseKeyword = "P:Director-2" });
            KeywordParser.Add("P:Director-3", new Executor() { BaseKeyword = "P:Director-3" });
            KeywordParser.Add("P:Director-4", new Executor() { BaseKeyword = "P:Director-4" });
            KeywordParser.Add("P:Secretary", new Executor() { BaseKeyword = "P:Secretary" });
            KeywordParser.Add("P:Secretary-1", new Executor() { BaseKeyword = "P:Secretary-1" });
            KeywordParser.Add("P:Secretary-2", new Executor() { BaseKeyword = "P:Secretary-2" });
            KeywordParser.Add("P:Secretary-3", new Executor() { BaseKeyword = "P:Secretary-3" });
            KeywordParser.Add("P:Secretary-4", new Executor() { BaseKeyword = "P:Secretary-4" });
            KeywordParser.Add("P:Chairman", new Executor() { BaseKeyword = "P:Chairman" });
            KeywordParser.Add("P:Auditor", new Executor() { BaseKeyword = "P:Auditor" });
            KeywordParser.Add("P:Partner", new Executor() { BaseKeyword = "P:Partner" });
            KeywordParser.Add("P:PrecedentPartner", new Executor() { BaseKeyword = "P:PrecedentPartner" });

            KeywordParser.Add("P:Identity-Executor-Type", new IdentityType() { BaseKeyword = "P:Identity-Executor-Type" });
            KeywordParser.Add("P:Identity-Shareholder-Type", new IdentityType() { BaseKeyword = "P:Identity-Shareholder-Type" });
            KeywordParser.Add("P:Identity-Partner-Type", new NonExecutorIndntityType() { BaseKeyword = "P:Identity-Partner-Type" });
            KeywordParser.Add("P:Identity-UltimateOwner-Type", new IdentityType() { BaseKeyword = "P:Identity-UltimateOwner-Type" });
            KeywordParser.Add("P:Shareholder-Type", new ShareholderType() { BaseKeyword = "P:Shareholder-Type" });
            KeywordParser.Add("P:Transferor", new TransferExecutor() { BaseKeyword = "P:Transferor" });
            KeywordParser.Add("P:Transferee", new TransferExecutor() { BaseKeyword = "P:Transferee" });
            KeywordParser.Add("P:UltimateOwner", new TransferExecutor() { BaseKeyword = "P:UltimateOwner" });

            KeywordParser.Add("C:Dividend-Paid", new CSSActiveHide() { BaseKeyword = "Dividend-Paid" });
            KeywordParser.Add("P:AGM-AR-Transfer-Status", new SelectedUpdate() { BaseKeyword = "P:AGM-AR-Transfer-Status" });
            KeywordParser.Add("P:AGM-AR-Transfer-Status1", new SelectedUpdate() { BaseKeyword = "P:AGM-AR-Transfer-Status1" });
            KeywordParser.Add("P:AGM-AR-Transfer-Date", new SelectedUpdate() { BaseKeyword = "P:AGM-AR-Transfer-Date" });
            KeywordParser.Add("P:AGM-AR-Balance-Presentation", new SelectedUpdate() { BaseKeyword = "P:AGM-AR-Balance-Presentation" });

            KeywordParser.Add("ExecutorAll-Director", new Executor() { BaseKeyword = "ExecutorAll-Director" });
            KeywordParser.Add("ExecutorAll-Shareholder", new Executor() { BaseKeyword = "ExecutorAll-Shareholder" });
            KeywordParser.Add("ExecutorAll-Secretary", new Executor() { BaseKeyword = "ExecutorAll-Secretary" });
            KeywordParser.Add("ExecutorAll-All", new Executor() { BaseKeyword = "ExecutorAll-All" });

        }
        public string GetColumnName(string originalName)
        {
            if (originalName.Equals("NRICOrPassNo"))
                return "isnull(NRICNo,isnull(PassportNo,isnull(FiNNo,' ')))";
            else
                return originalName;
           

        }
        public string GetControlValues(string key, string value)
        {
            if (key.StartsWith("C:") && value.Equals("false"))
                return "";
            else if ((key.StartsWith("D:") || key.StartsWith("DM:")) && value != null)
            {
                if (value.Trim().Length > 0)
                {

                    //DateTime dDate = Convert.ToDateTime(value);
                    DateTime dDate = DateTime.Parse(value);
                    return String.Format("{0:dd/MM/yyyy}", dDate).Replace('-', '/');

                }
                else
                    return "";
            }
            else
                return value;
        }

    }
}
