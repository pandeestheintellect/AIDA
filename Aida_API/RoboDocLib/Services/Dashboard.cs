using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Linq;
using NLog;

namespace RoboDocLib.Services
{
    public class Dashboard
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public Dashboard(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;


        }
        public List<StatusCountModel> GetNewEnquiryStatusWise(string status)
        {
            List<StatusCountModel> response = new List<StatusCountModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery =
                    @" select Periods,count(name) Counts, " +
                    "    case when Periods = 1 then 'More than 30 days' else " +
                    "        case when Periods = 2 then '16 days to 30 days' else " +
                    "        case when Periods = 3 then '8 days to 15 days' else 'Less than 7 days' end end end DisplayName," +
                    "        min(Status) Status" +
                    " from" +
                    " (" +
                    "    select case when datediff(d, CreatedDate, getdate()) > 30 then 1 else " +
                    "            case when datediff(d, CreatedDate, getdate()) > 15 then 2 else " +
                    "            case when datediff(d, CreatedDate, getdate()) > 7 then 3 else 4 end end end periods, name, Status " +
                    "    from InitialDocumentDispatch" +
                    "    where Status = @status " +
                    " ) a " +
                    " group by periods order by periods";

                response = db.Query<StatusCountModel>(sqlQuery, new { status }).AsList<StatusCountModel>();
            }

            return response;
        }

        public List<ServiceStatusCountModel> GetServiceStatus(string status)
        {
            List<ServiceStatusCountModel> response = new List<ServiceStatusCountModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery =
                    @" select sd.Name, sum(case when Month(sb.createdDate) = Month(Getdate()) then 1 else 0 end) MonthlyCounts, "+
                    " count(1) YearlyCounts,Min(sb.ServiceCode) ServiceCode  from BusinessProfile bp " +
                    " join ServiceBusiness sb on bp.Id = sb.BusinessProfileId " +
                    " join ServicesDefinition sd on sb.ServiceCode = sd.Name " +
                    " where Year(sb.createdDate) = Year(Getdate()) ";

                if (status.Equals("Started"))
                    sqlQuery = sqlQuery + " and sb.status ='New' ";
                else if (status.Equals("Ongoing"))
                    sqlQuery = sqlQuery + " and sb.status ='In-Progress' ";
                else if (status.Equals("Finished"))
                    sqlQuery = sqlQuery + " and sb.status in ('Completed','Terminated') ";

                sqlQuery = sqlQuery + " group by sd.Name order by 1 ";

                response = db.Query<ServiceStatusCountModel>(sqlQuery, new { status }).AsList<ServiceStatusCountModel>();
            }

            return response;
        }

        public List<ServicePerformanceReportModel> GetStatusWisePeformance(string status)
        {
            List<ServicePerformanceReportModel> response = new List<ServicePerformanceReportModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                List<ServicePerformanceCountModel> detail = new List<ServicePerformanceCountModel>();
                string sqlQuery =
                    @" select Month(sb.createdDate) CreatedMonth,sd.Name,count(1) Counts, 	" +
                     " Min(DATENAME(m,sb.createdDate)) Created,Min(sd.Code) as ServiceCode " +
                     " from BusinessProfile bp join ServiceBusiness sb on bp.Id = sb.BusinessProfileId " +
                     " join ServicesDefinition sd on sb.ServiceCode = sd.Name " +
                     " where Year(sb.createdDate) = Year(Getdate())   ";
                if (status.Equals("completed"))
                    sqlQuery = sqlQuery + " and sb.status=@status";

                sqlQuery = sqlQuery + " group by Month(sb.createdDate),sd.Name order by 1,2";
                detail = db.Query<ServicePerformanceCountModel>(sqlQuery, new { status }).AsList<ServicePerformanceCountModel>();

                if (detail.Count>0)
                {
                    
                    var distinctdetail = (from z in detail
                                          orderby z.Name
                                        select z.Name).Distinct();

                    ServicePerformanceReportModel reportModel;
                    
                    foreach (var det in distinctdetail)
                    {
                        reportModel = new ServicePerformanceReportModel();
                        reportModel.Series = new List<ChartDataModel>();
                        foreach (ServicePerformanceCountModel report in detail.FindAll(o => o.Name.Equals(det)))
                        {
                            reportModel.Series.Add(new ChartDataModel() { Name = report.Created, Value = report.Counts });
                            reportModel.Name = report.Name;
                        }
                        response.Add(reportModel);
                    }
                }
            }

            return response;
        }

        public List<ServiceEntityCountModel> GetServiceStatusEntityWise()
        {
            List<ServiceEntityCountModel> response = new List<ServiceEntityCountModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery =
                    @" select Month(sb.createdDate) CreatedMonth,sd.Name, isnull(bp.BusinessType,'PRIVATE LIMITED COMPANY') ClientType, " +
                    " sb.Status, count(1) Counts,Min(DATENAME(m,sb.createdDate)) Created from BusinessProfile bp " +
                    " join ServiceBusiness sb on bp.Id = sb.BusinessProfileId join ServicesDefinition sd on sb.ServiceCode = sd.Name " +
                    " where Year(sb.createdDate) = Year(Getdate()) " +
                    "group by Month(sb.createdDate),sd.Name,isnull(bp.BusinessType,'PRIVATE LIMITED COMPANY') ,sb.status " +
                    "order by 1,2,3,4 ";

                response = db.Query<ServiceEntityCountModel>(sqlQuery).AsList<ServiceEntityCountModel>();
            }

            return response;
        }

        public List<ActiveProfileModel> GetActiveProfile()
        {
            List<ActiveProfileModel> response = new List<ActiveProfileModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery =
                    @" select top 5 sb.BusinessProfileId,Min(bp.Name)Name,count(1) Counts " + 
                    " from BusinessProfile bp join ServiceBusiness sb on bp.Id = sb.BusinessProfileId " +
                    " where sb.status='In-Progress' group by sb.BusinessProfileId order by 2";

                response = db.Query<ActiveProfileModel>(sqlQuery).AsList<ActiveProfileModel>();
            }

            return response;
        }

        public List<EventInfoModel> GetEvents(string eventDate)
        {
            List<EventInfoModel> response = new List<EventInfoModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery =
                    @" select 'Birthday' EventName, FirstName Name,'Employee' Entity,convert(varchar,BirthDate,103) EventDate," +
                    " Mobile,isnull(Email,'NA') Email  from EmployeeMaster " +
                    " where DATEPART(d, BirthDate) = DATEPART(d, @eventDate) AND DATEPART(m, BirthDate) = DATEPART(m, @eventDate)  " +
                    " union all " +
                    " select 'Birthday', bo.Name,bp.Name,convert(varchar,BirthDate,103),bo.Mobile,isnull(bo.Email,'NA') " +
                    " from BusinessOfficer bo " +
                    " join BusinessProfile bp on bo.BusinessProfileId = bp.Id " +
                    " where DATEPART(d, BirthDate) = DATEPART(d, @eventDate) AND DATEPART(m, BirthDate) = DATEPART(m, @eventDate) " +
                    " union all " +
                    " select 'Job anniversary' , bo.Name,bp.Name , convert(varchar,JoinDate,103),bo.Mobile,isnull(bo.Email,'NA')   " +
                    " from BusinessOfficer bo " +
                    " join BusinessProfile bp on bo.BusinessProfileId = bp.Id " +
                    " where DATEPART(d, JoinDate) = DATEPART(d, @eventDate) AND DATEPART(m, JoinDate) = DATEPART(m, @eventDate) " +
                    " union all " +
                    " select 'incorporation anniversary' , bp.Name,'' , convert(varchar,IncorpDate,103),Mobile,isnull(Email,'NA')  " +
                    " from BusinessProfile bp " +
                    " where DATEPART(d, IncorpDate) = DATEPART(d, @eventDate) AND DATEPART(m, IncorpDate) = DATEPART(m, @eventDate) " +
                    " order by 1,2";

                response = db.Query<EventInfoModel>(sqlQuery,new { eventDate }).AsList<EventInfoModel>();
            }

            return response;
        }

    }
}
