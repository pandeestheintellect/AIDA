using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class BusinessProfileMaster
    {
        string connectionString = "";
        int UserId = 99;
        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public BusinessProfileMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }


        public ResponseModel PostBusinessProfile(BusinessProfileModel businessProfile)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"insert into BusinessProfile(Name,FormerName,CreatedDate,UpdatedBy,UEN,IncorpDate,Address1,Address2,City,Country,Pincode, " +
                                " Mobile,Email,IndustryType,Status,StatusDate,IssuedCapital,IssuedShares,IssuedCurrency,IssuedShareType, " +
                                " PaidupCapital,PaidupShares,PaidupCurrency,PaidupShareType,TradingName,Phone,Nature,BusinessType  )" +
                                "VALUES (@Name,@FormerName,Getdate(),@UserId,@UEN,@IncorpDate,@Address1,@Address2,@City,@Country,@Pincode,@Mobile,@Email, " +
                                " @IndustryType,@Status,@StatusDate,@IssuedCapital,@IssuedShares,@IssuedCurrency,@IssuedShareType," +
                                " @PaidupCapital,@PaidupShares,@PaidupCurrency,@PaidupShareType,@TradingName,@Phone,@Nature,@ClientType)";

                var result = db.Execute(sqlQuery, new
                {
                    businessProfile.Name,
                    businessProfile.FormerName,
                    businessProfile.UEN,
                    businessProfile.IncorpDate,
                    businessProfile.Address1,
                    businessProfile.Address2,
                    businessProfile.City,
                    businessProfile.Country,
                    businessProfile.Pincode,
                    businessProfile.Mobile,
                    businessProfile.Email,
                    businessProfile.IndustryType,
                    businessProfile.Status,
                    businessProfile.StatusDate,

                    businessProfile.IssuedCapital,
                    businessProfile.IssuedShares,
                    businessProfile.IssuedCurrency,
                    businessProfile.IssuedShareType,
                    businessProfile.PaidupCapital,
                    businessProfile.PaidupShares,
                    businessProfile.PaidupCurrency,
                    businessProfile.PaidupShareType,
                    businessProfile.TradingName,
                    businessProfile.Phone,
                    businessProfile.Nature,
                    businessProfile.ClientType,
                    UserId

                });
                sqlQuery = @"select max(id) from BusinessProfile";
                int id = db.QuerySingle<int>(sqlQuery);
                new BusinessOfficerMaster(Util).PostBusinessOfficer(
                    new BusinessOfficerModel() {BusinessProfileId=id, Name="Authorised Representative",UserRole="Authorised Representative" });
                response.IsSuccess = true;
                response.Message = "Business Profile added";

                logger.Info(Util.ClientIP + "|" + "New Client Profile added");

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessProfile));
            }
            return response;
        }

        public ResponseModel PutBusinessProfile(BusinessProfileModel businessProfile)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update BusinessProfile set Name=@Name,FormerName=@FormerName,UEN=@UEN,IncorpDate=@IncorpDate,Address1=@Address1,IndustryType=@IndustryType, " +
                                    " Status = @Status,StatusDate = @StatusDate,IssuedCapital = @IssuedCapital,IssuedShares = @IssuedShares,IssuedCurrency = @IssuedCurrency," +
                                    "IssuedShareType = @IssuedShareType, " +
                                    " Address2=@Address2,City=@City,Country=@Country,Pincode=@Pincode,Mobile=@Mobile,Email=@Email,UpdatedDate=Getdate(), " +
                                    " PaidupCapital=@PaidupCapital,PaidupShares=@PaidupShares,PaidupCurrency=@PaidupCurrency,PaidupShareType=@PaidupShareType, "+
                                    " TradingName=@TradingName,Phone=@Phone,Nature=@Nature,BusinessType=@ClientType where Id=@Id";

                var result = db.Execute(sqlQuery, new
                {
                    businessProfile.Name,
                    businessProfile.FormerName,
                    businessProfile.UEN,
                    businessProfile.IncorpDate,
                    businessProfile.Address1,
                    businessProfile.Address2,
                    businessProfile.City,
                    businessProfile.Country,
                    businessProfile.Pincode,
                    businessProfile.Mobile,
                    businessProfile.Email,
                    businessProfile.IndustryType,
                    businessProfile.Status,
                    businessProfile.StatusDate,
                    businessProfile.IssuedCapital,
                    businessProfile.IssuedShares,
                    businessProfile.IssuedCurrency,
                    businessProfile.IssuedShareType,
                    businessProfile.PaidupCapital,
                    businessProfile.PaidupShares,
                    businessProfile.PaidupCurrency,
                    businessProfile.PaidupShareType,
                    businessProfile.TradingName,
                    businessProfile.Phone,
                    businessProfile.Nature,
                    businessProfile.ClientType,
                    UserId,
                    businessProfile.Id
                });
                response.IsSuccess = true;
                response.Message = "Business Profile modified";

                logger.Info(Util.ClientIP + "|" + "Client Profile updated for Profile ID " + businessProfile.Id);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessProfile));
            }
            return response;
        }

        public ResponseModel DeleteBusinessProfile(int Id)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete BusinessProfile where Id = @Id";
                var result = db.Execute(sqlQuery, new { Id });
                new BusinessOfficerMaster(Util).DeleteAllBusinessOfficer(Id);
                response.IsSuccess = true;
                response.Message = "Business Profile deleted";

                logger.Info(Util.ClientIP + "|" + "Client Profile deleted with Profile ID " + Id);

            }
            return response;
        }

        public List<BusinessProfileModel> GetBusinessProfile()
        {
            List<BusinessProfileModel> response = new List<BusinessProfileModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Id,Name,FormerName,UEN,convert(varchar,IncorpDate,105) IncorpDate,Address1,Address2,City,Country,Pincode, " +
                    " Mobile,Email,IndustryType,Status,convert(varchar,StatusDate,105) StatusDate, " +
                    " IssuedCapital,IssuedShares,IssuedCurrency,IssuedShareType, " +
                    " PaidupCapital,PaidupShares,PaidupCurrency,PaidupShareType, TradingName,Phone,Nature," +
                    " isnull(BusinessType,'PRIVATE LIMITED COMPANY') as ClientType  from BusinessProfile order by Name";
                response = db.Query<BusinessProfileModel>(sqlQuery).AsList<BusinessProfileModel>();
            }
            return response;
        }

        public BusinessProfileModel GetBusinessProfile(int Id)
        {
            BusinessProfileModel response = new BusinessProfileModel();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Id,Name,FormerName,UEN,convert(varchar,IncorpDate,105) IncorpDate,Address1,Address2,City,Country,Pincode, " +
                    " Mobile,Email,IndustryType,Status,convert(varchar,StatusDate,105) StatusDate, " +
                    " IssuedCapital,IssuedShares,IssuedCurrency,IssuedShareType, " +
                    " PaidupCapital,PaidupShares,PaidupCurrency,PaidupShareType, TradingName,Phone,Nature," +
                    " isnull(BusinessType,'PRIVATE LIMITED COMPANY') as ClientType  " +
                    " from BusinessProfile where Id=@Id";
                response = db.QuerySingle<BusinessProfileModel>(sqlQuery,new { Id});
            }
            return response;
        }
        public void UpdateParserDetail(int serviceBusinessId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select sbf.Keyword  from ServiceBusinessFields sbf " +
                        " where sbf.Keyword like 'Entity-%' and ServiceBusinessId=@ServiceBusinessId";

                List<string> keywords = db.Query<string>(sqlQuery, new { serviceBusinessId}).AsList<string>();
                if (keywords.Count > 0)
                {
                    string Column = "";
                    foreach (string keyword in keywords)
                    {
                        Column = keyword.Replace("Entity-", "");
                        if (Column.EndsWith("Date"))
                            Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";
                        else if (Column.Equals("Address1"))
                            Column = "Address1 +', '+country+' '+pincode";
                        sqlQuery = " update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column + " from  " +
                                " (select sb.Id ServiceBusinessId,bf.* from BusinessProfile bf join ServiceBusiness sb on bf.Id = sb.BusinessProfileId " +
                                " where sb.id=@ServiceBusinessId) bo " +
                                " where ServiceBusinessFields.ServiceBusinessId = bo.ServiceBusinessId  and  Keyword=@keyword";


                        db.Execute(sqlQuery, new
                        {
                            serviceBusinessId,
                            keyword
                        });

                    }
                }
            }
        }
    }
}
