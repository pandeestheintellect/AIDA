using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;

using System.Diagnostics;
using System;

using DigiSigner.Client;     
using System.Text.RegularExpressions;
using System.IO; 
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.IO.Font;
using System.Globalization;
using NLog;
using TemplateEngine.Docx;
using OpenXmlPowerTools;

namespace RoboDocLib.Services
{
    public class DocumentManager
    {

        string connectionString = "";
        int UserId = 999;
        string DigiSignerKey = "77893e42-1090-4823-a518-9337572d2d6d";

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public DocumentManager(ControllerUtil util)
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

        private string MakeTemplate(string path, string prefix, DocumentGenerationModel documentSummary, List<DocumentGenerationFieldModel> documentFields)
        {
            string desitinationFile = "";

            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            desitinationFile = prefix + myRandomNo + "-" + documentSummary.DestinationFile;

            File.Copy(path + @"\Source-Template\" + documentSummary.FilePath + @"\" +  documentSummary.SourceFile,
                path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile + ".docx");

            Content valuesToFill = new Content();
            foreach (DocumentGenerationFieldModel fields in documentFields)
            {
                if (fields.Control.Trim().Length == 0)
                {
                    valuesToFill.Fields.Add(new FieldContent(fields.Keyword, fields.CapturedValue));
                }
                else if (fields.Control.Equals("checkbox"))
                {
                    if (fields.CapturedValue.Equals("true"))
                        valuesToFill.Images.Add(new ImageContent(fields.Keyword, File.ReadAllBytes(path + @"\images\check.png")));
                    else
                        valuesToFill.Images.Add(new ImageContent(fields.Keyword, File.ReadAllBytes(path + @"\images\uncheck.png")));
                }
                else if (fields.Control.Equals("currency"))
                {
                    valuesToFill.Fields.Add(new FieldContent(fields.Keyword, GetCurrencyFormat(fields.CapturedValue)));
                }
                else if (fields.Control.Equals("text"))
                {
                    valuesToFill.Fields.Add(new FieldContent(fields.Keyword, fields.CapturedValue));
                }
                else if (fields.Control.Equals("date"))
                {
                    if (fields.Keyword.StartsWith("DM:"))
                        valuesToFill.Fields.Add(new FieldContent(fields.Keyword, fields.CapturedValue.Substring(0,5)));
                    else
                        valuesToFill.Fields.Add(new FieldContent(fields.Keyword, fields.CapturedValue));
                }
            }

            Dictionary<string, List<string[]>> tables = new Dictionary<string, List<string[]>>();
            foreach (DocumentGenerationFieldModel fields in documentFields.Where(f=>f.Container.StartsWith("Table")))
            {
                string[] tablecols = fields.Container.Split('-');
                string[] cellrow = fields.CapturedValue.Split('\n') ;
                if (tables.TryGetValue(tablecols[0], out List<string[]> rows))
                {
                    rows[int.Parse(tablecols[1].Replace("Col", "")) - 1] = cellrow;
                }
                else
                {
                    List<string[]> row = new List<string[]>();
                    for (int i=1;i<=int.Parse(tablecols[2]);i++)
                    {
                        row.Add(new string[1]);
                    }
                    row[int.Parse(tablecols[1].Replace("Col",""))-1] = cellrow;
                    tables[tablecols[0]] = row;
                }
            }

            foreach(string tableName in tables.Keys)
            {
                TableContent tableContent = new TableContent(tableName);
                if (tables.TryGetValue(tableName, out List<string[]> table))
                {
                    int rows = table[0].Length;
                    int cols = table.Count;
                    
                    for (int i=0;i< rows; i++)
                    {
                        FieldContent[] fields = new FieldContent[cols];
                        for (int j = 0; j < cols; j++)
                        {
                            try
                            {
                                fields[j]= new FieldContent("Col" + (j + 1), table[j][i]);
                            }
                            catch
                            {
                                fields[j] = new FieldContent("Col" + (j + 1), "");
                            }
                        }
                        tableContent.AddRow(fields);
                    }
                }
                valuesToFill.Tables.Add(tableContent);

            }


            using (var outputDocument = new TemplateProcessor(path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile + ".docx")
                .SetRemoveContentControls(true))
            {
                string footerTest = outputDocument.GetFooterNote();
                if (footerTest.ToUpper().Contains("PAGE"))
                {
                    int iPage = footerTest.ToUpper().IndexOf("PAGE");
                    if (iPage == 0)
                        footerTest = "";
                    else
                        footerTest = footerTest.Substring(0, iPage - 1);


                }
                footerTest = footerTest.Replace("&", "and");
                outputDocument.FillContent(valuesToFill);
                
                outputDocument.SaveChanges();

                ConvertToHtml(path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile + ".docx", path + @"\Output\" + documentSummary.FilePath, footerTest);

                ConverterProperties properties = new ConverterProperties();
                properties.SetBaseUri(path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile);
                properties.SetFontProvider(new DefaultFontProvider(true, true, true));

                PdfWriter writer = new PdfWriter(path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile + ".pdf",
                new WriterProperties().SetFullCompressionMode(true));

                FileStream htmlfile = new FileStream(path + @"\Output\" + documentSummary.FilePath + @"\" + desitinationFile + ".html", FileMode.Open);
                iText.Html2pdf.HtmlConverter.ConvertToPdf(htmlfile, writer, properties);
                htmlfile.Close();
                writer.Close();
            }

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(documentSummary.FilePath + @"\" + desitinationFile)) ;
        }
        public static void ConvertToHtml(string file, string outputDirectory,string footerTest)
        {
            var fi = new FileInfo(file);
            Console.WriteLine(fi.Name);
            byte[] byteArray = File.ReadAllBytes(fi.FullName);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
                {
                    var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                    if (outputDirectory != null && outputDirectory != string.Empty)
                    {
                        DirectoryInfo di = new DirectoryInfo(outputDirectory);
                        if (!di.Exists)
                        {
                            throw new OpenXmlPowerToolsException("Output directory does not exist");
                        }
                        destFileName = new FileInfo(Path.Combine(di.FullName, destFileName.Name));
                    }
                    var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                    int imageCounter = 0;

                    var pageTitle = fi.FullName;
                    //var part = wDoc.CoreFilePropertiesPart;
                    //if (part != null)
                    //{
                    //    pageTitle = (string)part.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fi.FullName;
                    //}

                    // TODO: Determine max-width from size of content area.
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        //AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                        AdditionalCss = " @page { " +
                            "	size: A4; " +
                            " 	margin:1.5cm; " +
                            " 	@bottom-center { " +
                            " 	 font-family:Calibri; " +
                            " 	 font-size : 12px; " +
                            " 	 content: '"+footerTest+" Page ' counter(page) ' of ' counter(pages); " +
                            " 	} " +
                            " } " +
                            " body { font-family:Calibri;font-size:16px;} ",

                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        
                        ImageHandler = imageInfo =>
                        {
                            DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryName);
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            if (extension == "png")
                                imageFormat = ImageFormat.Png;
                            else if (extension == "gif")
                                imageFormat = ImageFormat.Gif;
                            else if (extension == "bmp")
                                imageFormat = ImageFormat.Bmp;
                            else if (extension == "jpeg")
                                imageFormat = ImageFormat.Jpeg;
                            else if (extension == "tiff")
                            {
                                // Convert tiff to gif.
                                extension = "gif";
                                imageFormat = ImageFormat.Gif;
                            }
                            else if (extension == "x-wmf")
                            {
                                extension = "wmf";
                                imageFormat = ImageFormat.Wmf;
                            }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                return null;

                            string imageFileName = imageDirectoryName + "/image" +
                                imageCounter.ToString() + "." + extension;
                            try
                            {
                                imageInfo.Bitmap.Save(imageFileName, imageFormat);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            string imageSource = localDirInfo.Name + "/image" +
                                imageCounter.ToString() + "." + extension;

                            XElement img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageSource),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    XElement htmlElement = OpenXmlPowerTools.HtmlConverter.ConvertToHtml(wDoc, settings);

                    // Produce HTML document with <!DOCTYPE html > declaration to tell the browser
                    // we are using HTML5.
                    var html = new XDocument(
                        new XDocumentType("html", null, null, null),
                        htmlElement);

                    // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                    // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                    // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                    // for detailed explanation.
                    //
                    // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                    // must do it correctly, or entities will not be serialized properly.

                    //Erana
                    string htmlString = html.ToString(SaveOptions.DisableFormatting);
                    htmlString = htmlString.Replace("<span xml:space=", "&nbsp;<span xml:space=");
                    htmlString = htmlString.Replace("<br />[{PAGEBREAK}]", "<div style='page-break-before:always'>&nbsp;</div>");

                    File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
                }
            }
        }

        public string PreviewDocument(string path, int serviceBusinessId, int officerStepId, string type)
        {
            List<DocumentGenerationModel> summary;
            List<DocumentGenerationFieldModel> details;
            string OutFileName = "";
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string sqlQuery = "select distinct sbo.ServiceBusinessId,sbo.Id OfficerStepId,FileName SourceFile,':'+sbo.Executor+ convert(varchar,sbo.OfficerId) Executor, " +
                        " convert(varchar, sbo.ServiceBusinessId) + '-' + convert(varchar, sbo.Id) + '-' + doc.Code DestinationFile, isnull(FilePath,'') FilePath " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " where sbo.ServiceBusinessId=@ServiceBusinessId and sbo.Id =@officerStepId";
                summary = db.Query<DocumentGenerationModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationModel>();

              
                sqlQuery = @" select sbf.ServiceBusinessId,OfficerStepId,df.Keyword,isnull(CapturedValue,' ') CapturedValue,Control,isnull(Container,' ') Container " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                        " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode "+
                    " where Control<>'label' and sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId = @OfficerStepId";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceBusinessId,
                        officerStepId
                    }).AsList<DocumentGenerationFieldModel>();

                foreach (var document in summary)
                {
                    if (type.Equals("PDF"))
                        OutFileName = MakePDF(path, "Prev", document,
                            details.FindAll(x =>
                            (x.ServiceBusinessId == document.ServiceBusinessId && x.OfficerStepId == document.OfficerStepId))
                            .AsList<DocumentGenerationFieldModel>());
                    else
                        OutFileName = MakeTemplate(path, "Prev", document,
                        details.FindAll(x =>
                        (x.ServiceBusinessId == document.ServiceBusinessId && x.OfficerStepId == document.OfficerStepId))
                        .AsList<DocumentGenerationFieldModel>());

                }

            }
            return OutFileName;
        }
        public ResponseModel DispatchDocument(string path, ServicesSignSave serviceSign)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };

            List<DocumentGenerationModel> summary;
            List<DocumentGenerationFieldModel> details;
            string fileName = "";
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select distinct sbo.ServiceBusinessId,sbo.Id OfficerStepId,FileName SourceFile,':'+sbo.Executor+ convert(varchar,sbo.OfficerId) Executor, " +
                        " convert(varchar, sbo.ServiceBusinessId) + '-' + convert(varchar, sbo.Id) + '-' + doc.Code DestinationFile, isnull(FilePath,'') FilePath  " +
                        " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                        " where sbo.ServiceBusinessId=@ServiceBusinessId and sbo.Id in @officers ";
                
                List<int> officers = new List<int>(Array.ConvertAll(serviceSign.OfficerStepIds.Split(','), int.Parse));
                summary = db.Query<DocumentGenerationModel>(sqlQuery,
                    new {
                        serviceSign.ServiceBusinessId,
                        officers
                    }).AsList<DocumentGenerationModel>();

                sqlQuery = @" select sbf.ServiceBusinessId,OfficerStepId,df.Keyword,isnull(CapturedValue,' ') CapturedValue,Control,isnull(Container,' ') Container " +
                    " from DocumentMaster doc  join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
                    " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
                    " join DocumentFields df on df.Keyword = sbf.Keyword and df.Code=sbo.DocumentCode " +
                " where Control<>'label' and sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId in @officers";

                details = db.Query<DocumentGenerationFieldModel>(sqlQuery,
                    new
                    {
                        serviceSign.ServiceBusinessId,
                        officers
                    }).AsList<DocumentGenerationFieldModel>();

                foreach (var document in summary)
                {
                    fileName = MakeTemplate(path, "Sign", document,
                        details.FindAll(x =>
                        (x.ServiceBusinessId == document.ServiceBusinessId && x.OfficerStepId == document.OfficerStepId))
                        .AsList<DocumentGenerationFieldModel>());

                    sqlQuery = @"update ServiceBusinessOfficer set GeneratedFileName=@fileName,UpdatedDate=Getdate() where Id=@OfficerStepId";
                    db.Execute(sqlQuery, new { fileName,document.OfficerStepId});

                    sqlQuery = @" select GeneratedFileName DestinationFile,CapturedValue Executor,CapturedBy Email from ServiceBusinessOfficer sbo " +
                " join ServiceBusinessFields sbf on sbo.Id =sbf.OfficerStepId and sbo.ServiceBusinessId=sbf.ServiceBusinessId " +
                " where sbf.ServiceBusinessId=@ServiceBusinessId and sbf.OfficerStepId = @OfficerStepId and Keyword like 's:%' ";

                    List<DocumentGenerationSignModel> emails = db.Query<DocumentGenerationSignModel>(sqlQuery,
                        new
                        {
                            serviceSign.ServiceBusinessId,
                            document.OfficerStepId
                        }).AsList<DocumentGenerationSignModel>();

                    fileName =  SendDocument(path, emails);

                    sqlQuery = @"update ServiceBusinessOfficer set digiSignCode=@fileName,UpdatedDate=Getdate() where Id=@OfficerStepId";
                    db.Execute(sqlQuery, new { fileName, document.OfficerStepId });

                }

                response.IsSuccess = true;
                response.Message = "Services modified";
                
            }
            return response;
        }

        private string MakePDF(string path, string prefix, DocumentGenerationModel documentSummary, List<DocumentGenerationFieldModel> documentFields)
        {
            string desitinationFile = "";
            //using (var document = DocX.Load(path + @"\Source\" + documentSummary.SourceFile))
            //{
            //    var image = document.AddImage(path + @"\Source\check.png");
            //    var checkPic = image.CreatePicture(14, 14);

            //    image = document.AddImage(path + @"\Source\uncheck.png");
            //    var uncheckpic = image.CreatePicture(14, 14);

            //    foreach (DocumentGenerationFieldModel fields in documentFields)
            //    {
            //        if (fields.Keyword.StartsWith("C:"))
            //        {
            //            if (fields.CapturedValue.Equals("true"))
            //                document.ReplaceTextWithObject("[{" + fields.Keyword + "}]", checkPic, false, RegexOptions.IgnoreCase);
            //            else
            //                document.ReplaceTextWithObject("[{" + fields.Keyword + "}]", uncheckpic, false, RegexOptions.IgnoreCase);
            //        }
            //        else
            //            document.ReplaceText("[{" + fields.Keyword + "}]", fields.CapturedValue);
            //    }
                
            //    document.ReplaceText(":Executor", documentSummary.Executor);

            //    Random rnd = new Random();
            //    int myRandomNo = rnd.Next(10000000, 99999999);
            //    desitinationFile = prefix + myRandomNo + "-" + documentSummary.DestinationFile;

            //    document.SaveAs(path + @"\Output\" + desitinationFile + ".docx");

            //    //var doc = new GcWordDocument();
            //    //doc.Load(path + @"\Output\" + desitinationFile + ".docx");
            //    //doc.SaveAsPdf(path + @"\Output\" + desitinationFile + ".pdf");

            //}
            return desitinationFile;
        }

        private string SendDocument(string path, List<DocumentGenerationSignModel> emails)
        {
            DigiSignerClient client = new DigiSignerClient(DigiSignerKey);

            SignatureRequest request = new SignatureRequest();
            request.UseTextTags = true;
            request.HideTextTags = true;

            DigiSigner.Client.Document document = new DigiSigner.Client.Document(path + @"\Output\" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(emails[0].DestinationFile))+ ".pdf");
            foreach (DocumentGenerationSignModel email in emails)
            {
                Signer signer = new Signer(email.Email);
                signer.Role = email.Executor.Replace("s:","");
                if (email.Executor.Equals("s:Preparation"))
                    signer.Order = 2;
                else if (email.Executor.Equals("s:Approved"))
                    signer.Order = 3;
                else
                    signer.Order = 1;
                document.Signers.Add(signer);
            }
            request.Documents.Add(document);

            SignatureRequest response = client.SendSignatureRequest(request);

            return response.SignatureRequestId;

        }

        public void ProcessSignerCallback(string path, SignerCallback document)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "select GeneratedFileName from ServiceBusinessOfficer where DigiSignCode=@SignatureRequestId ";
                string fileName = db.QuerySingle<string>(sqlQuery, new { document.signature_request.SignatureRequestId });

                if (document.signature_request.Documents.Count > 0)
                {
                    DigiSignerClient client = new DigiSignerClient(DigiSignerKey);
                    fileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(fileName));
                    fileName = fileName.Replace(".pdf", "");
                    client.GetDocumentById(document.signature_request.Documents[0].ID, path + @"\Output\" + fileName + "-completed.pdf");
                    fileName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fileName + "-completed.pdf"));

                    sqlQuery = "update ServiceBusinessOfficer set status='Completed',DownloadedFileName =@fileName,UpdatedDate=Getdate()  where DigiSignCode=@SignatureRequestId ";
                    db.Execute(sqlQuery, new { document.signature_request.SignatureRequestId, fileName });

                    sqlQuery = "select ServiceBusinessId from ServiceBusinessOfficer where DigiSignCode=@SignatureRequestId ";
                    int ServiceBusinessId = db.QuerySingle<int>(sqlQuery, new { document.signature_request.SignatureRequestId });


                    sqlQuery = "select status " +
                                " from ServiceBusinessOfficer where ServiceBusinessId = @ServiceBusinessId " +
                                " group by ServiceBusinessId,status having count(distinct status) = 1";
                    string serviceStatus = db.QuerySingle<string>(sqlQuery, new { ServiceBusinessId });

                    if (serviceStatus.Equals("Completed"))
                    {
                        sqlQuery = "update ServiceBusiness set status='Completed',UpdatedDate=Getdate() where ID=@ServiceBusinessId ";
                        db.Execute(sqlQuery, new { ServiceBusinessId });

                    }
                }
            }
        }

        //public string GenerateDoc(string path, int serviceBusinessId, int officerStepId)
        //{
        //    using (IDbConnection db = new SqlConnection(connectionString))
        //    {
        //        string sqlQuery = "select FileName,Keyword,isnull(CapturedValue,' ') CapturedValue  from DocumentMaster doc  " +
        //                        " join ServiceBusinessOfficer sbo on doc.Code = sbo.DocumentCode " +
        //                        " join ServiceBusinessFields sbf on sbo.id = sbf.officerStepId " +
        //                " where sbf.ServiceBusinessId=@ServiceBusinessId and OfficerStepId=@OfficerStepId ";
        //        List<DocumentGenerationModel> docGenFields = db.Query<DocumentGenerationModel>(sqlQuery, new { serviceBusinessId, officerStepId }).AsList<DocumentGenerationModel>();

        //        return ParseDoc(path, docGenFields);
        //    }
        //}

        //private string ParseDoc(string path, string pdfTool, List<DocumentGenerationModel> docGenFields)
        //{
        //    string formatedFile;
        //    string fileName;
        //    using (var document = DocX.Load(path + @"\Source\" + docGenFields[0].SourceFile))
        //    {
        //        foreach (DocumentGenerationModel fields in docGenFields)
        //        {
        //            document.ReplaceText("[{" + fields.Keyword + "}]", fields.CapturedValue);
        //        }

        //        Random rnd = new Random();
        //        int myRandomNo = rnd.Next(10000000, 99999999);
        //        formatedFile = "f" + myRandomNo + docGenFields[0].SourceFile.Replace(".docx", "");
        //        fileName = path + @"\Output\" + formatedFile + ".docx";

        //        document.SaveAs(fileName);

        //        var doc = new GcWordDocument();

        //        doc.Load(fileName);
        //        fileName = path + @"\Output\" + formatedFile + ".pdf";
        //        doc.SaveAsPdf(fileName);

        //        //fileName = path + @"\Output\" + formatedFile + ".pdf";
        //        //DocX.ConvertToPdf(document, fileName);


        //        //string fileDir = path;

        //        //var pdfProcess = new Process();
        //        //pdfProcess.StartInfo.FileName = pdfTool;
        //        //pdfProcess.StartInfo.Arguments =
        //        //    string.Format("--norestore --nofirststartwizard --headless --convert-to pdf  \"{0}\""
        //        //                          , fileName);
        //        //pdfProcess.StartInfo.WorkingDirectory = fileDir;
        //        //pdfProcess.StartInfo.RedirectStandardOutput = true;
        //        //pdfProcess.StartInfo.RedirectStandardError = true;
        //        //pdfProcess.StartInfo.UseShellExecute = false;
        //        //pdfProcess.Start();

        //        //string output = pdfProcess.StandardOutput.ReadToEnd();
        //        //string error = pdfProcess.StandardError.ReadToEnd();


        //        //ProcessStartInfo procStartInfo = new ProcessStartInfo(pdfTool, string.Format("--convert-to pdf --nologo {0}", fileName));
        //        //procStartInfo.RedirectStandardOutput = true;
        //        //procStartInfo.UseShellExecute = false;
        //        //procStartInfo.CreateNoWindow = true;
        //        //procStartInfo.WorkingDirectory = fileDir;

        //        //Process process = new Process() { StartInfo = procStartInfo, };
        //        //process.Start();
        //        //process.WaitForExit();




        //    }

        //    //string fileDir = path;

        //    //           var pdfProcess = new Process();
        //    //           pdfProcess.StartInfo.FileName = pdfTool;
        //    //           pdfProcess.StartInfo.Arguments =
        //    //               string.Format("--norestore --nofirststartwizard --headless --convert-to pdf  \"{0}\""
        //    //                                     , fileName);
        //    //           pdfProcess.StartInfo.WorkingDirectory = fileDir;
        //    //           pdfProcess.StartInfo.RedirectStandardOutput = true;
        //    //           pdfProcess.StartInfo.RedirectStandardError = true;
        //    //           pdfProcess.StartInfo.UseShellExecute = false;
        //    //           pdfProcess.Start();

        //    //           string output = pdfProcess.StandardOutput.ReadToEnd();
        //    //           string error = pdfProcess.StandardError.ReadToEnd();

        //    return formatedFile;

        //}
    }

    
}


