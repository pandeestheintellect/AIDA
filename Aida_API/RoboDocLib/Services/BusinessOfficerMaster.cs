using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using RoboDocLib.Parsers;
using NLog;
using Newtonsoft.Json;
using System.Globalization;

namespace RoboDocLib.Services
{
    public class BusinessOfficerMaster
    {
        string connectionString = "";
        int UserId = 99;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public BusinessOfficerMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }

        public ResponseModel PostBusinessOfficer(BusinessOfficerModel businessOfficer)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into BusinessOfficer(BusinessProfileId,CreatedDate,UpdatedBy,Name,Address," +
                " Nationality,BirthDate,BirthCountry, " +
                " Position,UserRole,Email,Mobile,NricNo,NricIssueDate,FinNo,FinIssueDate,FinExpiryDate, MyInfoRequestId," +
                " PassportNo,PassportIssueDate,PassportIssuePlace,PassportExpiryDate,Sex,Phone,BirthPlace,JoinDate," +
                " PassportIssueCountry,NumberOfShares, AliasName,ResidentialStatus,Race,PassType," +
                " NricIssuePlace,NricExpiryDate,NricIssueCountry,EntityType)" +
                " values (@BusinessProfileId,Getdate(),@UserId, @Name, @Address, @Nationality, @BirthDate, @BirthCountry, " +
                " @Position, @UserRole, @Email, @Mobile, @NricNo,@NricIssueDate,@FinNo,@FinIssueDate,@FinExpiryDate," +
                " @MyInfoRequestId, @PassportNo,@PassportIssueDate,@PassportIssuePlace,@PassportExpiryDate,@Sex,@Phone," +
                " @BirthPlace,@JoinDate,@PassportIssueCountry,@NumberOfShares,@AliasName,@ResidentialStatus,@Race," +
                " @PassType,@NricIssuePlace,@NricExpiryDate,@NricIssueCountry,@EntityType)";
                var result = db.Execute(sqlQuery, new { UserId,
                    businessOfficer.BusinessProfileId,
                    businessOfficer.Name,
                    businessOfficer.Address,
                    businessOfficer.Nationality,
                    businessOfficer.BirthDate,
                    businessOfficer.BirthCountry,
                    businessOfficer.Position,
                    businessOfficer.UserRole,
                    businessOfficer.Email,
                    businessOfficer.Mobile,
                    businessOfficer.NricNo,
                    businessOfficer.NricIssueDate,
                    businessOfficer.FinNo,
                    businessOfficer.FinIssueDate,
                    businessOfficer.FinExpiryDate,
                    businessOfficer.PassportNo,
                    businessOfficer.PassportIssueDate,
                    businessOfficer.PassportIssuePlace,
                    businessOfficer.PassportExpiryDate,
                    businessOfficer.Sex,
                    businessOfficer.Phone,
                    businessOfficer.BirthPlace,
                    businessOfficer.JoinDate,
                    businessOfficer.PassportIssueCountry,
                    businessOfficer.NumberOfShares,
                    businessOfficer.AliasName,
                    businessOfficer.ResidentialStatus,
                    businessOfficer.Race,
                    businessOfficer.PassType,
                    businessOfficer.MyInfoRequestId,
                    businessOfficer.NricIssuePlace,
                    businessOfficer.NricIssueCountry,
                    businessOfficer.NricExpiryDate,
                    businessOfficer.EntityType

                });
                response.IsSuccess = true;
                response.Message = "Business Officer added";

                logger.Info(Util.ClientIP + "|" + "New Officer Creaded for Client Profile ID " + businessOfficer.BusinessProfileId);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessOfficer));
                
            }

            return response;
        }

        public ResponseModel PutBusinessOfficer(BusinessOfficerModel businessOfficer)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update BusinessOfficer set BusinessProfileId=@BusinessProfileId, Name=@Name, Address=@Address, Nationality=@Nationality," +
                                    " UpdatedBy=@UserId, UpdatedDate=Getdate(), BirthDate=@BirthDate, BirthCountry=@BirthCountry, Position=@Position, UserRole=@UserRole, " +
                                    " Email=@Email, Mobile=@Mobile,NricNo=@NricNo,NricIssueDate=@NricIssueDate,FinNo=@FinNo,FinIssueDate=@FinIssueDate,FinExpiryDate=@FinExpiryDate, " +
                                    " PassportNo=@PassportNo,PassportIssueDate=@PassportIssueDate,PassportIssuePlace=@PassportIssuePlace,PassportExpiryDate=@PassportExpiryDate, "+
                                    " Sex=@Sex,Phone=@Phone,BirthPlace=@BirthPlace,JoinDate=@JoinDate,PassportIssueCountry=@PassportIssueCountry,NumberOfShares=@NumberOfShares, " +
                                    " AliasName=@AliasName,ResidentialStatus=@ResidentialStatus,Race=@Race,PassType=@PassType, " +
                                    " NricIssuePlace=@NricIssuePlace,NricExpiryDate=@NricExpiryDate,NricIssueCountry=@NricIssueCountry," +
                                    " EntityType=@EntityType where OfficerId=@OfficerId";

                var result = db.Execute(sqlQuery, new
                {
                    UserId,
                    businessOfficer.BusinessProfileId,
                    businessOfficer.Name,
                    businessOfficer.Address,
                    businessOfficer.Nationality,
                    businessOfficer.BirthDate,
                    businessOfficer.BirthCountry,
                    businessOfficer.Position,
                    businessOfficer.UserRole,
                    businessOfficer.Email,
                    businessOfficer.Mobile,
                    businessOfficer.NricNo,
                    businessOfficer.NricIssueDate,
                    businessOfficer.FinNo,
                    businessOfficer.FinIssueDate,
                    businessOfficer.FinExpiryDate,
                    businessOfficer.PassportNo,
                    businessOfficer.PassportIssueDate,
                    businessOfficer.PassportIssuePlace,
                    businessOfficer.PassportExpiryDate,
                    businessOfficer.Sex,
                    businessOfficer.Phone,
                    businessOfficer.BirthPlace,
                    businessOfficer.JoinDate,
                    businessOfficer.PassportIssueCountry,
                    businessOfficer.NumberOfShares,
                    businessOfficer.AliasName,
                    businessOfficer.ResidentialStatus,
                    businessOfficer.Race,
                    businessOfficer.PassType,
                    businessOfficer.NricIssuePlace,
                    businessOfficer.NricIssueCountry,
                    businessOfficer.NricExpiryDate,
                    businessOfficer.EntityType,
                    businessOfficer.OfficerId
                });
                response.IsSuccess = true;
                response.Message = "Business Officer details modified";
                
                logger.Info(Util.ClientIP + "|" + "Officer details updated for Office ID " + businessOfficer.OfficerId);

                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessOfficer));

            }


            return response;
        }

        public ResponseModel DeleteBusinessOfficer(int officerId)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete BusinessOfficer where OfficerId = @officerId";
                var result = db.Execute(sqlQuery, new { officerId });
                response.IsSuccess = true;
                response.Message = "Business Profile deleted";

                logger.Info(Util.ClientIP + "|" + "Officer details deleted for Office ID " + officerId);

             
            }
            return response;
        }
        public ResponseModel DeleteAllBusinessOfficer(int businessProfileId)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete BusinessOfficer where BusinessProfileId = @businessProfileId";
                var result = db.Execute(sqlQuery, new { businessProfileId });
                response.IsSuccess = true;
                response.Message = "Business Profile deleted";

                logger.Info(Util.ClientIP + "|" + "All Officer details deleted for the Client Profile ID " + businessProfileId);

                
            }
            return response;
        }
        public List<BusinessOfficerModel> GetBusinessOfficer(int BusinessProfileId)
        {
            List<BusinessOfficerModel> response = new List<BusinessOfficerModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select OfficerId, BusinessProfileId,Name,Address,Nationality,convert(varchar,BirthDate,105) BirthDate, " +
                " BirthCountry,Position,UserRole,Email,Mobile, isnull(MyInfoRequestId,0) MyInfoRequestId, " +
                " NricNo, convert(varchar,NricIssueDate,105) NricIssueDate,FinNo, convert(varchar,FinIssueDate,105) FinIssueDate," +
                " convert(varchar,FinExpiryDate,105) FinExpiryDate, PassportNo,convert(varchar,PassportIssueDate ,105) PassportIssueDate ," +
                " PassportIssuePlace,convert(varchar,PassportExpiryDate,105) PassportExpiryDate, " +
                " Sex,Phone,BirthPlace,convert(varchar,JoinDate,105) JoinDate ,PassportIssueCountry,NumberOfShares," +
                " AliasName,ResidentialStatus,Race,PassType,NricIssuePlace, convert(varchar,NricExpiryDate,105) NricExpiryDate," +
                " NricIssueCountry,EntityType from BusinessOfficer where BusinessProfileId=@BusinessProfileId  order by Name";
                response = db.Query<BusinessOfficerModel>(sqlQuery, new { BusinessProfileId }).AsList<BusinessOfficerModel>();
            }
            return response;
        }

        public List<DropDownModel> GetBusinessOfficerDown(int BusinessProfileId)
        {
            List<DropDownModel> result = null;
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<DropDownModel>("select OfficerId Value,Name + ' ( ' + UserRole + ' )'  Text from BusinessOfficer where BusinessProfileId=@BusinessProfileId order by Name",
                    new { BusinessProfileId }).AsList<DropDownModel>();
                result.Insert(0, new DropDownModel() { Value = "0", Text = "All" });
            }
            return result;
        }
        public BusinessOfficerModel GetAuthorisedRepresentative(int BusinessProfileId)
        {
            BusinessOfficerModel response = new BusinessOfficerModel();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Name,isnull(Email,'NO') Email ,Mobile " +
                " from BusinessOfficer where UserRole='Authorised Representative' and BusinessProfileId=@BusinessProfileId";
                response = db.QuerySingle<BusinessOfficerModel>(sqlQuery, new { BusinessProfileId });
            }
            return response;
        }

        public BusinessOfficerModel GetBusinessOfficerWithMyInfo(int BusinessProfileId, int MyInfoRequestId)
        {
            
            try
            {
                BusinessOfficerModel response = new BusinessOfficerModel();
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"select OfficerId, BusinessProfileId,Name,Address,Nationality,convert(varchar,BirthDate,105) BirthDate, " +
                    " BirthCountry,Position,UserRole,Email,Mobile, " +
                    " NricNo,convert(varchar,NricIssueDate,105) NricIssueDate,FinNo,convert(varchar,FinIssueDate,105) FinIssueDate," +
                    " convert(varchar,FinExpiryDate ,105) FinExpiryDate , PassportNo,convert(varchar,PassportIssueDate ,105) PassportIssueDate ," +
                    " PassportIssuePlace,convert(varchar,PassportExpiryDate,105) PassportExpiryDate, " +
                    " Sex,Phone,BirthPlace,convert(varchar,JoinDate,105) JoinDate,PassportIssueCountry,NumberOfShares," +
                    " AliasName,ResidentialStatus,Race,PassType,NricIssuePlace, convert(varchar,NricExpiryDate,105) NricExpiryDate," +
                    " NricIssueCountry,EntityType  from BusinessOfficer where BusinessProfileId=@BusinessProfileId and MyInfoRequestId=@MyInfoRequestId";
                    return db.QuerySingle<BusinessOfficerModel>(sqlQuery, new { BusinessProfileId, MyInfoRequestId });
                }
            }
            catch
            {
                return null;
            }
            
            
        }
        public List<string> GetTransferCodeAndName(int serviceBusinessId, string userRole)
        {
            return GetSignatoryCodeAndName(serviceBusinessId, userRole);
        }
        public List<string> GetSignatoryCodeAndName(int serviceBusinessId)
        {
            return GetSignatoryCodeAndName(serviceBusinessId, "ALL");
        }
        public List<string> GetSignatoryCodeAndName(int serviceBusinessId,string userRole)
        {
            string sql = " select convert(varchar,bo.OfficerId) +':'+ bo.Name + " +
            " case when es.RepresentativeId is not null then ' ('+ es.Name +')' else '' end from BusinessOfficer bo  " +
                " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId " +
                " left join EntityShareholder es on bo.OfficerId = es.RepresentativeId " +
            " where bo.userRole <> 'Authorised Representative' ";
            
            if (userRole.Equals("Shareholder"))
                sql = sql + " and (userRole='Shareholder' or (userRole='Director' and EntityType='Director Shareholder')) ";
            else if (userRole.Equals("UltimateOwner"))
                sql = sql + " and userRole='Shareholder' and EntityType='U.Beneficial Owner'  ";
            else if (userRole.Equals("Transferee") || userRole.Equals("Transferor"))
                sql = sql + " and userRole in ('Director','Shareholder') ";
            else if (userRole.Equals("Chairman"))
                sql = sql + " and userRole in ('Director','Shareholder','Chairman')  ";
            else if (userRole.Equals("Partner"))
                sql = sql + " and userRole in ('Partner','Precedent Partner','Sole Proprietor')  ";
            else if (userRole.Equals("PrecedentPartner"))
                sql = sql + " and userRole in ('Precedent Partner')  ";
            else if (!userRole.Equals("ALL"))
                sql = sql + " and userRole=@userRole ";

            sql = sql+ " and sb.Id = @serviceBusinessId order by bo.Name";

            List<string> result;
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<string>(sql,new { serviceBusinessId, userRole }, commandType: System.Data.CommandType.Text).AsList<string>();
            }
            return result;
        }

        public void UpdateNonExecutorIndntityType(string actualKeyword, string value, int serviceBusinessId, int officerStepId)
        {
            if (value.Length < 2)
                return;
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                //string baseKeyword = actualKeyword.Replace("P:", "").Replace("-Type", "") + "-%";
                string baseKeyword = actualKeyword.Replace("Identity-", "").Replace("-Type", "");
                string sqlQuery = "select CapturedValue from ServiceBusinessFields where Keyword = @baseKeyword " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                string code = db.QuerySingle<string>(sqlQuery, new { baseKeyword, serviceBusinessId, officerStepId });
                code = code.Split(':')[0];
                baseKeyword = actualKeyword.Replace("P:", "").Replace("-Type", "") + "-%";
                sqlQuery = "select Keyword from ServiceBusinessFields where Keyword like @baseKeyword " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                List<string> keywords = db.Query<string>(sqlQuery, new { baseKeyword, serviceBusinessId, officerStepId }).AsList<string>();
                if (keywords.Count > 0)
                {
                    baseKeyword = baseKeyword.Replace("%", "");
                    string Column = "";

                    foreach (string keyword in keywords)
                    {
                        Column = keyword.Replace(baseKeyword, "");
                        if (value.Equals("NRIC And Passport"))
                        {

                            if (Column.EndsWith("Date"))
                            {
                                Column = "'NRIC: '+ FORMAT (NRIC" + Column + ", 'dd/MM/yyyy') +', Passport: '+ FORMAT (Passport" + Column + ", 'dd/MM/yyyy') ";
                            }
                            else
                            {
                                Column = "'NRIC: '+ isnull(NRIC" + Column + ",'')+', Passport: '+ isnull(Passport" + Column + ",'')";
                            }

                        }
                        else
                        {
                            Column = value + keyword.Replace(baseKeyword, "");
                            if (Column.EndsWith("Date"))
                                Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";
                        }


                        sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column + " from " +
                                    " (select bo.* from BusinessOfficer bo where OfficerId  = @code  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                        try
                        {
                            db.Execute(sqlQuery, new
                            {
                                serviceBusinessId,
                                officerStepId,
                                code,
                                keyword
                            });

                        }
                        catch { }

                    }
                }

            }
        }
        public void UpdateIdentityParserDetail(string actualKeyword, string value, int serviceBusinessId, int officerStepId)
        {
            if (value.Length < 2)
                return;
            int officerId = 0;
            string sqlQuery = "";
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (actualKeyword.Equals("P:Identity-UltimateOwner-Type"))
                {
                    sqlQuery = "select CapturedValue from ServiceBusinessFields " +
                            " where ServiceBusinessId =@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword='P:UltimateOwner' ";
                    string Code = db.QuerySingle<string>(sqlQuery, new { serviceBusinessId, officerStepId });

                    Code = Code.Substring(0, Code.IndexOf(':'));
                    //string Column = "";
                    officerId = int.Parse(Code);
                }
                else
                {
                    sqlQuery = "select bo.OfficerId from BusinessOfficer bo " +
                            " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                            " where ServiceBusinessId =@ServiceBusinessId and sbo.Id=@OfficerStepId ";
                    officerId = db.QuerySingle<int>(sqlQuery, new { serviceBusinessId, officerStepId });
                }

                string baseKeyword = actualKeyword.Replace("P:", "").Replace("-Type", "") + "-%";
                sqlQuery = "select Keyword from ServiceBusinessFields where Keyword like @baseKeyword " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                List<string> keywords = db.Query<string>(sqlQuery, new { baseKeyword, serviceBusinessId, officerStepId }).AsList<string>();
                if (keywords.Count > 0)
                {
                    baseKeyword = baseKeyword.Replace("%", "");
                    
                    string Column = "";

                    foreach (string keyword in keywords)
                    {
                        Column = keyword.Replace(baseKeyword, "");
                        if (value.Equals("NRIC And Passport"))
                        {

                            if (Column.EndsWith("Date"))
                            {
                                Column = "'NRIC: '+ FORMAT (NRIC" + Column + ", 'dd/MM/yyyy') +', Passport: '+ FORMAT (Passport" + Column + ", 'dd/MM/yyyy') ";
                            }
                            else
                            {
                                Column = "'NRIC: '+ isnull(NRIC" + Column + ",'')+', Passport: '+ isnull(Passport" + Column + ",'')";
                            }

                        }
                        else
                        {
                            Column = value + keyword.Replace(baseKeyword, "");
                            if (Column.EndsWith("Date"))
                                Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";
                        }


                        sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column + " from " +
                                    " (select bo.* from BusinessOfficer bo " +
                                    "  where  OfficerId  = @OfficerId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                        try
                        {
                            db.Execute(sqlQuery, new
                            {
                                serviceBusinessId,
                                officerStepId,
                                officerId,
                                keyword
                            });

                        }
                        catch { }

                    }
                }

                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+UserRole+convert(varchar,a.OfficerId)  from " +
                                    " (select sbo.Id OfficerStepID, sbo.ServiceBusinessId, bo.* from BusinessOfficer bo " +
                                    " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                                    "  where sbo.ServiceBusinessId = @ServiceBusinessId and sbo.Id = @OfficerStepId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId = a.ServiceBusinessId and ServiceBusinessFields.OfficerStepId = a.OfficerStepId " +
                                    " and ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@baseKeyword";

                baseKeyword = actualKeyword.Replace("P:", "s:");
                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    baseKeyword
                });
            }

        }

        public void UpdateSignatoryParserDetail(string actualKeyword, string value, int serviceBusinessId, int officerStepId)
        {
            UpdateKeyParserDetail(actualKeyword,value, serviceBusinessId,officerStepId);
        }
        public void UpdateKeyParserDetail(string actualKeyword, string value, int serviceBusinessId, int officerStepId)
        {
            if (value.IndexOf(":") == -1)
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string baseKeyword = actualKeyword.Replace("P:", "") + "-%";
                    string sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = ''  " +
                            " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword like @baseKeyword";

                    db.Execute(sqlQuery, new
                    {
                        serviceBusinessId,
                        officerStepId,
                        baseKeyword
                    });

                    baseKeyword = actualKeyword.Replace("P:", "s:") ;
                    sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = '', CapturedBy = ''  " +
                            " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword = @baseKeyword";

                    db.Execute(sqlQuery, new
                    {
                        serviceBusinessId,
                        officerStepId,
                        baseKeyword
                    });

                    baseKeyword = actualKeyword.Replace("P:", "")+"-CSS";
                    sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = ' '  " +
                                    " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@baseKeyword";

                    db.Execute(sqlQuery, new
                    {
                        serviceBusinessId,
                        officerStepId,
                        baseKeyword
                    });

                }
                return;
            }
                
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string Code = value.Substring(0, value.IndexOf(':'));
                string Column = "";
                int officerId = int.Parse(Code);
                string baseKeyword = actualKeyword.Replace("P:", "") + "-%";
                Keywords keywordsParser = new Keywords();
                string sqlQuery = "select Keyword from ServiceBusinessFields where Keyword like @baseKeyword " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                List<string> keywords = db.Query<string>(sqlQuery, new { baseKeyword, serviceBusinessId, officerStepId }).AsList<string>();
                if (keywords.Count > 0)
                {
                    baseKeyword = baseKeyword.Replace("%", "");
                    //sqlQuery = "select bo.OfficerId from BusinessOfficer bo " +
                    //    " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                    //    " where ServiceBusinessId =@ServiceBusinessId and sbo.Id=@OfficerStepId ";
                    //officerId = db.QuerySingle<int>(sqlQuery, new { serviceBusinessId, officerStepId });
                   
                    foreach (string keyword in keywords)
                    {
                        Column = keywordsParser.GetColumnName(keyword.Replace(baseKeyword, ""));
                        if (Column.EndsWith("Date"))
                            Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";

                        if (keyword.EndsWith("-CSS"))

                            sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = 'hide-no'  " +
                                    " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                        else
                            sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column + " from " +
                                    " (select bo.* from BusinessOfficer bo " +
                                    " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId  " +
                                    "  where sb.Id = @ServiceBusinessId and bo.OfficerId  = @OfficerId  ) a " +
                                    " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";

                        db.Execute(sqlQuery, new
                        {
                            serviceBusinessId,
                            officerStepId,
                            officerId,
                            keyword
                        });

                    }
                }

                /*
                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+a.UserRole+convert(varchar,a.OfficerId)  from " +
                                    " (select sbo.Id OfficerStepID, sbo.ServiceBusinessId, bo.* from BusinessOfficer bo " +
                                    " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                                    "  where sbo.ServiceBusinessId = @ServiceBusinessId and bo.OfficerId  = @OfficerId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId = a.ServiceBusinessId and ServiceBusinessFields.OfficerStepId = a.OfficerStepId " +
                                    " and ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@baseKeyword";
                                    */

                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+a.UserRole+convert(varchar,a.OfficerId)  from  " +
                                    " (select bo.* from BusinessOfficer bo " +
                                    " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId  " +
                                    "  where sb.Id = @ServiceBusinessId and bo.OfficerId  = @OfficerId  ) a " +
                                    " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword like @baseKeyword";

                baseKeyword = actualKeyword.Replace("P:", "s:")+"%";
                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    officerId,
                    baseKeyword
                });
                baseKeyword = actualKeyword.Replace("P:", "d:") + "%";
                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    officerId,
                    baseKeyword
                });

                //sqlQuery = "select UserRole from BusinessOfficer where OfficerId=@officerId ";
                //Column = db.QuerySingle<string>(sqlQuery, new { officerId });
                //if (Column.Equals("Representative"))
                //{
                //     sqlQuery = "select Keyword from ServiceBusinessFields where Keyword like 'EntityShareholder-%' " +
                //        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                //    keywords = db.Query<string>(sqlQuery, new { serviceBusinessId, officerStepId }).AsList<string>();
                //    if (keywords.Count > 0)
                //    {
                //        baseKeyword = "EntityShareholder-";

                //        foreach (string keyword in keywords)
                //        {
                //            Column = keywordsParser.GetColumnName(keyword.Replace(baseKeyword, ""));
                //            if (Column.EndsWith("Date"))
                //                Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";

                //            if (keyword.EndsWith("-CSS"))

                //                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', CapturedValue = 'hide-no'  " +
                //                        " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                //            else
                //                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', CapturedValue = " + Column + " from " +
                //                        " (select * from EntityShareholder where RepresentativeId=@officerId ) a" +
                //                        " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId " +
                //                        "and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";

                //            db.Execute(sqlQuery, new
                //            {
                //                serviceBusinessId,
                //                officerStepId,
                //                officerId,
                //                keyword
                //            });

                //        }
                //    }

                //}
            }
        }

        public void UpdateParserDetail(int serviceBusinessId, int officerStepId,string userRole)
        {
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                /*
                string sqlQuery = "select distinct sbf.Keyword from ServiceBusinessFields sbf join DocumentFields df on sbf.Keyword = df.Keyword" +
                        " where sbf.Keyword like @userRole+'-%' " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                */
                string sqlQuery = "select  sbf.Keyword from ServiceBusinessFields sbf " +
                            " where sbf.Keyword like @userRole+'-%' " +
                            " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                List<string> keywords = db.Query<string>(sqlQuery, new { serviceBusinessId, officerStepId, userRole }).AsList<string>();
                if (keywords.Count>0)
                {
                    
                    sqlQuery = "select bo.OfficerId from BusinessOfficer bo " +
                        " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                        " where ServiceBusinessId =@ServiceBusinessId and sbo.Id=@OfficerStepId ";
                    int officerId = db.QuerySingle<int>(sqlQuery, new { serviceBusinessId, officerStepId });
                    
                    string Column = "";
                    Keywords keywordsParser = new Keywords();
                    foreach (string keyword in keywords)
                    {
                        Column = keywordsParser.GetColumnName(keyword.Replace(userRole + "-", ""));
                        if (Column.EndsWith("Date"))
                            Column = " FORMAT (" + Column + ", 'dd/MM/yyyy') ";

                        if (keyword.EndsWith("-CSS"))
                        {
                            sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = 'hide-no'  " +
                            " where  ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";

                            db.Execute(sqlQuery, new
                            {
                                serviceBusinessId,
                                officerStepId,
                                keyword
                            });
                        }
                        else
                        {

                            /*
                             
                            sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', CapturedValue = " + Column + " from " +
                                    " (select sbo.Id OfficerStepID, sbo.ServiceBusinessId, bo.* from BusinessOfficer bo "+
                                    " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                                    "  where sbo.ServiceBusinessId = @ServiceBusinessId and sbo.Id = @OfficerStepId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId = a.ServiceBusinessId and ServiceBusinessFields.OfficerStepId = a.OfficerStepId " +
                                    " and ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                            */

                            sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column + " from " +
                                    " (select bo.* from BusinessOfficer bo " +
                                    "  where bo.OfficerId = @officerId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword=@keyword";
                            db.Execute(sqlQuery, new
                            {
                                serviceBusinessId,
                                officerStepId,
                                officerId,
                                keyword
                            });

                        }
                    }
                    /*
                    sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+UserRole+convert(varchar,a.OfficerId) from " +
                                " (select sbo.Id OfficerStepID, sbo.ServiceBusinessId, bo.* from BusinessOfficer bo " +
                                " join ServiceBusinessOfficer sbo on bo.OfficerId = sbo.OfficerId " +
                                "  where sbo.ServiceBusinessId = @ServiceBusinessId and sbo.Id = @OfficerStepId  ) a " +
                                " where ServiceBusinessFields.ServiceBusinessId = a.ServiceBusinessId and ServiceBusinessFields.OfficerStepId = a.OfficerStepId " +
                                " and ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword in ('s:Executor','d:Executor','c:Executor')";
                                */
                    sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+UserRole+convert(varchar,a.OfficerId) from " +
                               " (select bo.* from BusinessOfficer bo " +
                                "  where bo.OfficerId = @officerId  ) a " +
                                " where ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword in ('s:Executor','d:Executor','c:Executor')";

                    db.Execute(sqlQuery, new
                    {
                        serviceBusinessId,
                        officerStepId,
                        officerId
                    });

                    
                }

            }
        }

        private string GetCurrencyFormat(string currency)
        {
            try
            {
                decimal parsed = decimal.Parse(currency, CultureInfo.InvariantCulture);
                CultureInfo hindi = new CultureInfo("hi-IN");
                return string.Format(hindi, "{0:c}", parsed).Substring(1);

            }
            catch
            {
                return " ";
            }
        }
        private string GetCurrencyFormatInt(string currency)
        {
            string formated = GetCurrencyFormat(currency);
            if (formated.Trim().Length > 0)
            {
                return formated.Substring(0, formated.Length - 3);
            }
            else
                return formated;
        }
        public void UpdateExecuterAll(int serviceBusinessId, int officerStepId, string baseKeyword )
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string userRole = baseKeyword.Replace("ExecutorAll-", "");


                string sqlQuery = " select  bo.OfficerId, bo.Name, isnull(NumberOfShares,0) NumberOfShares,Email,UserRole  " +
                        " from BusinessOfficer bo  " +
                            " join ServiceBusiness sb on bo.BusinessProfileId = sb.BusinessProfileId " +
                        " where bo.userRole <> 'Authorised Representative' ";

                /*
                if (userRole.Equals("Shareholder"))
                    sqlQuery = sqlQuery + " and (userRole='Shareholder' or (userRole='Director' and EntityType='Director Shareholder')) ";
                else 
                */
                if (!userRole.Equals("All"))
                    sqlQuery = sqlQuery + " and userRole=@userRole ";

                sqlQuery = sqlQuery + " and sb.Id = @serviceBusinessId order by bo.Name";

                List<BusinessOfficerModel> businessOfficers= db.Query<BusinessOfficerModel>(sqlQuery, new { serviceBusinessId, userRole }, commandType: System.Data.CommandType.Text).AsList<BusinessOfficerModel>();

                
                sqlQuery = "select  sbf.Keyword from ServiceBusinessFields sbf " +
                            " where sbf.Keyword like @baseKeyword+'-%' " +
                            " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                List<string> keywords = db.Query<string>(sqlQuery, new { serviceBusinessId, officerStepId, baseKeyword }).AsList<string>();
                if (keywords.Count > 0)
                {
                    string Column = "";
                    string ColumnValue = "";
                    foreach (string keyword in keywords)
                    {
                        Column = keyword.Replace(baseKeyword + "-", "");

                        if(Column.Equals("Name"))
                        {
                            ColumnValue = "";
                            foreach (BusinessOfficerModel businessOfficer in businessOfficers)
                                ColumnValue += businessOfficer.Name + ", ";

                            if (ColumnValue.Length > 3)
                                ColumnValue = ColumnValue.Substring(0, ColumnValue.Length - 2);
                        }
                        else if (Column.Equals("NumberOfShares"))
                        {
                            ColumnValue = "";
                            int numberOfShares = 0;
                            foreach (BusinessOfficerModel businessOfficer in businessOfficers)
                            {
                                if (businessOfficer.NumberOfShares>0)
                                {
                                    numberOfShares += businessOfficer.NumberOfShares;
                                    ColumnValue += string.Format("<tr><td width='400px' class='sub-heading '>{0}</td><td>:</td><td class='sub-heading' style='text-align:right'>{1}</td></tr>",
                                                                        businessOfficer.Name, GetCurrencyFormatInt(businessOfficer.NumberOfShares + ""));
                                }
                            }
                               
                            if(numberOfShares>0)
                                ColumnValue += string.Format("<tr><td width='400px' class='sub-heading '>TOTAL</td><td>:</td><td class='sub-heading'style='text-align:right'> <div><hr></div>{0}<div><hr></div></td></tr>",
                                                                   GetCurrencyFormatInt(numberOfShares + ""));
                        }
                        else if (Column.Equals("Signature"))
                        {
                            ColumnValue = "";
                            string CapturedBy = "";
                            sqlQuery = "delete ServiceBusinessFields  " +
                            " where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword ='s:Exe-Auto'";

                            db.Execute(sqlQuery, new
                            {
                                serviceBusinessId,
                                officerStepId
                            });

                            sqlQuery = "insert into ServiceBusinessFields (ServiceBusinessId,OfficerStepId,Keyword,CapturedSource,CapturedValue,CapturedBy,UpdatedTime) " +
                            " values(@ServiceBusinessId,@OfficerStepId,'s:Exe-Auto','DB',@CapturedBy,@Email,getdate() )";

                            foreach (BusinessOfficerModel businessOfficer in businessOfficers)
                            {
                                CapturedBy = string.Format("s:{0}{1}", businessOfficer.UserRole, businessOfficer.OfficerId);
                                ColumnValue += string.Format("<tr><td width='400px' class='sub-heading '>{0}</td><td>:</td><td><span class='signature'>[{1}]</span> <br><hr style='width: 300px; margin: 0 auto; '></td></tr>",
                                                                    businessOfficer.Name, CapturedBy);

                                db.Execute(sqlQuery, new
                                {
                                    serviceBusinessId,
                                    officerStepId,
                                    businessOfficer.Email,
                                    CapturedBy
                                });
                            }

                        }
                        sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = @ColumnValue " + 
                        " where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@keyword";

                        db.Execute(sqlQuery, new
                        {
                            ColumnValue,
                            serviceBusinessId,
                            officerStepId,
                            keyword
                        });

                        /*
                        sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedBy = email,CapturedValue=SUBSTRING(Keyword,1,2)+UserRole+convert(varchar,a.OfficerId) from " +
                                   " (select bo.* from BusinessOfficer bo " +
                                    "  where bo.OfficerId = @officerId  ) a " +
                                    " where ServiceBusinessFields.ServiceBusinessId=@ServiceBusinessId and ServiceBusinessFields.OfficerStepId=@OfficerStepId and Keyword in ('s:Executor','d:Executor','c:Executor')";
                        
                        db.Execute(sqlQuery, new
                        {
                            serviceBusinessId,
                            officerStepId,
                            officerId
                        });
                        */
                    }


                }

            }
        }
    }
}
