using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;
using Newtonsoft.Json;
using System.IO;
using System;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Forms.Fields;
using iText.Kernel.Pdf.Action;
using iText.Forms;

namespace RoboDocLib.Services
{
    public class RegistersManager
    {
        string connectionString = "";
        int UserId = 999;
        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public RegistersManager(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }

        

        public string GetPDFPath(string path, string desitinationFile, string htmlString)
        {
            PdfWriter writer=null;
            try
            {

                Random rnd = new Random();
                int myRandomNo = rnd.Next(10000000, 99999999);
                
                desitinationFile = string.Format("{0}-{1}-{2}", "register", myRandomNo, desitinationFile);

                ConverterProperties properties = new ConverterProperties();
                properties.SetCreateAcroForm(true);
                properties.SetBaseUri(path + @"\SourceHTML");
                properties.SetFontProvider(new DefaultFontProvider(true, true, true));
                writer = new PdfWriter(path + @"\Output\register\" + desitinationFile,new WriterProperties().SetFullCompressionMode(true));

                HtmlConverter.ConvertToPdf(htmlString, writer, properties); 
                
            }
            catch { }
            

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(desitinationFile));
        }

        public ResponseModel DeleteRegister(string tableName, string message, int id)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete " + tableName + " where Id=" + id;
                var result = db.Execute(sqlQuery);
                response.IsSuccess = true;
                response.Message = message;
            }
            return response;
        }

        public string DownloadShareholdingsRegister(int businessProfile, int officerId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Directors Shareholdings.html");

            List<MembersRegisterModel> detail = GetMembersRegister(businessProfile, officerId);
            string tableData = "";
            foreach (MembersRegisterModel aam in detail)
            {
                tableData += "<tr>" +

                "<td>" + aam.AllotmentDate + "</td>" +
                "<td>" + aam.AllotmentNo + "</td>" +
                "<td>" + aam.CertificateNo + "</td>" +
                "<td>S$ " + aam.Price + "</td>" +
                "<td>" + aam.SrFrom + "</td>" +
                "<td>" + aam.SrTo + "</td>" +
                "<td>" + aam.Acquisitions + "</td>" +
                "<td>" + aam.Disposals + "</td>" +
                "<td>" + aam.Balance + "</td>" +
                "<td>" + aam.ClassOfShare + "</td>" +
                "</tr>";

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
                htmlString = htmlString.Replace("[{Date-Entry}]", detail[0].EntryDate);
                htmlString = htmlString.Replace("[{Member-Address}]", detail[0].OfficerAddress);
                htmlString = htmlString.Replace("[{Member-BirthDate}]", detail[0].OfficerBirthDate);
                htmlString = htmlString.Replace("[{Member-Identity}]", detail[0].OfficerIdentity);
            }
            else
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", " ");
                htmlString = htmlString.Replace("[{Date-Entry}]", " ");
                htmlString = htmlString.Replace("[{Member-Address}]", " ");
                htmlString = htmlString.Replace("[{Member-BirthDate}]", " ");
                htmlString = htmlString.Replace("[{Member-Identity}]", " ");
            }

            List<OfficersRegisterModel> detail1 = GetOfficersRegister("Directors", businessProfile, officerId);

            if (detail1.Count > 0)
            {
                htmlString = htmlString.Replace("[{Director-Remark1}]", detail1[detail1.Count-1].Remarks1);
                htmlString = htmlString.Replace("[{Director-EntryDate}]", detail1[detail1.Count-1].EntryDate);
                htmlString = htmlString.Replace("[{Director-StartDate}]", detail1[detail1.Count - 1].StartDate);
                htmlString = htmlString.Replace("[{Director-CessationDate}]", detail1[detail1.Count-1].CessationDate);
                htmlString = htmlString.Replace("[{Director-Remark2}]", detail1[detail1.Count - 1].Remarks2);
            }

            return GetPDFPath(Util.DocPath, "Register of Directors Shareholdings.pdf", htmlString);
        }


        #region Application Allotments

        public string DownloadApplicationsAllotments(int businessProfile, int officerId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Applications and Allotments.html");

            List<ApplicationsAllotmentsModel> detail = GetApplicationsAllotments(businessProfile, officerId);
            string tableData = "";
            foreach (ApplicationsAllotmentsModel aam in detail)
            {
                tableData += "<tr>" +
                "<td style='text-align:center'>" + aam.ApplicationDate + "</td>" +
                "<td style='text-align:center'>" + aam.AllotmentDate + "</td>" +
                "<td style='text-align:center'>" + aam.AllotmentNo + "</td>" +
                "<td>" + aam.OfficerName +"<br>"+ aam.OfficerAddress  + "</td>" +
                "<td style='text-align:center'>" + aam.OfficerBirthDate + "</td>" +
                "<td>" + aam.OfficerIdentity + "</td>" +
                "<td style='text-align:center'>" + aam.Applied + "</td>" +
                "<td style='text-align:center'>" + aam.Allotted + "</td>" +
                "<td style='text-align:center'>S$ " + aam.Price + "</td>" +
                "<td style='text-align:center'>S$ " + aam.AmountCalled + "</td>" +
                "<td style='text-align:center'>S$ " + aam.AmountPaid + "</td>" +
                "<td style='text-align:center'>" + aam.Consideration + "</td>" +
                "<td style='text-align:center'>" + aam.CertificateNo + "</td>" +
                "<td style='text-align:center'>" + aam.FolioNo + "</td>" +
                "</tr>";

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }
            else
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", "");
            }
            return GetPDFPath(Util.DocPath, "Register of Applications and Allotments.pdf", htmlString);
        }
        
        public ResponseModel PostApplicationsAllotments(ApplicationsAllotmentsModel allotment)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into ApplicationsAllotments (ApplicationDate,AllotmentDate,AllotmentNo,BusinessProfileId,OfficerId, " +
                                    " Applied,Allotted,Price,AmountCalled,AmountPaid,Consideration,CertificateNo,FolioNo,IdentityType) " +
                                    " values (@ApplicationDate,@AllotmentDate,@AllotmentNo,@BusinessProfileId,@OfficerId,@Applied,@Allotted, " +
                                    " @Price,@AmountCalled,@AmountPaid,@Consideration,@CertificateNo,@FolioNo,@IdentityType)";
                var result = db.Execute(sqlQuery, allotment);
                response.IsSuccess = true;
                response.Message = "Applications Allotments added";
            }
            return response;
        }

        public ResponseModel PutApplicationsAllotments(ApplicationsAllotmentsModel allotment)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ApplicationsAllotments set  ApplicationDate=@ApplicationDate,AllotmentDate=@AllotmentDate, " +
                " AllotmentNo=@AllotmentNo,OfficerId=@OfficerId, " +
                " Applied=@Applied,Allotted=@Allotted,Price=@Price,AmountCalled=@AmountCalled,AmountPaid=@AmountPaid, " +
                " Consideration=@Consideration,CertificateNo=@CertificateNo,FolioNo=@FolioNo,IdentityType=@IdentityType " +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, allotment);
                response.IsSuccess = true;
                response.Message = "ApplicationsAllotments modified";
            }
            return response;
        }

        public List<ApplicationsAllotmentsModel> GetApplicationsAllotments(int businessProfileId, int officerId)
        {
            List<ApplicationsAllotmentsModel> response = new List<ApplicationsAllotmentsModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,ApplicationDate,103) ApplicationDate,convert(varchar,AllotmentDate,103) AllotmentDate,AllotmentNo,a.BusinessProfileId,a.OfficerId,b.Name OfficerName, " +
                                    " c.Name BusinessProfileName, Applied,Allotted,Price,AmountCalled,AmountPaid,Consideration,CertificateNo,FolioNo,IdentityType, " +
                                    " convert(varchar,BirthDate,103) OfficerBirthDate, isnull(Address,' ') OfficerAddress, isnull(Nationality,' ') + '<br>' + "+
                                    " case when IdentityType = 'NRIC And Passport' then 'NRIC: ' + isnull(NRICNO, '') + ', Passport: ' + isnull(PassportNo, '') else " +
                                    " case when IdentityType = 'NRIC' then 'NRIC: ' + isnull(NRICNO, '') else " +
                                    " case when IdentityType = 'Passport' then 'Passport: ' + isnull(PassportNO, '') else " +
                                    " case when IdentityType = 'FIN' then 'FIN: ' + isnull(FINNO, '') else 'no' end " +
                                    "     end  end end OfficerIdentity,IdentityType " +
                                    "from ApplicationsAllotments a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and a.OfficerId = b.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id where a.BusinessProfileId = @businessProfileId ";
                if (officerId > 0)
                    sqlQuery += " and a.OfficerId = @officerId ";

                sqlQuery += " order by AllotmentNo desc";

                response = db.Query<ApplicationsAllotmentsModel>(sqlQuery, new { businessProfileId, officerId }).AsList<ApplicationsAllotmentsModel>();
            }
            return response;
        }

        #endregion

        #region Members Registers

        public string DownloadMembersRegister(int businessProfile, int officerId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Members.html");

            List<MembersRegisterModel> detail = GetMembersRegister(businessProfile, officerId);
            string tableData = "";
            foreach (MembersRegisterModel aam in detail)
            {
                tableData += "<tr>" +

                "<td style='text-align:center'>" + aam.AllotmentDate + "</td>" +
                "<td style='text-align:center'>" + aam.AllotmentNo + "</td>" +
                "<td style='text-align:center'>" + aam.CertificateNo + "</td>" +
                "<td style='text-align:center'>S$ " + aam.Price + "</td>" +
                "<td style='text-align:center'>" + aam.SrFrom + "</td>" +
                "<td style='text-align:center'>" + aam.SrTo+ "</td>" +
                "<td style='text-align:center'>" + aam.Acquisitions+ "</td>" +
                "<td style='text-align:center'>" + aam.Disposals + "</td>" +
                "<td style='text-align:center'>" + aam.Balance + "</td>" +
                "<td style='text-align:center'>" + aam.ClassOfShare+ "</td>" +
                "</tr>";

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if(detail.Count>0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
                htmlString = htmlString.Replace("[{Date-Entry}]", detail[0].EntryDate);
                htmlString = htmlString.Replace("[{Member-Address}]", detail[0].OfficerName +"<br>"+ detail[0].OfficerAddress);
                htmlString = htmlString.Replace("[{Member-BirthDate}]", detail[0].OfficerBirthDate);
                htmlString = htmlString.Replace("[{Member-Identity}]", detail[0].OfficerIdentity);
            }
            else
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", " ");
                htmlString = htmlString.Replace("[{Date-Entry}]", " ");
                htmlString = htmlString.Replace("[{Member-Address}]", " ");
                htmlString = htmlString.Replace("[{Member-BirthDate}]", " ");
                htmlString = htmlString.Replace("[{Member-Identity}]", " ");
            }

            return GetPDFPath(Util.DocPath, "Register of Members.pdf", htmlString);
        }


        public ResponseModel PostMembersRegister(MembersRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into MembersShareRegister (BusinessProfileId,OfficerId,EntryDate,AllotmentDate,AllotmentNo," +
                                    " CertificateNo,Price,SrFrom,SrTo,Acquisitions,Disposals,Balance,ClassOfShare,IdentityType ) " +
                                    " values (@BusinessProfileId,@OfficerId,@EntryDate,@AllotmentDate,@AllotmentNo, " +
                                    " @CertificateNo,@Price,@SrFrom,@SrTo,@Acquisitions,@Disposals,@Balance,@ClassOfShare,@IdentityType )";
                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Details added in Member Register";
            }
            return response;
        }

        public ResponseModel PutMembersRegister(MembersRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update MembersShareRegister set OfficerId=@OfficerId,EntryDate=@EntryDate,AllotmentDate=@AllotmentDate,AllotmentNo=@AllotmentNo,  " +
                " CertificateNo=@CertificateNo,Price=@Price,SrFrom=@SrFrom,SrTo=@SrTo,Acquisitions=@Acquisitions, " +
                " Disposals= @Disposals,Balance=@Balance,ClassOfShare=@ClassOfShare,IdentityType=@IdentityType " +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Members Register modified";
            }
            return response;
        }

        public List<MembersRegisterModel> GetMembersRegister(int businessProfileId, int officerId)
        {
            List<MembersRegisterModel> response = new List<MembersRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,EntryDate,103) EntryDate,convert(varchar,AllotmentDate,103) AllotmentDate,AllotmentNo,a.BusinessProfileId,a.OfficerId,b.Name OfficerName, " +
                                    " c.Name BusinessProfileName, CertificateNo,Price,SrFrom,SrTo,Acquisitions,Disposals,Balance,ClassOfShare, " +
                                    " convert(varchar,BirthDate,103) OfficerBirthDate, isnull(Address,' ') OfficerAddress, isnull(Nationality,' ') + '<br> ' +  "+
                                    " case when IdentityType = 'NRIC And Passport' then 'NRIC: ' + isnull(NRICNO, '') + ', Passport: ' + isnull(PassportNo, '') else " +
                                    " case when IdentityType = 'NRIC' then 'NRIC: ' + isnull(NRICNO, '') else " +
                                    " case when IdentityType = 'Passport' then 'Passport: ' + isnull(PassportNO, '') else " +
                                    " case when IdentityType = 'FIN' then 'FIN: ' + isnull(FINNO, '') else 'no' end " +
                                    "     end  end end OfficerIdentity,IdentityType " +
                                    " from MembersShareRegister a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and a.OfficerId = b.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";
                if (officerId > 0)
                    sqlQuery += " and a.OfficerId = @officerId ";

                sqlQuery += " order by AllotmentDate";

                response = db.Query<MembersRegisterModel>(sqlQuery, new { businessProfileId, officerId }).AsList<MembersRegisterModel>();
            }
            return response;
        }

        #endregion

        #region Register of Transfers

        public string DownloadTransfersRegister(int businessProfile, int transferorId, int transfereeId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Transfers.html");

            List<TransfersRegisterModel> detail = GetTransfersRegister(businessProfile, transferorId, transfereeId);
            string tableData = "";
            foreach (TransfersRegisterModel aam in detail)
            {
                tableData += "<tr>" +

                "<td style='text-align:center'>" + aam.TransferNo + "</td>" +
                "<td style='text-align:center'>" + aam.TransferDate + "</td>" +
                "<td >" + aam.TransferorName + "<br>" + aam.TransferorAddress+ "</td>" +
                "<td style='text-align:center'>" + aam.TransferorFolioNo+ "</td>" +
                "<td style='text-align:center'>" + aam.TransferorNoShares + "</td>" +
                "<td style='text-align:center'>" + aam.TransferorCertificateNo + "</td>" +
                "<td style='text-align:center'>" + aam.BalanceNoShares + "</td>" +
                "<td style='text-align:center'>" + aam.BalanceCertificateNo + "</td>" +
                "<td >" + aam.TransfereeName + "<br> " + aam.TransfereeAddress + "</td>" +
                "<td style='text-align:center'>" + aam.TransfereeNoShares + "</td>" +
                "<td style='text-align:center'>" + aam.TransfereeCertificateNo + "</td>" +
                "<td style='text-align:center'>" + aam.TransfereeFolioNo + "</td>" +

                "</tr>";

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }

            return GetPDFPath(Util.DocPath, "Register of Transfers.pdf", htmlString);
        }


        public ResponseModel PostTransfersRegister(TransfersRegisterModel transfers)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into TransfersRegister " +
                                    " (BusinessProfileId,TransferNo,TransferDate,TransferorId,TransferorFolioNo,TransferorNoShares,TransferorCertificateNo," +
                                    " BalanceNoShares,BalanceCertificateNo,TransfereeId,TransfereeFolioNo,TransfereeNoShares,TransfereeCertificateNo) " +
                                    " values (@BusinessProfileId,@TransferNo,@TransferDate,@TransferorId,@TransferorFolioNo,@TransferorNoShares,@TransferorCertificateNo," +
                                    " @BalanceNoShares,@BalanceCertificateNo,@TransfereeId,@TransfereeFolioNo,@TransfereeNoShares,@TransfereeCertificateNo )";
                var result = db.Execute(sqlQuery, transfers);
                response.IsSuccess = true;
                response.Message = "Details added in Transfers Register";
            }
            return response;
        }

        public ResponseModel PutTransfersRegister(TransfersRegisterModel transfers)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update TransfersRegister set BusinessProfileId=@BusinessProfileId,TransferNo=@TransferNo,TransferDate=@TransferDate,TransferorId=@TransferorId,  " +
                " TransferorFolioNo=@TransferorFolioNo,TransferorNoShares=@TransferorNoShares,TransferorCertificateNo=@TransferorCertificateNo, " +
                " BalanceNoShares=@BalanceNoShares,BalanceCertificateNo=@BalanceCertificateNo,TransfereeId=@TransfereeId, " +
                " TransfereeFolioNo=@TransfereeFolioNo,TransfereeNoShares=@TransfereeNoShares,TransfereeCertificateNo=@TransfereeCertificateNo where Id=@Id";

                var result = db.Execute(sqlQuery, transfers);
                response.IsSuccess = true;
                response.Message = "Transfers Register modified";
            }
            return response;
        }

        public List<TransfersRegisterModel> GetTransfersRegister(int businessProfileId, int transferorId, int transfereeId)
        {
            List<TransfersRegisterModel> response = new List<TransfersRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,a.BusinessProfileId,TransferNo,convert(varchar,TransferDate,103) TransferDate,TransferorId,b.Name TransferorName, " +
                                    " TransferorFolioNo,TransferorNoShares,TransferorCertificateNo,BalanceNoShares,BalanceCertificateNo, " +
                                    " TransfereeId,e.Name TransfereeName,TransfereeFolioNo,TransfereeNoShares,TransfereeCertificateNo, " +
                                    " c.Name BusinessProfileName,isnull(b.Address,' ') TransferorAddress,isnull(e.Address,' ') TransfereeAddress  from TransfersRegister a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and TransferorId = b.OfficerId " +
                                    " join BusinessOfficer e on a.BusinessProfileId = e.BusinessProfileId and TransfereeId = e.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";
                if (transferorId > 0)
                    sqlQuery += " and transferorId = @transferorId ";
                if (transfereeId > 0)
                    sqlQuery += " and transfereeId = @transfereeId ";

                sqlQuery += " order by TransferDate";

                response = db.Query<TransfersRegisterModel>(sqlQuery, new { businessProfileId, transferorId, transfereeId }).AsList<TransfersRegisterModel>();
            }
            return response;
        }

        public ResponseModel DeleteTransfersRegister(int id)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete TransfersRegister where Id=@Id";
                var result = db.Execute(sqlQuery, new { id });
                response.IsSuccess = true;
                response.Message = "Transfers Register deleted";
            }
            return response;
        }
        #endregion

        #region Officers Registers

        public string DownloadOfficersRegister(string userRole,int businessProfile, int officerId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of "+ userRole + ".html");

            List<OfficersRegisterModel> detail = GetOfficersRegister(userRole,businessProfile, officerId);
            string tableData = "";
            int i = 1;
            foreach (OfficersRegisterModel aam in detail)
            {
                tableData += "<tr>" +

                "<td  style='text-align:center'>" + i + "</td>" +
                "<td  style='text-align:center'>" + aam.EntryDate + "</td>" +
                "<td>" + aam.OfficerName + "<br> " + aam.OfficerAddress+ " </td>" +
                "<td  style='text-align:center'>" + aam.OfficerBirthDate+ "</td>" +
                "<td  >" + aam.OfficerIdentity + "</td>" +
                "<td  style='text-align:center'>" + aam.StartDate + "</td>" +
                "<td  style='text-align:center'>" + aam.CessationDate + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks1 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks2 + "</td>" +
                "</tr>";
                i++;

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }

            return GetPDFPath(Util.DocPath, "Register of " + userRole + ".pdf", htmlString);
        }

        public ResponseModel PostOfficersRegister(string userRole, OfficersRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into OfficersRegister(UserRole,BusinessProfileId,EntryDate,OfficerId,StartDate,CessationDate,Remarks1,Remarks2,IdentityType ) " +
                                    " values ('"+ userRole + "',@BusinessProfileId,@EntryDate,@OfficerId,@StartDate,@CessationDate,@Remarks1,@Remarks2,@IdentityType)";
                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Details added in " + userRole + " Register";
            }
            return response;
        }

        public ResponseModel PutOfficersRegister(string userRole, OfficersRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update OfficersRegister set BusinessProfileId=@BusinessProfileId,EntryDate=@EntryDate,IdentityType=@IdentityType,  " +
                " OfficerId=@OfficerId,StartDate=@StartDate,CessationDate=@CessationDate,Remarks1=@Remarks1,Remarks2=@Remarks2" +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = userRole + " Register modified";
            }
            return response;
        }

        public List<OfficersRegisterModel> GetOfficersRegister(string userRole, int businessProfileId, int officerId)
        {
            List<OfficersRegisterModel> response = new List<OfficersRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,EntryDate,103) EntryDate,a.BusinessProfileId,a.OfficerId,b.Name OfficerName, " +
                                    " c.Name BusinessProfileName, convert(varchar,StartDate,103) StartDate,convert(varchar,CessationDate,103)CessationDate,Remarks1,Remarks2, " +
                                    " convert(varchar,BirthDate,103) OfficerBirthDate, isnull(Address,' ') OfficerAddress, isnull(Nationality,' ') + '<br> ' + " +
                                     " case when IdentityType = 'NRIC And Passport' then 'NRIC: ' + isnull(NRICNO, '') + ', Passport: ' + isnull(PassportNo, '') else " +
                                    " case when IdentityType = 'NRIC' then 'NRIC: ' + isnull(NRICNO, '') else " +
                                    " case when IdentityType = 'Passport' then 'Passport: ' + isnull(PassportNO, '') else " +
                                    " case when IdentityType = 'FIN' then 'FIN: ' + isnull(FINNO, '') else 'no' end " +
                                    "     end  end end OfficerIdentity,IdentityType " +
                                    "  from OfficersRegister a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and a.OfficerId = b.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.userRole='"+ userRole + "' and a.BusinessProfileId = @businessProfileId ";
                if (officerId > 0)
                    sqlQuery += " and a.OfficerId = @officerId ";

                sqlQuery += " order by EntryDate";

                response = db.Query<OfficersRegisterModel>(sqlQuery, new { businessProfileId, officerId }).AsList<OfficersRegisterModel>();
            }
            return response;
        }


        #endregion

        #region Auditors Registers

        public string DownloadAuditorsRegister(int businessProfile)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Auditors.html");

            List<AuditorsRegisterModel> detail = GetAuditorsRegister(businessProfile);
            string tableData = "";
            int i = 1;
            foreach (AuditorsRegisterModel aam in detail)
            {
                tableData += "<tr>" +

                "<td  style='text-align:center'>" + i + "</td>" +
                "<td  style='text-align:center'>" + aam.EntryDate + "</td>" +
                "<td>" + aam.Name + "<br>" + aam.Address + " </td>" +
                "<td style='text-align:center'>" + aam.RegistrationNo + "</td>" +
                "<td style='text-align:center'>" + aam.StartDate + "</td>" +
                "<td style='text-align:center'>" + aam.CessationDate + "</td>" +
                "<td>" + aam.Remarks1 + "</td>" +
                "<td>" + aam.Remarks2 + "</td>" +
                "<td>" + aam.Remarks3 + "</td>" +
                "</tr>";
                i++;

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }

            return GetPDFPath(Util.DocPath, "Register of Auditors.pdf", htmlString);
        }

        public ResponseModel PostAuditorsRegister(AuditorsRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into AuditorsRegister(BusinessProfileId,Name,Address,RegistrationNo,EntryDate,StartDate,CessationDate,Remarks1,Remarks2,Remarks3) " +
                                    " values (@BusinessProfileId,@Name,@Address,@RegistrationNo,@EntryDate,@StartDate,@CessationDate,@Remarks1,@Remarks2,@Remarks3)";
                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Details added in Auditor Register";
            }
            return response;
        }

        public ResponseModel PutAuditorsRegister(AuditorsRegisterModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update AuditorsRegister set BusinessProfileId=@BusinessProfileId,EntryDate=@EntryDate,Name=@Name,Address=@Address,  " +
                " RegistrationNo=@RegistrationNo, StartDate=@StartDate,CessationDate=@CessationDate,Remarks1=@Remarks1,Remarks2=@Remarks2,Remarks3=@Remarks3" +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Auditor Register modified";
            }
            return response;
        }

        public List<AuditorsRegisterModel> GetAuditorsRegister(int businessProfileId)
        {
            List<AuditorsRegisterModel> response = new List<AuditorsRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,EntryDate,103) EntryDate,a.BusinessProfileId,a.Name,a.Address,RegistrationNo, " +
                                    " c.Name BusinessProfileName, convert(varchar,StartDate,103) StartDate,convert(varchar,CessationDate,103)CessationDate," +
                                    " Remarks1,Remarks2,Remarks3  from AuditorsRegister a " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";

                sqlQuery += " order by EntryDate";

                response = db.Query<AuditorsRegisterModel>(sqlQuery, new { businessProfileId }).AsList<AuditorsRegisterModel>();
            }
            return response;
        }


        #endregion

        #region Mortgages and Charges

        public string DownloadMortgagesCharges(int businessProfile)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Mortgages and Charges.html");

            List<MortgagesChargesModel> detail = GetMortgagesCharges(businessProfile);
            string tableData = "";
 
            foreach (MortgagesChargesModel aam in detail)
            {
                tableData += "<tr>" +

                "<td style='text-align:center'>" + aam.EntryDate + "</td>" +
                "<td>" + aam.Name + "<br>" + aam.Address + " </td>" +
                "<td style='text-align:center'>" + aam.ShortDescription + "</td>" +
                "<td style='text-align:center'>" + aam.Amount + "</td>" +
                "<td style='text-align:center'>" + aam.RegistrarDate + " <br> " + aam.NoOfCertificate + "</td>" +
                "<td style='text-align:center'>" + aam.DischargeDate + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks + "</td>" +
                "</tr>";
            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }

            return GetPDFPath(Util.DocPath, "Register of Mortgages and Charges.pdf", htmlString);
        }


        public ResponseModel PostMortgagesCharges(MortgagesChargesModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into MortgagesCharges(BusinessProfileId,Name,Address,EntryDate,ShortDescription,Amount,RegistrarDate,NoOfCertificate,DischargeDate,Remarks) " +
                                    " values (@BusinessProfileId,@Name,@Address,@EntryDate,@ShortDescription,@Amount,@RegistrarDate,@NoOfCertificate,@DischargeDate,@Remarks)";
                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Details added in Mortgages and Charges";
            }
            return response;
        }

        public ResponseModel PutMortgagesCharges(MortgagesChargesModel members)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update MortgagesCharges set BusinessProfileId=@BusinessProfileId,EntryDate=@EntryDate,Name=@Name,Address=@Address,  " +
                " ShortDescription=@ShortDescription,Amount=@Amount,RegistrarDate=@RegistrarDate,NoOfCertificate=@NoOfCertificate,DischargeDate=@DischargeDate,Remarks=@Remarks" +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, members);
                response.IsSuccess = true;
                response.Message = "Mortgages and Charges modified";
            }
            return response;
        }

        public List<MortgagesChargesModel> GetMortgagesCharges(int businessProfileId)
        {
            List<MortgagesChargesModel> response = new List<MortgagesChargesModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,EntryDate,103) EntryDate,a.BusinessProfileId,a.Name,a.Address, " +
                                    " c.Name BusinessProfileName, convert(varchar,RegistrarDate,103) RegistrarDate,convert(varchar,DischargeDate,103)DischargeDate, " +
                                    " ShortDescription,Amount,NoOfCertificate,Remarks from MortgagesCharges a " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";

                sqlQuery += " order by EntryDate";

                response = db.Query<MortgagesChargesModel>(sqlQuery, new { businessProfileId }).AsList<MortgagesChargesModel>();
            }
            return response;
        }


        #endregion

        #region Controllers Corporate Registers

        public string DownloadControllersCorporateRegister(int businessProfile)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Controllers-Corporate.html");

            List<ControllersCorporateRegisterModel> detail = GetControllersCorporateRegister(businessProfile);
            string tableData = "";
            int i = 1;
            foreach (ControllersCorporateRegisterModel aam in detail)
            {
                if (i>1)
                    tableData += "<div style='page-break-after:always;'></div>";

                tableData += "<table class='table' border='1' cellpadding='3' style='width:290mm !important;margin-top:4px !important;font-size:12px;'>" +
                "<tr>" +
                "<td style='width:10mm;text-align:center'>No.</td>" +
                "<td style='width:140mm;text-align:center'>Required Details</td>" +
                "<td style='width:140mm;text-align:center'>Furnished Particulars</td>" +
                "<tr><td style='text-align:center'>1</td><td>Last Updated Date</td><td>" + aam.EntryDate + "</td></tr>" +
                "<tr><td style='text-align:center'>2</td><td>Name of the Entity</td><td>" + aam.Name + "</td></tr>" +
                "<tr><td style='text-align:center'>3</td><td>Unique entity number issued by the Registrar, if any</td><td>" + aam.RegistrationNo + "</td></tr>" +
                "<tr><td style='text-align:center'>4</td><td>Address of registered office</td><td>" + aam.Address + "</td>" +
                "<tr><td style='text-align:center'>5</td><td>Legal form of the registrable corporate controller</td><td>" + aam.LegalForm + "</td></tr>" +
                "<tr><td style='text-align:center'>6</td><td>Jurisdiction where the registrable controller is formed of incorporated</td><td>" + aam.Jurisdiction + "</td></tr>" +
                "<tr><td style='text-align:center'>7</td><td>Statute under which the registrable controller is formed of incorporated</td><td>" + aam.Statue + "</td></tr>" +
                "<tr><td style='text-align:center'>8</td><td>Name of corporate entity register of the jurisdiction in which the registrable corporate controller is formed or incorporated, if applicable</td><td> " + aam.IdentityNo + " </td> </tr>" +
                "<tr><td style='text-align:center'>9</td><td>Identification number or registration number of the registrable corporate controller on the corporate entity register of the jurisdiction where the registrable corporate controller is formed or incorporated, if applicable </td><td> " + aam.RegisterName + " </td> </tr>" +
                "<tr><td style='text-align:center'>10</td><td>Date on which the registrable corporate controller became a corporate controller of the company or foreign company (as the case may be) </td><td> " + aam.StartDate + " </td> </tr>" +
                "<tr><td style='text-align:center'>11</td><td>Date on which the registrable corporate controller ceased to be a corporate controller of the company or foreign company (as the case may be) </td><td> " + aam.CessationDate + " </td></tr> "+
                "</table>";
                i++;
            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);
            }

            return GetPDFPath(Util.DocPath, "Register of Controllers-Corporate.pdf", htmlString);
        }

        public ResponseModel PostControllersCorporateRegister(ControllersCorporateRegisterModel corporate)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" insert into ControllersCorporateRegister(BusinessProfileId,Name,RegistrationNo,Address,EntryDate, " +
                                    " LegalForm,Jurisdiction,Statue,IdentityNo,RegisterName,StartDate,CessationDate) " +
                                    " values (@BusinessProfileId,@Name,@RegistrationNo,@Address,@EntryDate, " +
                                    " @LegalForm,@Jurisdiction,@Statue,@IdentityNo,@RegisterName,@StartDate,@CessationDate)";
                var result = db.Execute(sqlQuery, corporate);
                response.IsSuccess = true;
                response.Message = "Details added in Controllers Corporate Register";
            }
            return response;
        }

        public ResponseModel PutControllersCorporateRegister(ControllersCorporateRegisterModel corporate)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update ControllersCorporateRegister set BusinessProfileId=@BusinessProfileId,EntryDate=@EntryDate,Name=@Name,Address=@Address,  " +
                " RegistrationNo=@RegistrationNo, StartDate=@StartDate,CessationDate=@CessationDate,LegalForm=@LegalForm,Jurisdiction=@Jurisdiction," +
                                    " Statue=@Statue,IdentityNo=@IdentityNo,RegisterName=@RegisterName where Id=@Id";

                var result = db.Execute(sqlQuery, corporate);
                response.IsSuccess = true;
                response.Message = "Controllers Corporate Register modified";
            }
            return response;
        }

        public List<ControllersCorporateRegisterModel> GetControllersCorporateRegister(int businessProfileId)
        {
            List<ControllersCorporateRegisterModel> response = new List<ControllersCorporateRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,convert(varchar,EntryDate,103) EntryDate,a.BusinessProfileId,a.Name,a.Address,RegistrationNo, " +
                                    " c.Name BusinessProfileName, convert(varchar,StartDate,103) StartDate,convert(varchar,CessationDate,103)CessationDate," +
                                    " LegalForm,Jurisdiction,Statue,IdentityNo,RegisterName  from ControllersCorporateRegister a " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";

                sqlQuery += " order by EntryDate";

                response = db.Query<ControllersCorporateRegisterModel>(sqlQuery, new { businessProfileId }).AsList<ControllersCorporateRegisterModel>();
            }
            return response;
        }


        #endregion

        #region Nominee Registers

        public List<DropDownModel> GetNominatorDown(int BusinessProfileId)
        {
            List<DropDownModel> result = null;
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"select 'C:'+convert(varchar,id) Value,name + '(Corporate)' Text from ControllersCorporateRegister " +
                             " union all "+
                             " select 'I:' + convert(varchar,a.OfficerId),Name + '(Individual)'  from OfficersRegister a " +
                             " join BusinessOfficer b on a.OfficerId = b.OfficerId " +
                             " where a.UserRole = 'Controllers-Individuals' order by 2";

                result = connection.Query<DropDownModel>(sql,new { BusinessProfileId }).AsList<DropDownModel>();
                result.Insert(0, new DropDownModel() { Value = "0", Text = "All" });
            }
            return result;
        }

        public ResponseModel PostNomineeRegister(NomineeRegisterModel nominee)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Random rnd = new Random();
                int myRandomNo = rnd.Next(10000000, 99999999);
                string sqlQuery = @" insert into NomineeRegister(BusinessProfileId,Nature,OfficerId,NominatorId,NominatorName) " +
                                    " values (@BusinessProfileId,@Nature,@OfficerId,@NominatorId,@myRandomNo)";
                var result = db.Execute(sqlQuery, new { nominee.BusinessProfileId,nominee.Nature,nominee.OfficerId,nominee.NominatorId, myRandomNo });
                if (nominee.Nature.Equals("Individual"))
                sqlQuery = @" update NomineeRegister set nominatorname=name,remarks1=Address,remarks2=convert(varchar,birthdate,103), " +
                            "	remarks3=Nationality,remarks4=isnull(NricNo,PassportNo),remarks5='',  " +
                            "	remarks6=convert(varchar,StartDate,103),remarks7=convert(varchar,CessationDate,103)  " +
                            " from (select a.OfficerId,Name,Address,birthdate,Nationality,NricNo,PassportNo,StartDate,CessationDate from BusinessOfficer a join OfficersRegister b on a.OfficerId = b.OfficerId) a  " +
                            " where NomineeRegister.OfficerId = a.OfficerId and nominatorname='@myRandomNo' ";
                else
                    sqlQuery = @" update NomineeRegister set nominatorname=name,remarks1=Address,remarks2=RegistrationNo, " +
                            "	remarks3=Jurisdiction,remarks4=RegisterName,remarks5=IdentityNo,  " +
                            "	remarks6=convert(varchar,StartDate,103),remarks7=convert(varchar,CessationDate,103)  " +
                            " from ControllersCorporateRegister a  " +
                            " where NomineeRegister.Id= @ID and nominatorname='@myRandomNo'";

                db.Execute(sqlQuery, new {nominee.OfficerId, myRandomNo });

                response.IsSuccess = true;
                response.Message = "Details added in Auditor Register";
            }
            return response;
        }

        public ResponseModel PutNomineeRegister(NomineeRegisterModel nominee)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update NomineeRegister set BusinessProfileId=@BusinessProfileId,Nature=@Nature," +
                " OfficerId=@OfficerId,NominatorId = @NominatorId " +
                                    " where Id=@Id";

                var result = db.Execute(sqlQuery, nominee);
                if (nominee.Nature.Equals("Individual"))
                    sqlQuery = @" update NomineeRegister set nominatorname=name,remarks1=Address,remarks2=convert(varchar,birthdate,103), " +
                                "	remarks3=Nationality,remarks4=isnull(NricNo,PassportNo),remarks5='',  " +
                                "	remarks6=convert(varchar,StartDate,103),remarks7=convert(varchar,CessationDate,103)  " +
                                " from (select a.OfficerId,Name,Address,birthdate,Nationality,NricNo,PassportNo,StartDate,CessationDate from BusinessOfficer a join OfficersRegister b on a.OfficerId = b.OfficerId) a  " +
                                " where NomineeRegister.OfficerId = a.OfficerId and 'I:'+convert(varchar,a.OfficerId) = @NominatorId ";
                else
                    sqlQuery = @" update NomineeRegister set nominatorname=name,remarks1=Address,remarks2=RegistrationNo, " +
                            "	remarks3=Jurisdiction,remarks4=RegisterName,remarks5=IdentityNo,  " +
                            "	remarks6=convert(varchar,StartDate,103),remarks7=convert(varchar,CessationDate,103)  " +
                            " from ControllersCorporateRegister a  " +
                            " where NomineeRegister.Id= @ID and 'C:'+convert(varchar,a.id) = @NominatorId "; 

                db.Execute(sqlQuery, new { nominee.NominatorId, nominee.Id});

                response.IsSuccess = true;
                response.Message = "Auditor Register modified";
            }
			
            return response;
        }

        public List<NomineeRegisterModel> GetNomineeRegister(int businessProfileId, int officerId)
        {
            List<NomineeRegisterModel> response = new List<NomineeRegisterModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,a.BusinessProfileId,b.OfficerId,b.Name OfficerName," +
                                    " c.Name BusinessProfileName,NominatorId,NominatorName,a.Nature   from NomineeRegister a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and a.OfficerId = b.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";

                if (officerId > 0)
                    sqlQuery += " and a.OfficerId = @officerId ";

                sqlQuery += " order by b.Name";

                response = db.Query<NomineeRegisterModel>(sqlQuery, new { businessProfileId, officerId }).AsList<NomineeRegisterModel>();
            }
            return response;
        }


        public string DownloadNomineeRegister(int businessProfileId, int officerId)
        {
            string htmlString = File.ReadAllText(Util.DocPath + @"\SourceHTML\Post Registration 02 SR\Register of Nominee Nominators.html");


            List<NomineeRegisterPDFModel> detail = new List<NomineeRegisterPDFModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select a.Id,a.BusinessProfileId,b.OfficerId,b.Name OfficerName," +
                                    " c.Name BusinessProfileName,NominatorId,NominatorName,a.Nature, remarks1 , remarks2 " +
                                    " , remarks3, remarks4, remarks5, remarks6, remarks7   from NomineeRegister a " +
                                    " join BusinessOfficer b on a.BusinessProfileId = b.BusinessProfileId and a.OfficerId = b.OfficerId " +
                                    " join BusinessProfile c on a.BusinessProfileId = c.Id  where a.BusinessProfileId = @businessProfileId ";

                if (officerId > 0)
                    sqlQuery += " and a.OfficerId = @officerId ";

                sqlQuery += " order by b.Name";

                detail = db.Query<NomineeRegisterPDFModel>(sqlQuery, new { businessProfileId, officerId }).AsList<NomineeRegisterPDFModel>();
            }

            string tableData = "";
            int i = 0;
            foreach (NomineeRegisterPDFModel aam in detail)
            {
                tableData += "<tr>" +

                "<td style='text-align:center'>" + 1 + "</td>" +
                "<td style='text-align:center'>" + aam.Nature + "</td>" +
                "<td>" + aam.NominatorName +"<br>" + aam.Remarks1 + " </td>" +
                "<td style='text-align:center'>" + aam.Remarks2 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks3 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks4 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks5 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks6 + "</td>" +
                "<td style='text-align:center'>" + aam.Remarks7 + "</td>" +

                "</tr>";

            }
            htmlString = htmlString.Replace("[{TABLE-DATA}]", tableData);
            if (detail.Count > 0)
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", detail[0].BusinessProfileName);

            }
            else
            {
                htmlString = htmlString.Replace("[{Entry-Name}]", " ");
            }

            List<OfficersRegisterModel> detail1 = GetOfficersRegister("Directors", businessProfileId, officerId);

            if (detail1.Count > 0)
            {
                htmlString = htmlString.Replace("[{Director-EntryDate}]", detail1[detail1.Count - 1].EntryDate);
                htmlString = htmlString.Replace("[{Director-Name}]", detail1[detail1.Count - 1].OfficerName);
                htmlString = htmlString.Replace("[{Director-BirthDate}]", detail1[detail1.Count - 1].OfficerBirthDate);
                htmlString = htmlString.Replace("[{Director-Nationality}]", "");
                htmlString = htmlString.Replace("[{Director-Identity}]", detail1[detail1.Count - 1].OfficerIdentity);
                htmlString = htmlString.Replace("[{Director-StartDate}]", detail1[detail1.Count - 1].StartDate);
                htmlString = htmlString.Replace("[{Director-CessationDate}]", detail1[detail1.Count - 1].CessationDate);
                htmlString = htmlString.Replace("[{Director-Remark2}]", detail1[detail1.Count - 1].Remarks2);
            }

            return GetPDFPath(Util.DocPath, "Register of Nominee Nominators.pdf", htmlString);
        }


        #endregion
    }
}
