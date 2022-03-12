using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System;
using Newtonsoft.Json;
using NLog;

namespace RoboDocLib.Services
{
    public class EmployeeMaster
    {
        string connectionString = "";
        int UserId = 999;
        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public EmployeeMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;
        }

        public ResponseModel PostEmployee(EmployeeModelWrapper employeeMaster)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                EmployeeModel employee = employeeMaster.Master;

                string sqlQuery = @"Select isnull(max(AutoId)+1,1) from EmployeeMaster";
                int AutoId = db.QuerySingle<int>(sqlQuery);
                employee.Code = string.Format("E{0}",string.Format("{0:0000}", AutoId));
                sqlQuery = @" insert into EmployeeMaster(UserId,CreatedDate,Code,FirstName,LastName,Nationality,BirthDate,Gender,Position, " +
                                    " CardType,CardNumber,Address1,Address2,City,Country,Pincode,Email,Mobile,Phone,AutoId,LoginRole) " +
                                    " values(@UserId,getdate(),@Code,@FirstName,@LastName,@Nationality,@BirthDate,@Gender,@Position,@CardType, " +
                                    " @CardNumber,@Address1,@Address2,@City,@Country,@Pincode,@Email,@Mobile,@Phone,@AutoId,@LoginRole) ";
                var result = db.Execute(sqlQuery, new
                {
                    UserId,
                    employee.Code,
                    employee.FirstName,
                    employee.LastName,
                    employee.Nationality,
                    employee.BirthDate,
                    employee.Gender,
                    employee.Position,
                    employee.CardType,
                    employee.CardNumber,
                    employee.Address1,
                    employee.Address2,
                    employee.City,
                    employee.Country,
                    employee.Pincode,
                    employee.Mobile,
                    employee.Email,
                    employee.Phone,
                    AutoId,
                    employee.LoginRole
                });
                if (employee.Userpassword.Trim().Length>0)
                    updatePassword(employee.Code, employee.Userpassword, db);

                logger.Info(Util.ClientIP + "|" + "Employee details added for " + employee.Code);
                employee.Userpassword = "****";
                logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(employee));

                UpdatePositionAndRoles(employee.Code, "Position", employeeMaster.Position, db);
                UpdatePositionAndRoles(employee.Code, "Role", employeeMaster.Role, db);
                response.IsSuccess = true;
                response.Message = "Employee added";



            }
            return response;
        }
        private void UpdatePositionAndRoles(string code, string nature, List<DropDownModel> names, IDbConnection db)
        {
            string sqlQuery = @"Delete EmployeePositionsAndRoles where Code=@code and nature=@nature";
            var result = db.Execute(sqlQuery, new { code, nature });

            sqlQuery = @"insert into EmployeePositionsAndRoles values(@code,@nature,@value)";

            foreach(DropDownModel name in names)
            {
                db.Execute(sqlQuery, new { code, nature, name.Value });
            }

            logger.Info(Util.ClientIP + "|" + "Employee  " + nature + " update");

            logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(names));
        }
        public ResponseModel PutEmployee(EmployeeModelWrapper employeeMaster)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                EmployeeModel employee = employeeMaster.Master;

                string sqlQuery = @" update EmployeeMaster set UserId=@UserId,UpdatedDate=getdate(),FirstName=@FirstName,LastName=@LastName, " +
                    " Nationality=@Nationality,BirthDate=@BirthDate,Gender=@Gender,Position=@Position,CardType=@CardType,CardNumber=@CardNumber, " +
                    " Address1=@Address1,Address2=@Address2,City=@City,Country=@Country,Pincode=@Pincode, " +
                    " Email=@Email,Mobile=@Mobile,Phone=@Phone,LoginRole=@LoginRole where Code=@Code ";
                var result = db.Execute(sqlQuery, new
                {
                    UserId,
                    employee.Code,
                    employee.FirstName,
                    employee.LastName,
                    employee.Nationality,
                    employee.BirthDate,
                    employee.Gender,
                    employee.Position,
                    employee.CardType,
                    employee.CardNumber,
                    employee.Address1,
                    employee.Address2,
                    employee.City,
                    employee.Country,
                    employee.Pincode,
                    employee.Mobile,
                    employee.Email,
                    employee.Phone,
                    employee.LoginRole
                });

                if (employee.Userpassword.Trim().Length > 0)
                    updatePassword(employee.Code, employee.Userpassword, db);

                logger.Info(Util.ClientIP + "|" + "Employee details modified for " + employee.Code);
                employee.Userpassword = "*****";
                logger.Info(JsonConvert.SerializeObject(employee));

                response.IsSuccess = true;
                response.Message = "Employee modified";

                UpdatePositionAndRoles(employee.Code, "Position", employeeMaster.Position, db);
                UpdatePositionAndRoles(employee.Code, "Role", employeeMaster.Role, db);
            }
            return response;
        }

        public ResponseModel DeleteEmployee(string code)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete EmployeeMaster where Code=@code";
                var result = db.Execute(sqlQuery, new { code });
                response.IsSuccess = true;
                response.Message = "Employee deleted";

                logger.Info(Util.ClientIP + "|" + "Employee deleted, deleted code is " + code);
            }
            return response;
        }

        public List<EmployeeModel> GetEmployee()
        {
            List<EmployeeModel> response = new List<EmployeeModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Code,FirstName,LastName,Nationality,convert(varchar,BirthDate,103) BirthDate,Gender,Position, " +
                                    " CardType,CardNumber,Address1,Address2,City,Country,Pincode,Email,Mobile,Phone, isnull(LoginRole,'None') LoginRole," +
                                    " ' ' Userpassword from EmployeeMaster order by FirstName";
                response = db.Query<EmployeeModel>(sqlQuery).AsList<EmployeeModel>();
            }
            return response;
        }

        public List<DropDownModel> GetPositionOrRole(string code, string nature)
        {
            List<DropDownModel> response = new List<DropDownModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select Name Value,Name Text from EmployeePositionsAndRoles where code=@code and nature=@nature order by Name";
                response = db.Query<DropDownModel>(sqlQuery,new { code,nature}).AsList<DropDownModel>();
            }
            return response;
        }

        private void updatePassword(string code, string password, IDbConnection connection)
        {
            

            if (password.Length>0)
            {
                password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(password));

                string sql = "Update EmployeeMaster set userpassword= @password where code=@code";
                
                connection.Execute(sql, new { password,code });

                logger.Info(Util.ClientIP + "|" + "Password changed for " + code);
            }
        }

        public List<string> GetEmployeeCodeAndName()
        {
            string sql = "select a.code+':'+FirstName+':'+ Name   from EmployeeMaster a " +
                          " join EmployeePositionsAndRoles b on a.Code = b.code order by 1 ";

            List<string> result;
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<string>(sql, commandType: System.Data.CommandType.Text).AsList<string>();
            }
            return result;
        }

        public void UpdateParserDetail(string actualKeyword, string value, int serviceBusinessId, int officerStepId)
        {
            if (value.IndexOf(":") == -1)
                return;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string Code = value.Substring(0, value.IndexOf(':'));
                string Column = "";
                string baseKeyword = actualKeyword.Replace("P:", "") + "-%";
                string sqlQuery = "select Keyword from ServiceBusinessFields where Keyword like @baseKeyword " +
                        " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
                List<string> keywords = db.Query<string>(sqlQuery, new { baseKeyword, serviceBusinessId, officerStepId }).AsList<string>();
                baseKeyword = baseKeyword.Replace("%", "");
                foreach (string keyword in keywords)
                {
                    Column = keyword.Replace(baseKeyword, "");
                    if (Column.Equals("Address1"))
                        Column = "Address1 +', '+Address2+', '+country+' '+pincode";
                    else if (Column.Equals("Position"))
                        Column = "'" + value.Split(':')[2] +"'" ;

                    sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue = " + Column +" from EmployeeMaster a " +
                                "where a.Code = @Code and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@keyword";
                    
                    db.Execute(sqlQuery, new
                    {
                        serviceBusinessId,
                        officerStepId,
                        keyword,
                        Code
                    });
                }

                sqlQuery = "update ServiceBusinessFields set CapturedSource = 'DB', UpdatedTime=GETDATE(), CapturedValue=Keyword, CapturedBy= email from EmployeeMaster a " +
                           "where a.Code = @Code and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@baseKeyword";

                baseKeyword = actualKeyword.Replace("P:", "s:");

                db.Execute(sqlQuery, new
                {
                    serviceBusinessId,
                    officerStepId,
                    baseKeyword,
                    Code
                });
            }
        }

        public ResponseModel SendLoginOTP(string email, string otp)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (email != null)
                {

                    string body = "<head>" +
                    "</head>" +
                    "<body>" +

                        "<h1>Login OTP.</h1>" + Environment.NewLine +
                        "<p>Dear User, </p>" + Environment.NewLine +
                        "<p>Your OTP is <b>" + otp + "</b>. It will expire in 10 minutes. Please use this to login.</p>" + Environment.NewLine +
                        "<p>If your OTP does not work, please login again.</p>" + Environment.NewLine +
                        "<p>If you did not make this request, you may ignore this email</p>" + Environment.NewLine +
                        "</body>";

                    MailClient mail = new MailClient();


                    string emailResponse = mail.SendMail("Mail from AIDA", email, "OTP to login", body);
                    if (emailResponse.Equals("OKAY"))
                    {
                        response.IsSuccess = true;
                        response.Message = "Email sent to " + email;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = emailResponse;
                    }


                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Mail Not sent problem in email";
                }

                logger.Info(Util.ClientIP + "|" + "Login OTP sent for email id " + email + " and response is " + response.Message);

            }

            return response;
        }


        public ResponseModel GetLoginOTP(LoginModel login)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Login failed" };
            logger.Info(Util.ClientIP + "|" + "Login request for " + login.UserId);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                login.Password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));

                string sqlQuery = @"select isnull(email,'') " +
                            " from EmployeeMaster where code=@UserId and Userpassword=@password and" +
                             " isnull(LoginRole,'None') <> 'None'";
                string Email = db.QuerySingleOrDefault<string>(sqlQuery, new { login.UserId, login.Password });
                if (Email != null && Email.Length>0)
                {
                    sqlQuery = @"update EmployeeMaster set OTP=@OTP,OTPDate = getdate() where code=@UserId";
                    string OTP = Guid.NewGuid().ToString();
                    OTP = OTP.Replace("-", "");
                    if (OTP.Length > 8)
                        OTP = OTP.Substring(0, 8);

                    db.Execute(sqlQuery, new { OTP, login.UserId });
                    response = SendLoginOTP(Email, OTP);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not a valid login credential or email id";

                }

                logger.Info(Util.ClientIP + "|" + "OTP send and response is " + response.Message);

            }

            if (response == null)
                response = new LoginResponseModel() { IsSuccess = false, Message = "Login failed" };

            logger.Info(Util.ClientIP + "|" + "Login status " + response.Message);

            return response;
        }

        public LoginResponseModel GetLoginResponse(LoginModel login,string path)
        {
            LoginResponseModel response = new LoginResponseModel() { IsSuccess = false, Message = "Login failed" };
            logger.Info(Util.ClientIP + "|" + "Login request for " + login.UserId);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                login.Password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(login.Password));

                string sqlQuery = @"select FirstName + ' ' + isnull(LastName,'') Name,LoginRole Role, " +
                                    " datediff(n,otpdate,getdate()) TimeDiff from EmployeeMaster where code=@UserId and OTP=@password and" +
                                    " isnull(LoginRole,'None') <> 'None'";
                response = db.QueryFirstOrDefault<LoginResponseModel>(sqlQuery, new { login.UserId,login.Password });
                if (response!= null && response.Role.Length>0)
                {
                    if (response.TimeDiff>10)
                    {
                        response.IsSuccess = false;
                        response.Message = "OTP expired, Please login again.";
                    }
                    else
                    {
                        response.Menus = JsonConvert.DeserializeObject<List<AppMenuModel>>(
                            System.IO.File.ReadAllText(path + @"\menu\" + response.Role + @".json"));
                        response.IsSuccess = true;
                        response.Message = "Welcome " + response.Name;
                    }
                }

            }

            if(response==null)
                response = new LoginResponseModel() { IsSuccess = false, Message = "Login failed" };

            logger.Info(Util.ClientIP + "|" + "Login status " + response.Message);

            return response;
        }
    }
}
