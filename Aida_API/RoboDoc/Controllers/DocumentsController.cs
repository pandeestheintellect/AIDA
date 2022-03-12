using RoboDocCore.Models;
using RoboDocLib.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RoboDoc.Controllers
{
    public class DocumentsController : APIController
    {
        [HttpGet]
        [Route("api/documents")]
        public List<DocumentModel> GetDocuments()
        {
            return new DocumentMaster(Util).GetDocument();
        }

        [Route("api/documents")]
        [HttpPost]
        public ResponseModel PostDocument(DocumentModel document)
        {
            return new DocumentMaster(Util).PostDocument(document);
        }

        [Route("api/documents")]
        [HttpPut]
        public ResponseModel PutDocument(DocumentModel document)
        {
            return new DocumentMaster(Util).PutDocument(document);
        }

        [Route("api/documents/{code}")]
        [HttpDelete]
        public ResponseModel DeleteDocument(string code)
        {
            return new DocumentMaster(Util).DeleteDocument(code);
        }

        [Route("api/documents/{documentCode}")]
        [HttpGet]
        public List<DocumentFieldModel> GetDocumentFields(string documentCode)
        {
            return new DocumentMaster(Util).GetDocumentFields(documentCode);
        }

    }
}
