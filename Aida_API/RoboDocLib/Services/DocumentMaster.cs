using RoboDocCore.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using NLog;

namespace RoboDocLib.Services
{
    public class DocumentMaster
    {
        string connectionString = "";
        int UserId = 999;

        public ControllerUtil Util { get; private set; }
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public DocumentMaster(ControllerUtil util)
        {
            Util = util;

            connectionString = util.ConnectionString;

        }

        public ResponseModel PostDocument(DocumentModel document)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"insert into DocumentMaster(Code,Name,FilePath,FileName,Remarks,EffectiveDate,VersionNo,Status, UserId,CreatedDate,UpdatedDate)" +
                                        "VALUES (@Code,@Name,@Remarks,999,getdate(),getdate())";
                var result = db.Execute(sqlQuery, document);
                response.IsSuccess = true;
                response.Message = "Document added";
            }
            return response;
        }

        public ResponseModel PutDocument(DocumentModel document)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"update DocumentMaster set Name=@Name,FilePath=@FilePath, FileName=@FileName, EffectiveDate=@EffectiveDate,VersionNo=@VersionNo,Status=@Status,UserId=@UserId,UpdatedDate=getdate()" +
                                    " where Code=@Code";

                var result = db.Execute(sqlQuery, new
                {
                    document.Code,
                    document.Name,
                    document.FilePath,
                    document.FileName,
                    document.EffectiveDate,
                    document.VersionNo,
                    document.Status,
                    UserId
                });
                response.IsSuccess = true;
                response.Message = "Document modified";
            }
            return response;
        }

        public ResponseModel DeleteDocument(string code)
        {
            ResponseModel response = new ResponseModel() { IsSuccess = false, Message = "Unknow Error" };
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @"Delete DocumentMaster where Code=@Code";
                var result = db.Execute(sqlQuery, new { code });
                response.IsSuccess = true;
                response.Message = "Document deleted";
            }
            return response;
        }

        public List<DocumentModel> GetDocument()
        {
            List<DocumentModel> response = new List<DocumentModel>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = @" select dm.Code,dm.Name,FilePath,FileName,convert(varchar,EffectiveDate,105) EffectiveDate, " +
                            " VersionNo,Status,sd.Name ServiceName from DocumentMaster dm  " +
                            " join ServiceDocument sm on dm.Code=sm.DocumentCode " +
                            " join ServicesDefinition sd on sm.ServiceCode = sd.Code " +
                            " order by dm.Name ";
                response = db.Query<DocumentModel>(sqlQuery).AsList<DocumentModel>();
            }
            return response;
        }

        public List<DocumentFieldModel> GetDocumentFields(string documentCode)
        {
            string sql = "select Code,Keyword,Label,Control,Nature,IsRequired from DocumentFields where Code=@Code order by Keyword";
            List<DocumentFieldModel> result;
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<DocumentFieldModel>(sql, new { Code = documentCode },
                    commandType: System.Data.CommandType.Text).AsList<DocumentFieldModel>();
            }
            return result;
        }

    }
}
