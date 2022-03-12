using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.IO;
using System.Net;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using System;
using iText.Kernel.Pdf;
using iText.Forms.Fields;
using iText.Kernel.Pdf.Action;
using iText.Forms;
using DigiSigner.Client;
using System.Globalization;
using NLog;
using Newtonsoft.Json;

namespace RoboDocLib.Services
{
    public class HTMLDocumentManager
    {
        string connectionString = "";
        int UserId = 999;
        string DigiSignerKey = "77893e42-1090-4823-a518-9337572d2d6d";

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public HTMLDocumentManager(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

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
        public string GetForm(string path, int serviceBusinessId, int officerStepId, string type)
        {
            string htmlString;
            List<DocumentGenerationFieldModel> details;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"select distinct FilePath+'/'+FileName" +
                " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode" +
                " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId" +
                " where sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId";

                string fileName = db.QuerySingle<string>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    });

                htmlString = File.ReadAllText(path + @"\SourceHTML\" + fileName);
                if (type.Equals("HTML"))
                {
                    htmlString = htmlString.Replace("'css/doc-core.css'", "'/css/doc-core.css'");
                    htmlString = htmlString.Replace("'images/logo.png'", "'/images/logo.png'");
                    htmlString = htmlString.Replace("'/api/form-filling'", string.Format("'{0}/{1}/{2}'", "/api/form-filling", serviceBusinessId, officerStepId));
                }
                else
                {
                    htmlString = htmlString.Replace("<form action='/api/form-filling' method='POST'>", "");
                    htmlString = htmlString.Replace("<input type='submit' value='Submit'>", "");
                    htmlString = htmlString.Replace("<input type='submit' value='Submit' onclick='this.disabled=true;this.value=\"please wait.\";this.form.submit();'>", "");
                    htmlString = htmlString.Replace("</form>", "");
                }

                sqlQuery = @" select ControlNumber,df.Keyword,isnull(CapturedValue,' ') CapturedValue," +
                        " Control,isnull(Container,' ') Container,IsRequired,Label,ControlNumber,Nature  " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                        " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode " +
                    " where sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationFieldModel>();

                foreach (var det in details)
                {
                    
                    if (det.Keyword.StartsWith("d:") || det.Keyword.StartsWith("c:") || det.Keyword.StartsWith("s:"))
                    {
                        if (!type.Equals("PREVIEW"))
                            htmlString = htmlString.Replace("[" + det.Keyword + "]", "");
                        else
                            htmlString = htmlString.Replace("[" + det.Keyword + "]", "[" + det.CapturedValue + "]");
                    }
                    else if (det.Keyword.EndsWith("-CSS") && !(det.Keyword.EndsWith("-Yes-CSS")|| det.Keyword.EndsWith("-No-CSS")))
                    {
                        if (det.CapturedValue.Equals("hide-no"))
                            htmlString = htmlString.Replace(det.Keyword, "");
                        else
                            htmlString = htmlString.Replace(det.Keyword, " hide-item ");
                    }
                    else if (det.Nature.Equals("DB"))
                        htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);
                    else if (type.Equals("PREVIEW"))
                    {
                        if (det.Control.Equals("checkbox"))
                        {
                            if (det.CapturedValue.Equals("true"))
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", "<img src='images/check.png'/>");
                            else
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", "<img src='images/uncheck.png'/>");
                        }
                        else if (det.Control.Equals("textarea") && (det.Container.Equals("List") ||
                            det.Container.Equals("Br")))
                        {
                            string[] listRows = det.CapturedValue.Split('\n');
                            if (listRows.Length==1 && det.CapturedValue.Contains("\r"))
                                listRows = det.CapturedValue.Split('\r');


                            string rows = "";
                            foreach(string row in listRows)
                            {
                                
                                if (det.Container.Equals("Br"))
                                    rows += row + "<br>";
                                else
                                    rows += "<li>" + row + "</li>";
                            }
                            htmlString = htmlString.Replace("[{" + det.Keyword + "}]", rows);
                        }
                        else if (det.Control.Equals("currency"))
                            htmlString = htmlString.Replace("[{" + det.Keyword + "}]", GetCurrencyFormat(det.CapturedValue));
                        else
                            htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);

                    }
                    else
                    {
                        if (det.Control.Trim().Length == 0 || det.ControlNumber.ToUpper().Equals("APP"))
                            htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);
                        else if (det.Control.Equals("checkbox"))
                        {
                            if (det.CapturedValue.Equals("true"))
                            {
                                if (!type.Equals("HTML") && det.ControlNumber.ToUpper().Equals("USER"))
                                {
                                    htmlString = htmlString.Replace("[{" + det.Keyword + "}]", "<img src='images/check.png'/>");
                                }
                                else
                                    htmlString = htmlString.Replace("[{" + det.Keyword + "}]", string.Format("{0}'{1}' {2}>", det.Label, det.Keyword, "checked"));

                            }
                            else
                            {
                                if (!type.Equals("HTML") && det.ControlNumber.ToUpper().Equals("USER"))
                                {
                                    htmlString = htmlString.Replace("[{" + det.Keyword + "}]", "<img src='images/uncheck.png'/>");
                                }
                                else
                                    htmlString = htmlString.Replace("[{" + det.Keyword + "}]", string.Format("{0}'{1}' {2}>", det.Label, det.Keyword, ""));

                            }

                        }
                        else if (det.Control.Equals("textarea"))
                        {
                            if (!type.Equals("HTML") && det.ControlNumber.ToUpper().Equals("USER"))
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);
                            else
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", string.Format("{0}'{1}'>{2}</textarea>", det.Label, det.Keyword, det.CapturedValue.Trim()));

                        }
                        else if (det.Control.Equals("select"))
                        {
                            htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);
                        }
                        else
                        {
                            if (!type.Equals("HTML") && det.ControlNumber.ToUpper().Equals("USER"))
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", det.CapturedValue);
                            else
                                htmlString = htmlString.Replace("[{" + det.Keyword + "}]", string.Format("{0}'{1}' value='{2}'>", det.Label, det.Keyword, det.CapturedValue.Trim()));
                        }

                    }

                }

            }

            return htmlString;
        }

        public string GetPDFForm(string path,string myPath, int serviceBusinessId, int officerStepId)
        {
            string desitinationFile = "";
            try
            {

                string htmlString = GetForm(path, serviceBusinessId, officerStepId, (myPath.Equals("PREVIEW")|| myPath.Equals("SIGN") ? "PREVIEW" : "PDF"));
                string fileName;
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = @"select distinct FilePath+'/'+FileName" +
                    " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode" +
                    " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId" +
                    " where sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId";

                    fileName = db.QuerySingle<string>(sqlQuery,
                        new
                        {
                            serviceBusinessId,
                            officerStepId
                        });
                }
                
                Random rnd = new Random();
                int myRandomNo = rnd.Next(10000000, 99999999);
                fileName = fileName.Replace(".html", "");
                if (myPath.Equals("SIGN"))
                {
                    desitinationFile = string.Format("{0}-{1}-{2}-{3}-{4}", fileName, myRandomNo, serviceBusinessId, officerStepId, "-Sign.pdf");
                    myPath = "PREVIEW";
                }
                else
                    desitinationFile = string.Format("{0}-{1}-{2}-{3}-{4}", fileName, myRandomNo, serviceBusinessId, officerStepId,".pdf");

                ConverterProperties properties = new ConverterProperties();
                properties.SetCreateAcroForm(true);
                properties.SetBaseUri(path + @"\SourceHTML");
                properties.SetFontProvider(new DefaultFontProvider(true, true, true));
                PdfWriter writer = new PdfWriter(path +@"\Output\"+desitinationFile,
                new WriterProperties().SetFullCompressionMode(true));

 
                HtmlConverter.ConvertToPdf(htmlString, writer, properties);
                if (!myPath.Equals("PREVIEW"))
                {
                    string editableFile = string.Format("{0}-{1}-{2}-{3}-{4}", fileName, myRandomNo, serviceBusinessId, officerStepId, "-Edit.pdf");
                    PdfDocument pdfDoc = new PdfDocument(new PdfReader(path + @"\Output\" + desitinationFile), new PdfWriter(path + @"\Output\" + editableFile));
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

                    // Being set as true, this parameter is responsible to generate an appearance Stream
                    // while flattening for all form fields that don't have one. Generating appearances will
                    // slow down form flattening, but otherwise Acrobat might render the pdf on its own rules.
                    form.SetGenerateAppearance(true);


                    iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(250, 30, 110, 26);
                    PdfButtonFormField pushButton = PdfFormField.CreatePushButton(pdfDoc, rect, "btnPos", "Submit");

                    //pushButton.SetBackgroundColor(ColorConstants.GRAY);
                    pushButton.SetValue("Submit");
                    pushButton.SetAction(PdfAction.CreateSubmitForm(string.Format("{0}api/form-pdf/{1}/{2}", myPath, serviceBusinessId, officerStepId), null,
                            PdfAction.SUBMIT_HTML_FORMAT));
                    pushButton.SetVisibility(PdfFormField.VISIBLE_BUT_DOES_NOT_PRINT);

                    form.AddField(pushButton);

                    pdfDoc.Close();

                    desitinationFile = editableFile;

                }
            }
            catch { }

            logger.Info(Util.ClientIP + "|" + "PDF form generated for Service Business Id " + serviceBusinessId + ", officer Step Id " + officerStepId + ", File Name  " +  desitinationFile);

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(desitinationFile));
        }

        public ResponseModel DispatchDocument(string path, ServicesSignSave serviceSign)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string fileName = GetPDFForm(path, "SIGN", serviceSign.ServiceBusinessId, int.Parse(serviceSign.OfficerStepIds));
                
                string sqlQuery = @"update ServiceBusinessOfficer set status='Signing Pending', GeneratedFileName=@fileName,UpdatedDate=GetDate() where Id=@OfficerStepIds";
                db.Execute(sqlQuery, new { fileName, serviceSign.OfficerStepIds });


                sqlQuery = @" select GeneratedFileName DestinationFile,CapturedValue Executor,CapturedBy Email from ServiceBusinessOfficer sbo " +
            " join ServiceBusinessFields sbf on sbo.Id =sbf.OfficerStepId and sbo.ServiceBusinessId=sbf.ServiceBusinessId " +
            " where sbf.ServiceBusinessId=@ServiceBusinessId and sbf.OfficerStepId = @OfficerStepIds and ltrim(rtrim(isnull(CapturedBy,'')))<>'' and Keyword like 's:%' ";

                List<DocumentGenerationSignModel> emails = db.Query<DocumentGenerationSignModel>(sqlQuery,
                    new
                    {
                        serviceSign.ServiceBusinessId,
                        serviceSign.OfficerStepIds
                    }).AsList<DocumentGenerationSignModel>();

                if (emails.Count>0)
                {
                    fileName = SendDocument(path, emails);

                    sqlQuery = @"update ServiceBusinessOfficer set digiSignCode=@fileName,UpdatedDate=GetDate() where Id=@OfficerStepIds";
                    db.Execute(sqlQuery, new { fileName, serviceSign.OfficerStepIds });

                    response.IsSuccess = true;
                    response.Message = "Document sent for digital signature, Please check your email inbox";
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Document sending failed due to non availability of Email";

                }

            }

            logger.Info(Util.ClientIP + "|" + "Dispatch Document for Service Business Id " + serviceSign.ServiceBusinessId + ", officer Step Id " + serviceSign.OfficerStepIds + ", Status " + response.Message);

            return response;
        }
        public string PostForm(string formData, int serviceBusinessId, int officerStepId)
        {
            string[] data = formData.Split('&');
            Dictionary<string, string> postedData = new Dictionary<string, string>();
            string[] keyvaluedata = null;
            foreach (string keyvalue in data)
            {
                keyvaluedata = keyvalue.Split('=');
                if (!postedData.ContainsKey(WebUtility.UrlDecode(keyvaluedata[0])))
                    postedData.Add(WebUtility.UrlDecode(keyvaluedata[0]), WebUtility.UrlDecode(keyvaluedata[1]));
            }

            logger.Info(Util.ClientIP + "|" + "Document Modified for Service Business Id " + serviceBusinessId + ", officer Step Id " + officerStepId);

            return PostForm(postedData, serviceBusinessId, officerStepId, "HTML");

        }

        public string PostOfflineForm(string filepath, int serviceBusinessId, int officerStepId)
        {
            PdfReader reader = new PdfReader(filepath);
            PdfDocument document = new PdfDocument(reader);
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(document, false);
            IDictionary<String, PdfFormField> fields = acroForm.GetFormFields();
           
            Dictionary<string, string> postedData = new Dictionary<string, string>();
            PdfFormField toSet;

            foreach (string keyvalue in fields.Keys)
            {
                if (!postedData.ContainsKey(keyvalue))
                {
                    fields.TryGetValue(keyvalue, out toSet);
                    if (keyvalue.StartsWith("C:"))
                    {
                        if (!toSet.GetValueAsString().Equals("Off"))
                            postedData.Add(keyvalue, "true");
                        else
                            postedData.Add(keyvalue, ""); ;
                    }
                    else
                        postedData.Add(keyvalue, toSet.GetValueAsString());
                }
                    
            }
            string val = "";
            List<DocumentGenerationFieldModel> details;
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string sqlQuery = @" select sbf.Id ServiceBusinessId,df.Keyword,Control,isRequired " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                        " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode " +
                    " where Nature='html' and ControlNumber ='Client' and Control='textarea' and sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId order by isRequired ";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationFieldModel>();
                string capturedValue;
                for (int i = 0; i < details.Count; i++)
                {
                    if (i == 0)
                        capturedValue = "TextArea";
                    else
                        capturedValue = "TextArea_" + details[i].IsRequired;
                    if (!postedData.ContainsKey(details[i].Keyword))
                    {
                        if (postedData.TryGetValue(capturedValue, out val))
                        {
                            postedData.Add(details[i].Keyword, val.Replace("\\r\\n", "\n"));
                        }

                    }
                }
            }

            logger.Info(Util.ClientIP + "|" + "Document Modified from Offline Editable PDF for Service Business Id " + serviceBusinessId + ", officer Step Id " + officerStepId);

            return PostForm(postedData, serviceBusinessId, officerStepId, "PDF");

        }


        public string PostPDFForm(string formData, int serviceBusinessId, int officerStepId)
        {
            bool inWhile = true;
            int start = 0;
            int end = 1;
            Dictionary<string, string> postedData = new Dictionary<string, string>();
            string key = "";
            string val = "";
            while (inWhile)
            {
                start = formData.IndexOf("/T(", start);
                if (start < 0)
                {
                    inWhile = false;
                    break;
                }
                end = formData.IndexOf(")/V", start);
                key = formData.Substring(start + 3, end - start - 3);
                
                if (postedData.ContainsKey(key))
                    continue;

                if (key.StartsWith("C:"))
                {
                    start = formData.IndexOf("/V/", start);
                    end = formData.IndexOf(">>", start);
                }
                else
                {
                    start = formData.IndexOf("/V(", start);
                    end = formData.IndexOf(")>>", start);
                }
                val = formData.Substring(start + 3, end - start - 3);
                if (key.StartsWith("C:"))
                {
                    if (val.Equals("Yes"))
                        val = "true";
                    else
                        val = "";
                }

                postedData.Add(key, val);
            }
            List<DocumentGenerationFieldModel> details;
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string sqlQuery = @" select sbf.Id ServiceBusinessId,df.Keyword,Control,isRequired " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                        " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode " +
                    " where Nature='html' and ControlNumber ='Client' and Control='textarea' and sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId order by isRequired ";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationFieldModel>();
                string capturedValue;
                for (int i=0;i<details.Count;i++)
                {
                    if (i == 0)
                        capturedValue = "TextArea";
                    else
                        capturedValue = "TextArea_"+ details[i].IsRequired;

                    if (!postedData.ContainsKey(details[i].Keyword))
                    {
                        if (postedData.TryGetValue(capturedValue, out val))
                        {
                            postedData.Add(details[i].Keyword, val.Replace("\\r\\n", "\n"));
                        }

                    }

                }
            }

            logger.Info(Util.ClientIP + "|" + "Document Modified from PDF for Service Business Id " + serviceBusinessId + ", officer Step Id " + officerStepId);

            return PostForm(postedData, serviceBusinessId, officerStepId, "PDF");
        }
        public string PostForm(Dictionary<string,string> postedData, int serviceBusinessId, int officerStepId,string type)
        {
            
            List<DocumentGenerationFieldModel> details;
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string sqlQuery = @" select sbf.Id ServiceBusinessId,df.Keyword,Control,isRequired,Label,ControlNumber,isnull(CapturedValue,'')CapturedValue " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                        " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode " +
                    " where Nature='html' and sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationFieldModel>();

                sqlQuery = "update ServiceBusinessFields set capturedValue =@capturedValue, UpdatedTime=GETDATE()  where id=@ServiceBusinessId ";
                string capturedValue;
                foreach (var det in details)
                {
                    if (!type.Equals("HTML") && !det.ControlNumber.ToUpper().Equals("CLIENT"))
                        continue;

                    if (!postedData.TryGetValue(det.Keyword, out capturedValue))
                        capturedValue = "";
                    if (det.Keyword.StartsWith("C:") && capturedValue.Length > 0)
                        capturedValue = "true";
                    if (!capturedValue.Equals(det.CapturedValue))
                        db.Execute(sqlQuery, new { capturedValue, det.ServiceBusinessId });
                }


                //    sqlQuery = @" update ServiceBusinessOfficer set status = 'In-Progress' from " +
                //" (select ServiceBusinessId,OfficerId from ServiceBusinessOfficer  where Id=@OfficerStepId) a " +
                //" where ServiceBusinessOfficer.ServiceBusinessId = a.ServiceBusinessId and ServiceBusinessOfficer.OfficerId=a.OfficerId  ";

                sqlQuery = @" update ServiceBusinessOfficer set status = 'In-Progress',UpdatedDate=GetDate() where Id=@OfficerStepId ";

                db.Execute(sqlQuery, new
                {

                    officerStepId
                });

                sqlQuery = @" update ServiceBusiness set status = 'In-Progress' where Id=@ServiceBusinessId ";
                db.Execute(sqlQuery, new
                {
                    serviceBusinessId
                });

            }
            
            new BusinessOfficerMaster(Util).UpdateParserDetail(serviceBusinessId,
                officerStepId, "Executor");
            new BusinessProfileMaster(Util).UpdateParserDetail(serviceBusinessId);

            return "Posted";
        }

        private string SendDocument(string path, List<DocumentGenerationSignModel> emails)
        {
            DigiSignerClient client = new DigiSignerClient(DigiSignerKey);

            SignatureRequest request = new SignatureRequest();
            request.UseTextTags = true;
            request.HideTextTags = true;

            DigiSigner.Client.Document document = new DigiSigner.Client.Document(path + @"\Output\" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(emails[0].DestinationFile)));
            foreach (DocumentGenerationSignModel email in emails)
            {
                logger.Info(Util.ClientIP + "|" + "Document Sent to DigiSigner " + email.Email + "," + email.Executor);

                Signer signer = new Signer(email.Email);
                signer.Role = email.Executor.Replace("s:", "");
                if (email.Executor.Equals("s:Preparation"))
                    signer.Order = 2;
                else if (email.Executor.Equals("s:Approved"))
                    signer.Order = 3;
                else
                    signer.Order = 1;
                document.Signers.Add(signer);
            }
            request.Documents.Add(document);

            string signatureRequestId = " ";
            try
            {
                SignatureRequest response = client.SendSignatureRequest(request);

                logger.Info(Util.ClientIP + "|" + "Document Sent to DigiSigner and received Signature RequestId " + response.SignatureRequestId);

                signatureRequestId= response.SignatureRequestId;

            }
            catch (Exception e)
            {
                logger.Info(e.Message);
                
            }

            logger.Info(Util.ClientIP + "|" + "Document Sent to DigiSigner and received Signature RequestId " + signatureRequestId);

            return signatureRequestId;

        }

        public BusinessProfileModel ImportBusinessProfile(string filepath)
        {
            BusinessProfileModel businessProfile = new BusinessProfileModel();

            PdfReader reader = new PdfReader(filepath);
            PdfDocument document = new PdfDocument(reader);
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(document, false);
            IDictionary<String, PdfFormField> fields = acroForm.GetFormFields();

            PdfFormField toSet;
            fields.TryGetValue("Entity-Name", out toSet);
            businessProfile.Name = toSet.GetValueAsString();
            fields.TryGetValue("Entity-FormerName", out toSet);
            businessProfile.FormerName = toSet.GetValueAsString();
            fields.TryGetValue("Entity-TradingName", out toSet);
            businessProfile.TradingName = toSet.GetValueAsString();
            fields.TryGetValue("Entity-UEN", out toSet);
            businessProfile.UEN = toSet.GetValueAsString();
            fields.TryGetValue("Entity-IncorpDate", out toSet);
            businessProfile.IncorpDate = toSet.GetValueAsString();
            fields.TryGetValue("TextArea", out toSet);
            businessProfile.Address1 = toSet.GetValueAsString();
            fields.TryGetValue("Entity-City", out toSet);
            businessProfile.City = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Country", out toSet);
            businessProfile.Country = toSet.GetValueAsString();
            fields.TryGetValue("Entity-PostalCode", out toSet);
            businessProfile.Pincode = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Phone", out toSet);
            businessProfile.Phone = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Mobile", out toSet);
            businessProfile.Mobile = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Email", out toSet);
            businessProfile.Email = toSet.GetValueAsString();

            document.Close();
            reader.Close();

            logger.Info(Util.ClientIP + "|" + "Client Profile Imported ");
            logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessProfile));
            

            return businessProfile;
        }

        public EntityShareholderModel ImportEntityProfile(string filepath)
        {
            EntityShareholderModel entityShareholder = new EntityShareholderModel();

            PdfReader reader = new PdfReader(filepath);
            PdfDocument document = new PdfDocument(reader);
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(document, false);
            IDictionary<String, PdfFormField> fields = acroForm.GetFormFields();

            PdfFormField toSet;
            fields.TryGetValue("Entity-Name", out toSet);
            entityShareholder.Name = toSet.GetValueAsString();
            fields.TryGetValue("Entity-FormerName", out toSet);
            entityShareholder.FormerName = toSet.GetValueAsString();
            fields.TryGetValue("Entity-TradingName", out toSet);
            entityShareholder.TradingName = toSet.GetValueAsString();
            fields.TryGetValue("Entity-UEN", out toSet);
            entityShareholder.UEN = toSet.GetValueAsString();
            fields.TryGetValue("Entity-IncorpDate", out toSet);
            entityShareholder.IncorpDate = toSet.GetValueAsString();
            fields.TryGetValue("TextArea", out toSet);
            entityShareholder.Address = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Country", out toSet);
            entityShareholder.Country = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Phone", out toSet);
            entityShareholder.Phone = toSet.GetValueAsString();
            fields.TryGetValue("Entity-Email", out toSet);
            entityShareholder.Email = toSet.GetValueAsString();

            document.Close();
            reader.Close();

            logger.Info(Util.ClientIP + "|" + "EntityShareholder Imported ");
            logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(entityShareholder));


            return entityShareholder;
        }
        private string GetFormValue(IDictionary<String, PdfFormField> fields,string key)
        {
            PdfFormField toSet;
            fields.TryGetValue(key, out toSet);
            if (toSet != null)
            {
                return toSet.GetValueAsString();
            }
            else
                return "";
        }
        public BusinessOfficerModel ImportBusinessOfficer(string filepath)
        {
            BusinessOfficerModel businessOfficer = new BusinessOfficerModel();

            PdfReader reader = new PdfReader(filepath);
            PdfDocument document = new PdfDocument(reader);
            PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(document, false);
            IDictionary<String, PdfFormField> fields = acroForm.GetFormFields();

            PdfFormField toSet;
            fields.TryGetValue("Officer-Name", out toSet);
            businessOfficer.Name = toSet.GetValueAsString();
            businessOfficer.Address = GetFormValue(fields, "TextArea");
            fields.TryGetValue("Officer-nationality", out toSet);
            businessOfficer.Nationality = toSet.GetValueAsString();
            fields.TryGetValue("Officer-birthDate", out toSet);
            businessOfficer.BirthDate = toSet.GetValueAsString();
            fields.TryGetValue("Officer-passportNo", out toSet);
            businessOfficer.PassportNo = toSet.GetValueAsString();
            fields.TryGetValue("Officer-passportIssueDate", out toSet);
            businessOfficer.PassportIssueDate = toSet.GetValueAsString();
            fields.TryGetValue("Officer-passportExpiryDate", out toSet);
            businessOfficer.PassportExpiryDate = toSet.GetValueAsString();
            fields.TryGetValue("Officer-passportIssuePlace", out toSet);
            businessOfficer.PassportIssuePlace = toSet.GetValueAsString();
            fields.TryGetValue("Officer-finNo", out toSet);
            businessOfficer.NricNo = toSet.GetValueAsString();
            fields.TryGetValue("Officer-finIssueDate", out toSet);
            businessOfficer.NricIssueDate = toSet.GetValueAsString();
            fields.TryGetValue("Officer-finExpiryDate", out toSet);
            businessOfficer.FinExpiryDate = toSet.GetValueAsString();
            fields.TryGetValue("Officer-position", out toSet);
            businessOfficer.UserRole = toSet.GetValueAsString();
            fields.TryGetValue("Officer-Mobile", out toSet);
            businessOfficer.Mobile = toSet.GetValueAsString();
            fields.TryGetValue("Officer-Email", out toSet);
            businessOfficer.Email = toSet.GetValueAsString();

            document.Close();
            reader.Close();

            logger.Info(Util.ClientIP + "|" + "Officer Imported ");
            logger.Info(Util.ClientIP + "|" + JsonConvert.SerializeObject(businessOfficer));
            

            return businessOfficer;
        }
    }


}
