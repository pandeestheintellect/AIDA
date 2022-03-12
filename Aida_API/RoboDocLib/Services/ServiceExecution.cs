using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using RoboDocLib.Parsers;
using System;
using NLog;
using System.Configuration;
using System.Text.RegularExpressions;

namespace RoboDocLib.Services
{
    public class ServiceExecution
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceExecution(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }


        public List<ServicesEntry> GetServiceEntry(int serviceBusinessId, int officerId)
        {
            List<ServicesEntry> result;
            List<ServicesEntryFieldModel> fields;

            string sql = " select sbo.ServiceBusinessId,sbo.Id OfficerStepId, sbo.StepNo, sd.Code ServiceCode, sd.Name ServiceName, " +
                    " doc.Name DocumentName, bo.Name OfficerName ,bo.UserRole OfficerRole,bp.Name BusinessProfileName, " +
                    " sbo.Status,isnull(sbo.GeneratedFileName,'NoFile')GeneratedFileName from ServicesDefinition sd  " +
                    " join ServiceBusiness sb on sd.Code = sb.ServiceCode join ServiceBusinessOfficer sbo on sb.Id = sbo.ServiceBusinessId " +
                    " join DocumentMaster doc on sbo.DocumentCode = doc.Code join BusinessProfile bp on sbo.BusinessProfileId=bp.Id " +
                    " join BusinessOfficer bo on bp.Id=bo.BusinessProfileId and sbo.OfficerId=bo.OfficerId " +
                        " where sb.Id=@ServiceBusinessId and sbo.OfficerId = @OfficerId order by sbo.StepNo ";

            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<ServicesEntry>(sql, new { @ServiceBusinessId = serviceBusinessId, @OfficerId = officerId },
                    commandType: System.Data.CommandType.Text).AsList<ServicesEntry>();

                sql = @" select df.Fieldid, sbf.OfficerStepId,df.code, case when df.Control='text' then 'input' else df.Control end Type, " +
                    " df.Keyword Name, df.Label Label,case when df.Control='text' or df.Control='currency' then df.Control else null end InputType,isnull(CapturedValue,'') Value, " +
                    " df.ControlNumber, df.FxSizePer " +
                    " from ServiceBusinessOfficer sbo join ServiceBusinessFields sbf on sbo.id = sbf.officerstepid " +
                    " join DocumentFields df on sbo.DocumentCode = df.Code and sbf.Keyword=df.Keyword " +
                    " where Nature='Input'and sbo.ServiceBusinessId=@ServiceBusinessId and sbo.OfficerId = @OfficerId order by df.Fieldid ";

                fields = connection.Query<ServicesEntryFieldModel>(sql, new { @ServiceBusinessId = serviceBusinessId, @OfficerId = officerId },
                    commandType: System.Data.CommandType.Text).AsList<ServicesEntryFieldModel>();

            }

            Keywords keywords = new Keywords();
            KeywordParser keyword;
            List<string> options;
            foreach (var res in fields.FindAll(x => x.Type.Equals("select")).AsList<ServicesEntryFieldModel>())
            {
                if (keywords.KeywordParser.TryGetValue(res.Name, out keyword))
                {
                    options = keyword.GetList(Util, serviceBusinessId);
                    if (options == null)
                        options = new List<string>();
                    options.Add("");
                    res.Options = options.ToArray();
                    
                }

            }
            foreach (var res in result)
            {
                res.Fields = fields.FindAll(x => x.OfficerStepId == res.OfficerStepId).AsList<ServicesEntryFieldModel>();
                if ((res.Status.Equals("New") || res.Status.Equals("In-Progress")) && res.Fields.Count>0)
                    res.Fields.Add(new ServicesEntryFieldModel() { Type = "button", Label = "Save" });
            }

            return result;
        }
        
        public ResponseModel PostServiceEntry(ServicesEntrySave serviceEntry)
        {


            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ServiceBusinessFields set CapturedValue=@Value, UpdatedTime=GETDATE()  " +
                                    "  where ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId and Keyword=@Key";
                Keywords keywords = new Keywords();
                KeywordParser keyword;
                string value = "";
                foreach (KeyValuePair<string, string> entry in serviceEntry.ElementObject)
                {
                    if (keywords.KeywordParser.TryGetValue(entry.Key, out keyword))
                    {
                        keyword.UpdateDetail(Util, entry.Value, serviceEntry.ServiceBusinessId, serviceEntry.OfficerStepId);
                        
                        if (entry.Key.StartsWith("C:"))
                            value = keywords.GetControlValues(entry.Key, entry.Value);
                        else
                            value = entry.Value;
                    }
                    else
                        value = keywords.GetControlValues(entry.Key, entry.Value);

                    var result = db.Execute(sqlQuery, new
                    {
                        serviceEntry.ServiceBusinessId,
                        serviceEntry.OfficerStepId, 
                        entry.Key,
                        value
                    });
                }


                sqlQuery = @" update ServiceBusinessOfficer set status = 'In-Progress',UpdatedDate=GetDate() where Id=@OfficerStepId ";

                db.Execute(sqlQuery, new
                {
                    serviceEntry.OfficerStepId
                });

                sqlQuery = @" update ServiceBusiness set status = 'In-Progress' where Id=@ServiceBusinessId ";
                db.Execute(sqlQuery, new
                {
                    serviceEntry.ServiceBusinessId
                });

                
                sqlQuery = "select top 1 Keyword from ServiceBusinessFields " +
                " where  Keyword like'ExecutorAll-Director%' " +
                " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                value = db.QueryFirstOrDefault<string>(sqlQuery, new { serviceEntry.ServiceBusinessId, serviceEntry.OfficerStepId});
                if (value != null && value.Length>0)
                {
                    new BusinessOfficerMaster(Util).UpdateExecuterAll(serviceEntry.ServiceBusinessId,
                        serviceEntry.OfficerStepId, "ExecutorAll-Director");
                }

                sqlQuery = "select top 1 Keyword from ServiceBusinessFields " +
                " where  Keyword like'ExecutorAll-Shareholder%' " +
                " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                value = db.QueryFirstOrDefault<string>(sqlQuery, new { serviceEntry.ServiceBusinessId, serviceEntry.OfficerStepId });
                if (value != null && value.Length > 0)
                {
                    new BusinessOfficerMaster(Util).UpdateExecuterAll(serviceEntry.ServiceBusinessId,
                        serviceEntry.OfficerStepId, "ExecutorAll-Shareholder");
                }

                sqlQuery = "select top 1 Keyword from ServiceBusinessFields " +
                " where  Keyword like'ExecutorAll-Secretary%' " +
                " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                value = db.QueryFirstOrDefault<string>(sqlQuery, new { serviceEntry.ServiceBusinessId, serviceEntry.OfficerStepId });
                if (value != null && value.Length > 0)
                {
                    new BusinessOfficerMaster(Util).UpdateExecuterAll(serviceEntry.ServiceBusinessId,
                        serviceEntry.OfficerStepId, "ExecutorAll-Secretary");
                }
                sqlQuery = "select top 1 Keyword from ServiceBusinessFields " +
                " where  Keyword like'ExecutorAll-All%' " +
                " and ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";

                value = db.QueryFirstOrDefault<string>(sqlQuery, new { serviceEntry.ServiceBusinessId, serviceEntry.OfficerStepId });
                if (value != null && value.Length > 0)
                {
                    new BusinessOfficerMaster(Util).UpdateExecuterAll(serviceEntry.ServiceBusinessId,
                        serviceEntry.OfficerStepId, "ExecutorAll-All");
                }


                response.IsSuccess = true;
                response.Message = "Services modified";
            }
            new BusinessOfficerMaster(Util).UpdateParserDetail(serviceEntry.ServiceBusinessId,
                    serviceEntry.OfficerStepId, "Executor");
            new BusinessProfileMaster(Util).UpdateParserDetail(serviceEntry.ServiceBusinessId);



            logger.Info(Util.ClientIP + "|" + "Services modified for Service Business Id " + serviceEntry.ServiceBusinessId
                        + ", Officer Step Id  " +  serviceEntry.OfficerStepId + " and response is " + response.Message);
            
            return response;
        }

        public ResponseModel PutServiceEntry(ServicesEntrySave serviceEntry)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ServiceBusinessOfficer set status = @Elements,UpdatedDate=GetDate() " +
                                    "  where ServiceBusinessId=@ServiceBusinessId and Id=@OfficerStepId";
                var result = db.Execute(sqlQuery, new
                {
                    serviceEntry.Elements,
                    serviceEntry.ServiceBusinessId,
                    serviceEntry.OfficerStepId,
                    
                });
                response.IsSuccess = true;
                response.Message = "Services modified";
            }
            logger.Info(Util.ClientIP + "|" + "Services modified for Service Business Id " + serviceEntry.ServiceBusinessId
                        + ", Officer Step Id  " + serviceEntry.OfficerStepId + " and response is " + response.Message);

            return response;
        }

        public ResponseModel SendInvite(ServicesSignSave serviceSign)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select Email from BusinessOfficer bo " +
                        " where bo.OfficerId =@OfficerStepIds ";
                string emailid = db.QuerySingle<string>(sqlQuery, new { serviceSign.OfficerStepIds });
                if (emailid!=null)
                {

                    string OTP = Guid.NewGuid().ToString();
                    OTP = OTP.Replace("-", "");

                    if (OTP.Length > 20)
                        OTP = OTP.Substring(0, 20);

                    string body = "<head>" +
                    "</head>" +
                    "<body>" +
                        "<h1>Document filling invitation.</h1>" + Environment.NewLine +
                        "<p>Dear User, </p>" + Environment.NewLine +
                        "<p>Pleas follow this hyperlink to fill your document.</p>" + Environment.NewLine +
                        "<a href='"+ ConfigurationManager.AppSettings["MyPath"] + "/document-filling/invite/" + OTP + "' > Click here to continue</a>" +
                        //"<a href='http://localhost:4200/document-invite/" + link + "' > Click here to continue</a>" +                    
                        "</body>";

                    MailClient mail = new MailClient();
                    
                    string emailResponse = mail.SendMail("Mail from AIDA", emailid, "Invitation to fill the document", body);
                    if (emailResponse.Equals("OKAY"))
                    {
                        response.IsSuccess = true;
                        response.Message = "Email sent to " + emailid;

                        sqlQuery = @"Insert into DocumentInvite(InviteKey,ServiceBusinessId,OfficerId,Status) " +
                                " values (@OTP,@ServiceBusinessId,@OfficerStepIds,'New')";


                        db.Execute(sqlQuery, new { OTP, serviceSign.ServiceBusinessId, serviceSign.OfficerStepIds });
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

                logger.Info(Util.ClientIP + "|" + "Document filling invitation sent for Service Business Id " + serviceSign.ServiceBusinessId
            + ", Officer Step Id  " + serviceSign.OfficerStepIds + " and response is " + response.Message);

            }


            //using (IDbConnection db = new SqlConnection(connectionString))
            //{
            //    string sqlQuery = @"update ServiceBusinessOfficer set status = @Elements " +
            //                        "  where ServiceBusinessId=@ServiceBusinessId and Id=@OfficerStepId";
            //    var result = db.Execute(sqlQuery, new
            //    {
            //        serviceEntry.Elements,
            //        serviceEntry.ServiceBusinessId,
            //        serviceEntry.OfficerStepId,

            //    });
            //    response.IsSuccess = true;
            //    response.Message = "Services modified";
            //}
            return response;
        }


        public ResponseModel SendDocumentFillingOTP(string email,string otp)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (email != null)
                {

                    string body = "<head>" +
                    "</head>" +
                    "<body>" +
                        "<h1>Document filling OTP.</h1>" + Environment.NewLine +
                        "<p>Dear User, </p>" + Environment.NewLine +
                        "<p>Pleas use this <b>"+ otp + "</b> as OTP to complete the document filling.</p>" + Environment.NewLine +
                        "</body>";

                    MailClient mail = new MailClient();
                    
                    
                    string emailResponse = mail.SendMail("Mail from AIDA", email, "OTP to fill the document", body);
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

                logger.Info(Util.ClientIP + "|" + "Document filling OTP sent for email id " + email + " and response is " + response.Message);

            }

            return response;
        }

        public ResponseModel SendFormDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument,string path)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            if (serviceRegistraionDocument.Email != null)
            {


                string documentName = "";
                

                string [] files = serviceRegistraionDocument.FilePaths.Split(',');

                string folderName = "";
                if (serviceRegistraionDocument.DocumentType.Equals("Registration"))
                {
                    folderName = "Registration-Editable";
                    documentName = "Registration Documents.";
                }
                else if (serviceRegistraionDocument.DocumentType.Equals("Employment"))
                {
                    folderName = "Employment Agency";
                    documentName = "Employment Agency.";
                }

                else if (serviceRegistraionDocument.DocumentType.Equals("ec-clients"))
                {
                    folderName = "ACHI Biz AIDA 13-Checklist-EC-Client";
                    documentName = "Checklist.";
                }
                else if (serviceRegistraionDocument.DocumentType.Equals("nc-clients"))
                {
                    folderName = "ACHI Biz AIDA 12-Checklist-NC-Client";
                    documentName = "Checklist.";
                }

                string body = "<head>" +
                "</head>" +
                "<body>" +
                    "<h1>"+ documentName + "</h1>" + Environment.NewLine +
                    "<p>Dear " + serviceRegistraionDocument.Name + ", </p>" + Environment.NewLine +
                    "<p>" + serviceRegistraionDocument.Message + ",</p>" + Environment.NewLine +
                    "</body>";


                List<string> attachments = new List<string>();
                foreach (string file in files)
                    attachments.Add(path + @"\SourceHTML\"+ folderName + @"\" +  file);

                MailClient mail = new MailClient();
                string mailStaus= mail.SendMail("Mail from AIDA", serviceRegistraionDocument.Email,
                    documentName, body, attachments);
                if (mailStaus.Equals("OKAY"))
                {
                    response.IsSuccess = true;
                    response.Message = "Email sent to " + serviceRegistraionDocument.Email;


                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        string sqlQuery = "Insert into InitialDocumentDispatch(CreatedDate, Name,Email,Mobile,Message, " +
                            " Documents,FilePaths,Status,documentType) values(Getdate(), @Name, @Email, @Mobile, @Message, @DocumentNames, " +
                            " @FilePaths, 'In-Progress',@DocumentType)";
                        var result = db.Execute(sqlQuery, new
                        {
                            serviceRegistraionDocument.Name,
                            serviceRegistraionDocument.Email,
                            serviceRegistraionDocument.Mobile,
                            serviceRegistraionDocument.Message,
                            serviceRegistraionDocument.DocumentNames,
                            serviceRegistraionDocument.FilePaths,
                            serviceRegistraionDocument.DocumentType
                        });

                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = mailStaus;
                }

            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Mail Not sent problem in email";
            }

            logger.Info(Util.ClientIP + "|" + serviceRegistraionDocument.DocumentType + " Document sent to " + serviceRegistraionDocument.Email + " and response is " + response.Message);


            
            return response;
        }

        public List<ServiceRegistraionDocumentModel> GetInitialDocument(int periods, string status)
        {
            List<ServiceRegistraionDocumentModel> response = new List<ServiceRegistraionDocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int startDateDiff = 0;
                int endDateDiff = 0;
                if (periods==4)
                {
                    startDateDiff = -1;
                    endDateDiff = 8;
                }
                else if (periods == 3)
                {
                    startDateDiff = 7;
                    endDateDiff = 16;
                }
                else if (periods == 2)
                {
                    startDateDiff = 15;
                    endDateDiff = 31;
                }
                else if (periods == 1)
                {
                    startDateDiff = 30;
                    endDateDiff = 1000;
                }

                string sqlQuery =
                    @"	select Id, convert(varchar,CreatedDate,103) CreatedDate, Name,Email,Mobile,Message,Documents DocumentNames, " +
                    " Status from InitialDocumentDispatch " +
                    " where DocumentType='Registration' and Status=@status and datediff(d,CreatedDate,getdate()) >@startDateDiff " +
                    "  and datediff(d,CreatedDate,getdate()) < @endDateDiff  order by Id ";
                response = db.Query<ServiceRegistraionDocumentModel>(sqlQuery, new {status, startDateDiff, endDateDiff }).AsList<ServiceRegistraionDocumentModel>();
            }
            return response;
        }

        public List<ServiceRegistraionDocumentModel> GetSendFormDocument(string documentType,string start, string end)
        {
            List<ServiceRegistraionDocumentModel> response = new List<ServiceRegistraionDocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                
                string sqlQuery =
                    @"	select Id, convert(varchar,CreatedDate,103) CreatedDate, Name,Email,Mobile,Message,Documents DocumentNames, " +
                    " Status from InitialDocumentDispatch " +
                    " where DocumentType=@documentType and CreatedDate >@start" +
                    "  and CreatedDate < @end  order by Id ";
                response = db.Query<ServiceRegistraionDocumentModel>(sqlQuery, new { documentType, start, end}).AsList<ServiceRegistraionDocumentModel>();
            }
            return response;
        }

        public ResponseModel PutSendFormDocument(ServiceRegistraionDocumentModel serviceRegistraionDocument)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update InitialDocumentDispatch set Status=@status where Id=@Id ";
                var result = db.Execute(sqlQuery, new
                {
                    serviceRegistraionDocument.Status,
                    serviceRegistraionDocument.Id

                });
                response.IsSuccess = true;
                response.Message = "Status is update as "+ serviceRegistraionDocument.Status;
            }

            logger.Info(Util.ClientIP + "|" + "Document status updated and response is " + response.Message);

            return response;
        }

        public ResponseModel PostUploadDocument(int businessId, int officerId, string serviceCode, string documentType, 
            string filePath, string actualFile)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "Insert into BusinessDocuments(CreatedDate,BusinessProfileId, OfficerId,  " +
                    " ServiceCode, DocumentType,FilePath,ActualFileName) values " +
                    "(Getdate(), @businessId, @officerId, @serviceCode, @documentType, @filePath,@actualFile)";
                var result = db.Execute(sqlQuery, new
                {
                    businessId,
                    officerId,
                    serviceCode,
                    documentType,
                    filePath,
                    actualFile
                });

                response.IsSuccess = true;
                response.Message = "Document uploaded " ;
            }

            logger.Info(Util.ClientIP + "|" + "Document uploaded File Name is " + actualFile);

            return response;
        }

        public List<UploadedDocumentModel> GetUploadedDocument(int businessProfileId)
        {
            List<UploadedDocumentModel> response = new List<UploadedDocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string sqlQuery =
                    @"select  convert(varchar,bd.CreatedDate,103) created,isnull(sd.Name,bd.ServiceCode) ServiceName, " +
                     "       isnull(bo.Name,'') OfficerName,bd.ServiceCode ServiceCode,bd.Id DocumentId,DocumentType,FilePath, ActualFileName," +
                     "       bd.BusinessProfileId  from BusinessDocuments bd  " +
                     "   left join ServicesDefinition sd on sd.Code = bd.ServiceCode " +
                     "   left join BusinessOfficer bo on bd.OfficerId = bo.OfficerId " +
                     "   where deletedDate is null and bd.BusinessProfileId = @businessProfileId " +
                     "   order by bd.CreatedDate desc ";
                response = db.Query<UploadedDocumentModel>(sqlQuery, new { businessProfileId }).AsList<UploadedDocumentModel>();
            }
            return response;
        }

        public ResponseModel DeleteUploadedDocument(int documentId)
        {

            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update BusinessDocuments set DeletedDate=GetDate() where Id=@documentId ";
                var result = db.Execute(sqlQuery, new
                {
                    documentId

                });
                response.IsSuccess = true;
                response.Message = "Removed the document ";

                logger.Info(Util.ClientIP + "|" + "Document removed, Documet id is " + documentId);
            }
            return response;
        }

        public string MaskEmail(string email)
        {
            string _PATTERN = @"(?<=[\w]{1})[\w-\._\+%\\]*(?=[\w]{1}@)|(?<=@[\w]{1})[\w-_\+%]*(?=\.)";
            if (!email.Contains("@"))
                return new String('*', email.Length);
            if (email.Split('@')[0].Length < 4)
                return @"*@*.*";
            return Regex.Replace(email, _PATTERN, m => new string('*', m.Length));
        }

        public ResponseModel GetDocumentFillingOTP(string inviteKey)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select email from BusinessOfficer a " +
                            " join DocumentInvite b on a.OfficerId =b.OfficerId "+
                            " where InviteKey=@inviteKey and b.Status='New'";
                string Email = db.QuerySingleOrDefault<string>(sqlQuery, new { inviteKey });
                if (Email!=null)
                {
                    sqlQuery = @"update DocumentInvite set OTP=@OTP where InviteKey=@inviteKey";
                    string OTP = Guid.NewGuid().ToString();
                    OTP = OTP.Replace("-", "");
                    if (OTP.Length > 8)
                        OTP = OTP.Substring(0,8);

                    db.Execute(sqlQuery, new{ OTP , inviteKey });
                    response = SendDocumentFillingOTP(Email, OTP);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not a valid Key or already used";

                }

                logger.Info(Util.ClientIP + "|" + "OTP send and response is " + response.Message);
            }
            return response;
        }

        public DocumentFillingStart GetDocumentFillingOTP(DocumentFillingOTP inviteOTP)
        {
            DocumentFillingStart response = new DocumentFillingStart() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select ServiceBusinessId,OfficerId from DocumentInvite " +
                            " where InviteKey=@InviteKey and OTP=@OTP and status='New'";
                DocumentFillingStart detail = db.QuerySingleOrDefault<DocumentFillingStart>(sqlQuery, new { inviteOTP.InviteKey, inviteOTP.OTP });
                if (detail != null)
                {
                    sqlQuery = @"update DocumentInvite set OTPIP=@ClientIP, status='Used' where InviteKey=@inviteKey";

                    logger.Info(Util.ClientIP + "|" + "OTP, ServiceBusinessId " + response.ServiceBusinessId + ",OfficerId " + response.OfficerId);

                    db.Execute(sqlQuery, new { inviteOTP.InviteKey,Util.ClientIP });
                    response.ServiceBusinessId = detail.ServiceBusinessId;
                    response.OfficerId = detail.OfficerId;
                    response.IsSuccess = true;
                    response.Message = "Start filling";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Not a valid Key or already used";

                }

                logger.Info(Util.ClientIP + "|" + "OTP verification and response is " + response.Message);
            }
            return response;

        }
    }

    
}
