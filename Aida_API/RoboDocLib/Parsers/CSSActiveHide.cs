using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace RoboDocLib.Parsers
{
    public class CSSActiveHide : KeywordParser
    {
        public override List<string> GetList(ControllerUtil util, int serviceBusinessId)
        {
            return null;
        }
        public override void UpdateDetail(ControllerUtil util, string value, int serviceBusinessId, int officerStepId)
        {
            using (IDbConnection db = new SqlConnection(util.ConnectionString))
            {
                string userRole = BaseKeyword + "-No-CSS";
                string cssYesValue = "style='display: none;'";
                string cssNoValue = "style='display: none;'";
                if (value.Equals("true"))
                {
                    cssYesValue = "";
                }
                else
                {
                    cssNoValue = "";
                }
                string sqlQuery = @"update ServiceBusinessFields set CapturedValue=@cssNoValue, UpdatedTime=GETDATE()  " +
                    "  where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@userRole";

                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    userRole,
                    cssNoValue
                });

                userRole = BaseKeyword + "-Yes-CSS";
                sqlQuery = @"update ServiceBusinessFields set CapturedValue=@cssYesValue, UpdatedTime=GETDATE()  " +
                    "  where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@userRole";

                 db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    userRole,
                    cssYesValue
                });
            }
        }
    }
}
