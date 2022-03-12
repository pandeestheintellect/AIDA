namespace DigiSigner.Client
{
    public class Config
    {
        public const string DEFAULT_SERVER_URL = "https://digisigner.com/online/api";

        private const string VERSION = "/v1";
        private const string DOCUMENTS_URL = "/documents";
        private const string SIGNATURE_REQUESTS_URL = "/signature_requests";
        private const string FIELDS_URL = "/fields";
        private const string CONTENT_URL = "/content";
        private const string ATTACHMENT_URL = "/attachment";
        private const string SLASH = "/";

        public const string PARAM_DOC_ID = "document_id";

        private Config()
        {
        }

        public static string getDocumentUrl(string server)
        {
          return server + VERSION + DOCUMENTS_URL;
        }

        public static string getFieldsUrl(string server, string documentId)
        {
          return getDocumentUrl(server) + SLASH + documentId + FIELDS_URL;
        }

        public static string getContentUrl(string server, string documentId)
        {
          return getDocumentUrl(server) + SLASH + documentId + CONTENT_URL;
        }

        public static string getSignatureRequestsUrl(string server)
        {
          return server + VERSION + SIGNATURE_REQUESTS_URL;
        }

        public static string getDeleteDocumentUrl(string server, string documentId)
        {
          return getDocumentUrl(server) + SLASH + documentId;
        }
      
        public static string getDocumentAttachmentUrl(string server, string documentId, string fieldApiId)
        {
          return getDocumentUrl(server) + SLASH + documentId + FIELDS_URL + SLASH + fieldApiId + ATTACHMENT_URL;
        }
    }
}
