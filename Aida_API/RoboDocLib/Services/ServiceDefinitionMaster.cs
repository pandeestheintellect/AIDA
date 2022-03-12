using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class ServiceDefinitionMaster
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceDefinitionMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }

        public ResponseModel PostServiceDefinition(ServiceDefinitionModel servicesProfile)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"insert into ServicesDefinition(Code,Name,Remarks,HasOptionalDocument,UserId,CreatedDate,UpdatedDate)" +
                                        "VALUES (@Code,@Name,@Remarks,@HasOptionalDocument, 999,getdate(),getdate())";
                var result = db.Execute(sqlQuery, servicesProfile);
                response.IsSuccess = true;
                response.Message = "Services added";

                logger.Info(Util.ClientIP + "|" + "Services added");
                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(servicesProfile));

                
            }
            return response;
        }

        public ResponseModel PutServiceDefinition(ServiceDefinitionModel servicesProfile)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ServicesDefinition set Name=@Name,Remarks=@Remarks,HasOptionalDocument = @HasOptionalDocument ,UserId=@UserId,UpdatedDate=getdate()" +
                                    " where Code=@Code";

                var result = db.Execute(sqlQuery, new
                {
                    servicesProfile.Code,
                    servicesProfile.Name,
                    servicesProfile.Remarks,
                    servicesProfile.HasOptionalDocument,
                    UserId
                });
                response.IsSuccess = true;
                response.Message = "Services modified";

                logger.Info(Util.ClientIP + "|" + "Services Modified");
                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(servicesProfile));

                
            }
            return response;
        }

        public ResponseModel DeleteServiceDefinition(string serviceCode)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete ServicesDefinition where Code=@serviceCode ";
                var result = db.Execute(sqlQuery, new { serviceCode });
                response.IsSuccess = true;
                response.Message = "Services deleted";

                logger.Info(Util.ClientIP + "|" + "Services deleted with service code " + serviceCode);
                
            }
            return response;
        }

        public List<ServiceDefinitionModel> GetServiceDefinition()
        {
            List<ServiceDefinitionModel> response = new List<ServiceDefinitionModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Code,Name,Remarks,isnull(HasOptionalDocument ,'')HasOptionalDocument from ServicesDefinition order by Name";
                response = db.Query<ServiceDefinitionModel>(sqlQuery).AsList<ServiceDefinitionModel>();
            }
            return response;
        }
        
        public ServiceDefinitionModel GetServiceDefinitionForServiceCode(string serviceCode)
        {
            ServiceDefinitionModel response = new ServiceDefinitionModel();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Code,Name,Remarks,isnull(HasOptionalDocument ,'')HasOptionalDocument from ServicesDefinition where code = @serviceCode";
                response = db.QuerySingleOrDefault<ServiceDefinitionModel>(sqlQuery,new { serviceCode });
            }
            return response;
        }

        public List<DropDownModel> GetServiceDocuments(string serviceCode)
        {
            List<DropDownModel> response = new List<DropDownModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Code value,Name Text from DocumentMaster join ServiceDocument " +
                    " on Code = DocumentCode where ServiceCode = @ServiceCode order by Name";
                response = db.Query<DropDownModel>(sqlQuery,new { serviceCode }).AsList<DropDownModel>();
            }
            return response;
        }

        public ResponseModel PutServiceDocuments(List<DropDownModel> servicesDocuments)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string serviceCode = servicesDocuments.Find(doc => doc.Text == "#ServiceCode#").Value;
                
                string sqlQuery = @" Delete ServiceDocument where ServiceCode=@serviceCode";

                var result = db.Execute(sqlQuery, new{ serviceCode });

                sqlQuery = @" insert into ServiceDocument(ServiceCode,DocumentCode) values(@serviceCode,@Value) ";
                
                foreach(DropDownModel model in servicesDocuments.FindAll(doc => doc.Text != "#ServiceCode#"))
                {
                    db.Execute(sqlQuery, new { serviceCode,model.Value });
                }
                response.IsSuccess = true;
                response.Message = "Services document configured";

                logger.Info(Util.ClientIP + "|" + "Services document configured");
                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(servicesDocuments));

            }
            return response;
        }
    }
}
