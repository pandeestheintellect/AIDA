using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class CompanyMaster
    {
        string connectionString = "";
        int UserId = 999;
        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public CompanyMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }

        public List<CompanyModel> GetCompany()
        {
            List<CompanyModel> response = new List<CompanyModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select CompanyId,CompanyName, CompanyUEN, isnull(convert(varchar,IncorpDate,105),'') IncorpDate, Address1, Address2, City, Country, " +
                                    " Pincode,Phone,Mobile,Email,Fax,GstRegNo,IndustryType,Status,convert(varchar,StatusDate,105) StatusDate from CompanyMaster order by CompanyName";
                response = db.Query<CompanyModel>(sqlQuery).AsList<CompanyModel>();
            }
            return response;
        }

        public ResponseModel PostCompany(CompanyModel company)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into CompanyMaster(CreatedDate, UserId, CompanyName, CompanyUEN, IncorpDate, Address1, Address2, City, Country, " +
                                    " Pincode,Phone,Mobile,Email,Fax,GstRegNo,IndustryType,Status,StatusDate) " +
                                    " values (getdate(),@UserId,@CompanyName,@CompanyUEN,@IncorpDate,@Address1,@Address2,@City,@Country, " +
                                    " @Pincode,@Phone,@Mobile,@Email,@Fax,@GstRegNo,@IndustryType,@Status,getdate()) ";
                var result = db.Execute(sqlQuery, new
                {
                    UserId,
                    company.CompanyName,
                    company.CompanyUEN,
                    company.IncorpDate,
                    company.Address1,
                    company.Address2,
                    company.City,
                    company.Country,
                    company.Pincode,
                    company.Phone,
                    company.Mobile,
                    company.Email,
                    company.Fax,
                    company.GstRegNo,
                    company.IndustryType,
                    company.Status,
                    company.StatusDate
                });

                response.IsSuccess = true;
                response.Message = "Company added";

                logger.Info(Util.ClientIP + "|" + "Company details added");

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(company));

            }
            return response;
        }

        public ResponseModel PutCompany(CompanyModel company)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" update CompanyMaster set UserId=@UserId,UpdatedDate=getdate(),CompanyName=@CompanyName,CompanyUEN=@CompanyUEN, " +
                                " IncorpDate = @IncorpDate,Address1 =@Address1,Address2=@Address2,City=@City, " +
                                " Country=@Country, Pincode=@Pincode,Phone=@Phone,Mobile=@Mobile,Email=@Email,Fax=@Fax, " +
                                " GstRegNo=@GstRegNo,IndustryType=@IndustryType,Status=@Status,StatusDate=@StatusDate " +
                                " where CompanyId=@CompanyId ";

                var result = db.Execute(sqlQuery, new
                {
                    UserId,
                    company.CompanyName,
                    company.CompanyUEN,
                    company.IncorpDate,
                    company.Address1,
                    company.Address2,
                    company.City,
                    company.Country,
                    company.Pincode,
                    company.Phone,
                    company.Mobile,
                    company.Email,
                    company.Fax,
                    company.GstRegNo,
                    company.IndustryType,
                    company.Status,
                    company.StatusDate,
                    company.CompanyId
                });

                response.IsSuccess = true;
                response.Message = "Company modified";

                logger.Info(Util.ClientIP + "|" + "Company details updated for company ID " + company.CompanyId);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(company));
            }
            return response;
        }

        public ResponseModel DeleteCompany(int companyId)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete CompanyMaster where companyId=@companyId";
                var result = db.Execute(sqlQuery, new { companyId });
                response.IsSuccess = true;
                response.Message = "Company deleted";

                logger.Info(Util.ClientIP + "|" + "Company details deleted with company ID " + companyId);

              

            }
            return response;
        }
    }
}
