using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;

namespace RoboDocLib.Services
{
    public class ServiceSOPMaster
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceSOPMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;
        }

        public List<ServiceSOPModel> GetServiceSOP(string serviceCode)
        {
            List<ServiceSOPModel> response = new List<ServiceSOPModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select StepId,StepNo,Executor,DependencyStepNo,b.Name DocumentName,a.Remarks,VersionNo,FilePath " +
                        " from ServiceSOP a " +
                        " join DocumentMaster b on a.documentcode = b.code" +
                        " where serviceCode = @serviceCode order by FilePath,executor,stepno";
                response = db.Query<ServiceSOPModel>(sqlQuery, new { serviceCode }).AsList<ServiceSOPModel>();
            }
            return response;
        }

        public List<DocumentModel> GetServiceSOPSubscription(string serviceCode, string executor)
        {
            List<DocumentModel> response = new List<DocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select isnull(stepNo,999), Code,Name,FileName, Name DocumentName,VersionNo,isnull(executor,'')  Status " +
                        " from DocumentMaster dm " +
                        " join ServiceDocument sd on dm.code = sd.DocumentCode and sd.ServiceCode = @serviceCode " +
                        " left join ServiceSOP sop on DM.code = sop.documentcode  " +
                        " and sop.ServiceCode = @serviceCode and executor=@executor order by 1,Name";
                response = db.Query<DocumentModel>(sqlQuery, new { serviceCode, executor }).AsList<DocumentModel>();
            }
            return response;
        }

        public ResponseModel PutServiceSOP(string serviceCode, string executor, List<DocumentModel> documents)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete ServiceSOP where ServiceCode=@serviceCode and Executor=@executor ";
                var result = db.Execute(sqlQuery, new { serviceCode, executor });

                sqlQuery = @"insert into ServiceSOP (ServiceCode,StepNo,Executor,DocumentCode) values(@ServiceCode,@i,@Executor,@Code)";
                int i = 1;
                foreach (DocumentModel document in documents)
                {
                    db.Execute(sqlQuery, new { serviceCode, executor, i, document.Code });
                    i++;
                }

                response.IsSuccess = true;
                response.Message = "Services modified";
            }


            logger.Info(Util.ClientIP + "|" + "Services SOP modified for Service code " + serviceCode
                        + ", executor  " + executor + " and response is " + response.Message);

            return response;
        }

    }
}
