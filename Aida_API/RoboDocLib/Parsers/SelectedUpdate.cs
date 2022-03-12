using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;


namespace RoboDocLib.Parsers
{
    public class SelectedUpdate : KeywordParser
    {
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            List<string> list = new List<string>();

            if (BaseKeyword.Equals("P:AGM-AR-Transfer-Status"))
            {
                list.Add("have been");
                list.Add("have not");
                list.Add("have not been");

            }
            else if (BaseKeyword.Equals("P:AGM-AR-Transfer-Status1"))
            {
                list.Add("registered");
                list.Add("taken place");
            }
            else if (BaseKeyword.Equals("P:AGM-AR-Transfer-Date"))
            {
                list.Add("last main return");
                list.Add("incorporation of the company");
            }
            else if (BaseKeyword.Equals("P:AGM-AR-Balance-Presentation"))
            {
                list.Add("in the annual general meeting");
                list.Add("by way of a resolution by written means");
            }
            return list;
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            using (IDbConnection db = new SqlConnection(util.ConnectionString))
            {
                string sqlQuery = @"update ServiceBusinessFields set CapturedValue=@value, UpdatedTime=GETDATE()  " +
                    "  where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@BaseKeyword";

                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    value,
                    BaseKeyword
                });
            
            }
        }
    }
}
