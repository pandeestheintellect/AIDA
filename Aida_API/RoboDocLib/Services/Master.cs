using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;

namespace RoboDocLib.Services
{
    public class Master
    {
        string connectionString = "";

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public Master(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }
        public string GetMasterDescription(string keyword)
        {
            Dictionary<string, string> sqls = new Dictionary<string, string>();

            sqls.Add("REGISTRATION", "select Name from ServicesDefinition where code=@keyword");
            sqls.Add("POSTREGISTRATION", "select Name from ServicesDefinition where code=@keyword");
            sqls.Add("P:Identity-Shareholder-Type", "select code from IdentityType");
            string result = null;
            string sql;
            keyword = keyword.ToUpper();
            if (sqls.TryGetValue(keyword, out sql))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    result = connection.QuerySingle<string>(sql, new { keyword });
                }

            }
            return result;
        }
        public List<string> GetMasterOptions(string keyword)
        {
            string sql="";

            if (keyword.Equals("P:Identity-Executor-Type") ||keyword.Equals("P:Identity-Shareholder-Type")
                || keyword.Equals("P:Identity-UltimateOwner-Type") || keyword.Equals("P:Identity-Partner-Type"))
                sql= "select code from IdentityType";

            List<string> result = null;
            
            if (sql.Length>0)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    result = connection.Query<string>(sql, commandType: System.Data.CommandType.Text).AsList<string>();
                }

            }
            return result;
        }
        public List<DropDownModel> GetMasterDropDown(string master)
        {
            string sql = "";
            if (master.Equals("business-profile"))
                sql = "select Id Value,Name Text from BusinessProfile order by Name";
            else if (master.Equals("documents"))
                sql = "select Code Value,Name Text from documentmaster order by Name";
            else if (master.Equals("services"))
                sql = "select Code Value,Name Text from ServicesDefinition order by Name";
            else if (master.Equals("services-status"))
                sql = "select Code Value,Name Text from ServiceStatus";
            else if (master.Equals("industry-type"))
                sql = "select Code Value,Name Text from IndustryType";
            else if (master.Equals("representative"))
                sql = "select OfficerId Code,Name Text from BusinessOfficer where EntityType='Entity' order by Name";
            else if (master.Equals("acra-industry-classification"))
                sql = "select Code Value,Name Text from ACRAIndustrialClassification order by code";
            else if (master.ToLower().Equals("positions"))
                sql = "select Name Value,Name Text from Positions order by Name";
            else if (master.ToLower().Equals("roles"))
                sql = "select Name Value,Name Text from Roles order by Name";
            else if (master.ToLower().Equals("industrial-classification"))
                sql = "select Name Value,Name Text from IndustrialClassification order by Name";
            else if (master.Equals("client-status"))
                sql = "select Name Value,Name Text from ClientStatus order by Name";
            else if (master.Equals("identity-type"))
                sql = "select code Value, code Text from IdentityType order by code";
            List<DropDownModel> result = null;
            
            if (!sql.Equals(""))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    result = connection.Query<DropDownModel>(sql, commandType: System.Data.CommandType.Text).AsList<DropDownModel>();
                }

            }
            return result;
        }

        public ResponseModel AddMaster(DropDownModel values)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            string sql = "";
            if (values.Value.Equals("Positions"))
                sql = "Insert into Positions (Name) values (@text)";
            else if (values.Value.Equals("Roles"))
                sql = "Insert into Roles (Name) values (@text)";
            else if (values.Value.Equals("industrial-classification"))
                sql = "Insert into IndustrialClassification (Name) values (@text)";
            else if (values.Value.Equals("client-status"))
                sql = "Insert into ClientStatus (Name) values (@text)";

            if (!sql.Equals(""))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(sql, new { values.Text });

                    response.IsSuccess = true;
                    response.Message = "Detail added";
                }

            }
            logger.Info(Util.ClientIP + "|" + "Master added " + ", sql  " + sql + " and response is " + response.Message);
            return response;
        }

        public ResponseModel DeleteMaster(string master, string masterValue)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            string sql = "";
            if (master.Equals("Positions"))
                sql = "Delete Positions where name = @masterValue";
            else if (master.Equals("Roles"))
                sql = "Delete Roles where name = @masterValue";
            else if (master.Equals("industrial-classification"))
                sql = "Delete IndustrialClassification where name = @masterValue";
            else if (master.Equals("client-status"))
                sql = "Delete ClientStatus where name = @masterValue";
            if (!sql.Equals(""))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(sql, new { masterValue });

                    response.IsSuccess = true;
                    response.Message = "Detail deleted";
                }

            }

            logger.Info(Util.ClientIP + "|" + "Master deleted "+ ", sql  " + sql + " and response is " + response.Message);

            return response;
        }
    }
}
