using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;

namespace RoboDocLib.Services
{
    public class BusinessActivityMaster
    {
        string connectionString = "";
        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public BusinessActivityMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;
        }

        public List<BusinessActivityModel> GetBusinessActivity(int businessProfileId)
        {
            List<BusinessActivityModel> response = new List<BusinessActivityModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select BusinessProfileId,Name,Description from BusinessActivity where BusinessProfileId=@businessProfileId order by Name";
                response = db.Query<BusinessActivityModel>(sqlQuery, new { businessProfileId }).AsList<BusinessActivityModel>();
            }
            return response;
        }

        public ResponseModel PutBusinessActivity(List<BusinessActivityModel> activities)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete BusinessActivity where BusinessProfileId=@BusinessProfileId ";

                var result = db.Execute(sqlQuery, new { activities[0].BusinessProfileId });
                if(!(activities.Count==1 && activities[0].Name.Equals("DELETE") && activities[0].Description.Equals("DELETE")))
                {
                    sqlQuery = @"insert into BusinessActivity(BusinessProfileId,Name,Description) values (@BusinessProfileId,@Name,@Description )  ";
                    result = db.Execute(sqlQuery, activities);
                }

                response.IsSuccess = true;
                response.Message = "Activities modified";
            }

            logger.Info(Util.ClientIP + "|" + "Business Activity modified for Profile ID ");
        
            return response;
        }
    }
}
