using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class EntityShareholderMaster
    {
        string connectionString = "";
        string UserId = "999";

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public EntityShareholderMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }


        public ResponseModel PostEntityShareholder(EntityShareholderModel entityShareholder)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"insert into EntityShareholder(BusinessProfileId,Name,FormerName,TradingName,UEN,Address,Country,IncorpDate,Phone,Email,Nature,Status, " +
								" RepresentativeId ,CreatedDate,UpdatedBy)" +
                                " VALUES (@BusinessProfileId,@Name,@FormerName,@TradingName,@UEN,@Address,@Country,@IncorpDate,@Phone,@Email,@Nature,@Status, " +
                                " @RepresentativeId ,Getdate(),@UserId)";

                var result = db.Execute(sqlQuery, new
                {
                    entityShareholder.BusinessProfileId,
                    entityShareholder.Name,
                    entityShareholder.FormerName,
                    entityShareholder.TradingName,
                    entityShareholder.UEN,
                    entityShareholder.IncorpDate,
                    entityShareholder.Address,
                    entityShareholder.Country,
                    entityShareholder.Email,
                    entityShareholder.Status,
                    entityShareholder.Phone,
                    entityShareholder.Nature,
                    entityShareholder.RepresentativeId,
                    UserId

                });
                response.IsSuccess = true;
                response.Message = "Entity Shareholder Profile added";

                logger.Info(Util.ClientIP + "|" + "Entity shareholder details added for Profile ID" + entityShareholder.BusinessProfileId);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(entityShareholder));
              
            }
            return response;
        }

        public ResponseModel PutEntityShareholder(EntityShareholderModel entityShareholder)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update EntityShareholder set Name=@Name,FormerName=@FormerName,UEN=@UEN,IncorpDate=@IncorpDate,Address=@Address," +
                                    " Status = @Status,Country=@Country,Email=@Email,UpdatedDate=Getdate(), " +
                                    " TradingName=@TradingName,Phone=@Phone,Nature=@Nature,RepresentativeId=@RepresentativeId where Id=@Id";

                var result = db.Execute(sqlQuery, new
                {
                    entityShareholder.Name,
                    entityShareholder.FormerName,
                    entityShareholder.TradingName,
                    entityShareholder.UEN,
                    entityShareholder.IncorpDate,
                    entityShareholder.Address,
                    entityShareholder.Country,
                    entityShareholder.Email,
                    entityShareholder.Status,
                    entityShareholder.Phone,
                    entityShareholder.Nature,
                    entityShareholder.RepresentativeId,
                    UserId,
                    entityShareholder.Id
                });
                response.IsSuccess = true;
                response.Message = "Entity Shareholder Profile modified";

                logger.Info(Util.ClientIP + "|" + "Entity shareholder details updated for shareholder id " + entityShareholder.Id);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(entityShareholder));

            }
            return response;
        }

        public ResponseModel DeleteEntityShareholder(int Id)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete EntityShareholder where Id = @Id";
                var result = db.Execute(sqlQuery, new { Id });
                response.IsSuccess = true;
                response.Message = "Entity Shareholder Profile deleted";

                logger.Info(Util.ClientIP + "|" + "Entity shareholder details deleted for shareholder id " + Id);

            }
            return response;
        }

        public List<EntityShareholderModel> GetEntityShareholder(int businessProfileId)
        {
            List<EntityShareholderModel> response = new List<EntityShareholderModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select Id,a.Name,FormerName,UEN,convert(varchar,IncorpDate,105) IncorpDate,a.Address,Country, " +
                                    " a.Email,Status, TradingName,a.Phone,Nature,RepresentativeId,isnull(b.Name,'') RepresentativeName from EntityShareholder a  " +
                                    " left join BusinessOfficer b on a.RepresentativeId=b.officerid where a.BusinessProfileId = @BusinessProfileId order by Name";
                response = db.Query<EntityShareholderModel>(sqlQuery, new { businessProfileId }).AsList<EntityShareholderModel>();
            }
            return response;
        }

        public List<DropDownModel> GetRepresentative(int businessProfileId)
        {
            List<DropDownModel> result = null;
            string sql = "select OfficerId Value,Name Text from BusinessOfficer where EntityType='Entity' " +
                " and  BusinessProfileId = @BusinessProfileId order by Name";
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<DropDownModel>(sql, new { businessProfileId }).AsList<DropDownModel>();
            }
            return result;
        }

    }
}
