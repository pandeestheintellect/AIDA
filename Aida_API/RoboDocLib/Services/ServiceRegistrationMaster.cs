using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class ServiceRegistrationMaster
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceRegistrationMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;
            
        }

        public List<ServiceRegistrationDisplayModel> GetServiceRegistrationById(int serviceBusinessId)
        {
            List<ServiceRegistrationDisplayModel> response = new List<ServiceRegistrationDisplayModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = " select distinct sd.Name ServiceName,bp.Name BusinessProfileName, " +
                        " bo.Name OfficerName,sbo.Executor, sd.Remarks, Sb.Status ,bo.OfficerId ,sbo.ServiceBusinessId " +
                        " from ServicesDefinition sd join ServiceBusiness sb on sd.Code = sb.ServiceCode " +
                        " join ServiceBusinessOfficer sbo on sb.Id = sbo.serviceBusinessId join BusinessOfficer bo on sbo.OfficerId=bo.OfficerId  " +
                        " join BusinessProfile bp on sbo.BusinessProfileId=bp.Id where sb.Id=@serviceBusinessId";
                sqlQuery = sqlQuery + "   order by sd.Name, bp.Name,sbo.Executor, bo.Name ";

                response = db.Query<ServiceRegistrationDisplayModel>(sqlQuery,
                    new { serviceBusinessId }).AsList<ServiceRegistrationDisplayModel>();
            }
            return response;
        }


        public List<ServiceRegistrationDisplayModel> GetServiceRegistration(string serviceCode, int businessProfileId, string status)
        {
            List<ServiceRegistrationDisplayModel> response = new List<ServiceRegistrationDisplayModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = " select distinct convert(varchar,sb.CreatedDate,103) Created, sd.Name ServiceName,bp.Name BusinessProfileName, " +
                        " bo.Name OfficerName,sbo.Executor, sd.Remarks, Sb.Status ,bo.OfficerId ,sbo.ServiceBusinessId,sb.CreatedDate,isnull(sbo.DownloadedFileName,'NotDownloaded')DownloadedFileName " +
                        " from ServicesDefinition sd join ServiceBusiness sb on sd.Code = sb.ServiceCode " +
                        " join ServiceBusinessOfficer sbo on sb.Id = sbo.serviceBusinessId join BusinessOfficer bo on sbo.OfficerId=bo.OfficerId  " +
                        " join BusinessProfile bp on sbo.BusinessProfileId=bp.Id where 1=1 ";

                if (!serviceCode.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.ServiceCode = @ServiceCode";
                if (businessProfileId > 0)
                    sqlQuery = sqlQuery + " and sb.BusinessProfileId=@BusinessProfileId";

                if (!status.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.status=@Status";

                sqlQuery = sqlQuery + "   order by sb.CreatedDate desc,sd.Name, bp.Name,sbo.Executor, bo.Name ";

                response = db.Query<ServiceRegistrationDisplayModel>(sqlQuery,
                    new { serviceCode, businessProfileId, status }).AsList<ServiceRegistrationDisplayModel>();
            }
            return response;
        }

        public List<ServiceRegistrationClientDisplayModel> GetServiceRegistrationForDateRange(string serviceCode, int businessProfileId, string status, 
                    string startDate,string endDate, string entity)
        {
            List<ServiceRegistrationClientDisplayModel> response = new List<ServiceRegistrationClientDisplayModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = " select sb.Id ServiceBusinessId, bp.Id, bp.Name BusinessProfileName, bp.UEN, " +
                " convert(varchar,sb.CreatedDate,103) GeneratedDate,sb.status from BusinessProfile bp " +
                " join ServiceBusiness sb on bp.Id = sb.BusinessProfileId where "+
                " sb.createdDate>=@startDate and sb.createdDate<=@endDate ";
                /*
                if (!serviceCode.Equals("A") && businessProfileId > 0)
                    sqlQuery = sqlQuery + " and sb.BusinessProfileId=@BusinessProfileId";
                else if (businessProfileId > 0)
                    sqlQuery = sqlQuery + " sb.BusinessProfileId=@BusinessProfileId";

                if ((!serviceCode.Equals("A") || businessProfileId > 0) && !status.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.status=@Status";
                else if (!status.Equals("A"))
                    sqlQuery = sqlQuery + " sb.status=@Status";
                */

                if (businessProfileId > 0)
                    sqlQuery = sqlQuery + " and sb.BusinessProfileId=@BusinessProfileId";
                if (status.Equals("Finished"))
                    sqlQuery = sqlQuery + " and sb.status in ('Completed','Terminated') ";
                else if (!status.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.status=@Status";
                if (!serviceCode.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.ServiceCode = @serviceCode";
                if (!entity.Equals("A"))
                    sqlQuery = sqlQuery + " and isnull(bp.BusinessType,'PRIVATE LIMITED COMPANY') = @entity";
                sqlQuery = sqlQuery + "   order by 1,3";

                response = db.Query<ServiceRegistrationClientDisplayModel>(sqlQuery,
                    new { serviceCode, businessProfileId, status, startDate,endDate, entity }).AsList<ServiceRegistrationClientDisplayModel>();
            }
            return response;
        }
        
        public List<ServiceRegistrationDisplayModel> GetServiceRegistrationForOfficer(int serviceBusinessId, int officerId)
        {
            List<ServiceRegistrationDisplayModel> response = new List<ServiceRegistrationDisplayModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = " select sbo.ServiceBusinessId,sbo.Id OfficerStepId, sbo.StepNo, sd.Code ServiceCode, sd.Name ServiceName, " +
                    " doc.Name DocumentName, bo.Name OfficerName ,bo.UserRole OfficerRole,bp.Name BusinessProfileName, " +
                    " sbo.Status,isnull(sbo.GeneratedFileName,'NotGenerated')GeneratedFileName,isnull(sbo.DownloadedFileName,'NotDownloaded')DownloadedFileName from ServicesDefinition sd " +
                    " join ServiceBusiness sb on sd.Code = sb.ServiceCode join ServiceBusinessOfficer sbo on sb.Id = sbo.ServiceBusinessId " +
                    " join DocumentMaster doc on sbo.DocumentCode = doc.Code join BusinessProfile bp on sbo.BusinessProfileId=bp.Id " +
                    " join BusinessOfficer bo on bp.Id=bo.BusinessProfileId and sbo.OfficerId=bo.OfficerId " +
                    " where sb.Id=@ServiceBusinessId and sbo.OfficerId = @OfficerId order by sbo.StepNo ";
                response = db.Query<ServiceRegistrationDisplayModel>(sqlQuery,
                    new { serviceBusinessId, officerId }).AsList<ServiceRegistrationDisplayModel>();
            }
            return response;
        }

        public List<ServiceRegistrationViewModel> GetServiceRegistrationForClient(int businessProfileId)
        {
            List<ServiceRegistrationViewModel> response = new List<ServiceRegistrationViewModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = " select distinct top 6 convert(varchar,sb.CreatedDate,103) created,sd.Name ServiceName, " +
                                  "       bo.Name OfficerName, sb.status,sd.Code ServiceCode, sb.Id ServiceBusinessId,  " +
                                  "         sb.BusinessProfileId,sb.CreatedDate from ServicesDefinition sd " +
                                  "   join ServiceBusiness sb on sd.Code = sb.ServiceCode " +
                                  "   join ServiceBusinessOfficer sbo on sb.Id = sbo.ServiceBusinessId " +
                                  "   join BusinessOfficer bo on sbo.OfficerId = bo.OfficerId " +
                                  "   where sb.status <> 'Terminated' and sb.BusinessProfileId = @businessProfileId " +
                                  "   order by sb.CreatedDate desc ";
                response = db.Query<ServiceRegistrationViewModel>(sqlQuery,
                    new { businessProfileId }).AsList<ServiceRegistrationViewModel>();
            }
            return response;
        }

        public ResponseModel PutServiceRegistration(ServiceRegistrationModel services)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select count(1) No from ServiceBusiness where status ='New' and ServiceCode = @ServiceCode and BusinessProfileId=@BusinessProfileId  ";

                int count = db.QuerySingle<int>(sqlQuery, new { services.ServiceCode, services.BusinessProfileId, UserId });
                if (count == 0)
                {
                    sqlQuery = "insert into ServiceBusiness(CreatedDate,CreatedBy,ServiceCode,BusinessProfileId,Status) " +
                                    "select distinct getdate() CreatedDate, @UserId CreatedBy, sop.ServiceCode,bo.BusinessProfileId,'New' Status from ServiceSOP sop " +
                                    "join BusinessOfficer bo on sop.Executor = bo.UserRole  " +
                                    "join BusinessProfile bp on bo.BusinessProfileId = bp.Id  " +
                                    "where sop.ServiceCode = @ServiceCode and bp.Id =@BusinessProfileId ";

                    var dbreturn = db.Execute(sqlQuery, new { services.ServiceCode, services.BusinessProfileId, UserId });
                    
                    if (dbreturn==0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Services not created, mismatch in SOP and Officers mapping.";
                        return response;
                    }
                    if (services.StepIds.Length == 0)
                    {
                        sqlQuery = " insert into ServiceBusinessOfficer(ServiceBusinessId,BusinessProfileId,StepNo,Executor,DocumentCode,OfficerId,status,CreatedDate) " +
                                " select sb.Id ServiceBusinessId,sb.BusinessProfileId, sop.StepNo,sop.Executor,sop.DocumentCode,  " +
                                " bo.OfficerId,'New',Getdate() from ServiceSOP sop  join BusinessOfficer bo on sop.Executor = bo.UserRole  " +
                                " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId and sb.ServiceCode = sop.ServiceCode " +
                                " where sb.Status='New' and sb.ServiceCode = @ServiceCode and sb.BusinessProfileId=@BusinessProfileId ";


                        db.Execute(sqlQuery, new { services.ServiceCode, services.BusinessProfileId, UserId });

                    }
                    else
                    {
                        sqlQuery = " insert into ServiceBusinessOfficer(ServiceBusinessId,BusinessProfileId,StepNo,Executor,DocumentCode,OfficerId,status,CreatedDate) " +
                                " select sb.Id ServiceBusinessId,sb.BusinessProfileId, sop.StepNo,sop.Executor,sop.DocumentCode,  " +
                                " bo.OfficerId,'New',GetDate() from ServiceSOP sop  join BusinessOfficer bo on sop.Executor = bo.UserRole  " +
                                " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId and sb.ServiceCode = sop.ServiceCode " +
                                " where sb.Status='New' and sb.ServiceCode = @ServiceCode and sb.BusinessProfileId=@BusinessProfileId and sop.StepId=@stepId";

                        List<string> stepIds = new List<string>(services.StepIds.Split(','));
                        foreach (string stepId in stepIds)
                            db.Execute(sqlQuery, new { services.ServiceCode, services.BusinessProfileId, UserId, stepId });
                    }

                    sqlQuery = " insert into ServiceBusinessFields(ServiceBusinessId,OfficerStepId,Keyword) " +
                                " select Sbo.ServiceBusinessId,Sbo.Id ServiceBusinessOfficerId,df.Keyword from ServiceBusiness sb " +
                                " join ServiceBusinessOfficer sbo  on sb.Id =sbo.ServiceBusinessId join DocumentFields df on sbo.DocumentCode = df.Code  " +
                                " where sb.Status='New' and sb.ServiceCode = @ServiceCode and sb.BusinessProfileId=@BusinessProfileId ";

                    db.Execute(sqlQuery, new { services.ServiceCode, services.BusinessProfileId, UserId });

                    
                    response.IsSuccess = true;
                    response.Message = "New services registered.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "New services registered already.";
                }

            }


            logger.Info(Util.ClientIP + "|" + "New services registered.  and response is " + response.Message);

            logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(services));

            

            return response;
        }
        public ResponseModel DeleteRegistration(int serviceBusinessId)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ServiceBusiness set status='Terminated' where id=@serviceBusinessId";
                var result = db.Execute(sqlQuery, new { serviceBusinessId });
                response.IsSuccess = true;
                response.Message = "Registration terminated";
            }

            logger.Info(Util.ClientIP + "|" + " services terminated for Service Business ID " + serviceBusinessId);

            
            return response;
        }
        public List<DocumentModel> GetServiceDocument(string serviceCode)
        {
            List<DocumentModel> response = new List<DocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select dm.Code,dm.Name,FilePath,FileName,convert(varchar,EffectiveDate,105) EffectiveDate,VersionNo,Status "+
                                " from ServicesDefinition sd " +
                                " join ServiceDocument sdoc on sd.Code = sdoc.ServiceCode  " +
                                " join DocumentMaster dm on dm.Code = sdoc.DocumentCode  " +
                                " where HasOptionalDocument is not null and sd.Code=@serviceCode ";
                response = db.Query<DocumentModel>(sqlQuery, new { serviceCode }).AsList<DocumentModel>();
            }
            return response;
        }

        public List<ServiceSummaryResponseModel> GetServiceSummary(ServiceSummaryRequestModel serviceSummary)
        {
            List<ServiceSummaryResponseModel> response = new List<ServiceSummaryResponseModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "  select distinct bp.Id BusinessProfileId, bp.Name BusinessProfileName, bp.UEN, " +
                         " convert(varchar, bp.IncorpDate, 103) IncorpDate, " +
                         " sd.Name ServiceName,sb.ServiceCode, " +
                         " convert(varchar, sb.CreatedDate, 103) CreatedDate,sb.status,so.ServiceBusinessId from BusinessProfile bp " +
                         " join ServiceBusiness sb on bp.Id = sb.BusinessProfileId " +
                         " join ServiceBusinessOfficer so on sb.Id = so.ServiceBusinessId " +
                         " join BusinessOfficer bo on so.OfficerId = bo.OfficerId " +
                         " join ServicesDefinition sd on sb.ServiceCode = sd.Code " +
                         " join DocumentMaster dm on so.DocumentCode = dm.Code where 1=1 ";
             
                if (serviceSummary.BusinessProfileId > 0)
                    sqlQuery = sqlQuery + " and bp.BusinessProfileId="+ serviceSummary.BusinessProfileId;
                if (!serviceSummary.Status.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.status = ='" + serviceSummary.Status + "'";
                if (!serviceSummary.Uen.Equals("A"))
                    sqlQuery = sqlQuery + " and bp.UEN ='"+ serviceSummary.Uen+"'" ;
                if (!serviceSummary.IncorpDate.Equals("A"))
                    sqlQuery = sqlQuery + " and bp.IncorpDate ='" + serviceSummary.IncorpDate + "'";
                if (!serviceSummary.OfficerName.Equals("A"))
                    sqlQuery = sqlQuery + " and bo.Name like '" + serviceSummary.OfficerName + "%'";
                if (!serviceSummary.ServiceCode.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.ServiceCode ='" + serviceSummary.ServiceCode + "'";
                if (!serviceSummary.DocumentName.Equals("A"))
                    sqlQuery = sqlQuery + " and dm.name like '" + serviceSummary.DocumentName + "%'";
                if (!serviceSummary.StartDate.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.CreatedDate>='" + serviceSummary.StartDate + "'";
                if (!serviceSummary.EndDate.Equals("A"))
                    sqlQuery = sqlQuery + " and sb.CreatedDate<='" + serviceSummary.EndDate + "'";

                sqlQuery = sqlQuery + "   order by 1,3";

                response = db.Query<ServiceSummaryResponseModel>(sqlQuery).AsList<ServiceSummaryResponseModel>();
            }
            return response;
        }
    }
}
