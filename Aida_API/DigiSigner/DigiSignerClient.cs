using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DigiSigner.Client
{
    public class DigiSignerClient
    {
        private readonly string apiKey;
        private readonly string serverUrl;

        public DigiSignerClient(string apiKey)
        {
            this.apiKey = apiKey;
            serverUrl = Config.DEFAULT_SERVER_URL;
        }

        private void AddAuthInfo(WebHeaderCollection headers)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey + ":"));
            headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
        }

        /// <summary>
        /// Upload document and returns ID of document.
        /// </summary>
        /// <param name="document">document to upload.</param>
        /// <returns>ID of uploaded document.</returns>
        public string UploadDocument(string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                AddAuthInfo(webClient.Headers);

                byte[] result = webClient.UploadFile(Config.getDocumentUrl(serverUrl), filename);

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    Encoding.ASCII.GetString(result)
                )[Config.PARAM_DOC_ID];
            }
        }

        /// <summary>
        /// Deletes document by document ID.
        /// </summary>
        /// <param name="documentId">ID of deleteded document.</param>
        public void DeleteDocument(string documentId)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getDeleteDocumentUrl(serverUrl, documentId));

            AddAuthInfo(webRequest.Headers);

            webRequest.Method = "DELETE";
            webRequest.GetResponse();
        }

        /// <summary>
        /// Download the document by ID.
        /// </summary>
        /// <param name="documentId">ID of document.</param>
        /// <param name="filename">the name of the document file to be saved.</param>
        public void GetDocumentById(string documentId, string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                AddAuthInfo(webClient.Headers);

                webClient.DownloadFile(Config.getDocumentUrl(serverUrl) + "/" + documentId, filename);
            }
        }

        /// <summary>
        /// Adds content to the document after given document ID.
        /// </summary>
        /// <param name="documentId">documentId to insert content.</param>
        /// <param name="signatures">signatures will be rendered on the document.</param>
        public void AddContentToDocument(string documentId, List<Signature> signatures)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getContentUrl(serverUrl, documentId));

            AddAuthInfo(webRequest.Headers);

            webRequest.Method = "POST";
           
            WrireBodyRequest(
                webRequest,
                JsonConvert.SerializeObject(new DocumentContent(signatures), Formatting.Indented)
            );

            WebResponse response = webRequest.GetResponse();
        }

        /// <summary>
        /// Returns document fields for a document.
        /// </summary>
        /// <param name="documentId">Id of the document.</param>
        /// <returns>Document's fields</returns>
        public DocumentFields GetDocumentFields(string documentId)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getFieldsUrl(serverUrl, documentId));

            AddAuthInfo(webRequest.Headers);

            webRequest.Method = "GET";

            return ReadFieldsFromBody<DocumentFields>(
                webRequest.GetResponse()
            );
        }

        /// <summary>
        /// The get signature request to check information about signature such as signature is completed
        /// and IDs of signature request and documents.
        /// </summary>
        /// <param name="signatureRequestId">ID of the signature request.</param>
        /// <returns>SignatureRequest with filled IDs and signature request data.</returns>
        public SignatureRequest GetSignatureRequest(string signatureRequestId)
        {
            String url = Config.getSignatureRequestsUrl(serverUrl) + "/" + signatureRequestId;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            AddAuthInfo(webRequest.Headers);

            webRequest.Method = "GET";

            return ReadFieldsFromBody<SignatureRequest>(
                webRequest.GetResponse()
            );
        }

        /// <summary>
        /// Sends the signature request to the server.
        /// </summary>
        /// <param name="signatureRequest">signatureRequest filled signature request with required data.</param>
        /// <returns>result with sent signature request ID.</returns>
        public SignatureRequest SendSignatureRequest(SignatureRequest signatureRequest)
        {
            foreach (Document document in signatureRequest.Documents)
            {
                if (document.ID == null)
                {
                    document.ID = UploadDocument(document.FileName);
                }
            }

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Config.getSignatureRequestsUrl(serverUrl));

            AddAuthInfo(webRequest.Headers);

            webRequest.Method = "POST";

            WrireBodyRequest(
                webRequest,
                JsonConvert.SerializeObject(signatureRequest, Formatting.Indented)
            );

            return ReadFieldsFromBody<SignatureRequest>(
                webRequest.GetResponse()
            );
        }

        private void WrireBodyRequest(HttpWebRequest request, string json)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(json);

            request.ContentLength = buffer.Length;
            request.ContentType = "application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(buffer, 0, buffer.Length);
                dataStream.Flush();

                dataStream.Close();
            }
        }

        private T ReadFieldsFromBody<T>(WebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                byte[] buffer = new byte[response.ContentLength];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                string json = Encoding.ASCII.GetString(buffer);

                return JsonConvert.DeserializeObject<T>(json);
            }
        }
        
        /// <summary>
        /// Download the document attachment by ID of document and API ID of field.
        /// </summary>
        /// <param name="documentId">ID of document.</param>
        /// <param name="fieldApiId">ID of document.</param>
        /// <param name="filename">the name of the file to be saved.</param>
        public void GetDocumentAttachment(string documentId, string fieldApiId, string filename)
        {
            using (WebClient webClient = new WebClient())
            {
                AddAuthInfo(webClient.Headers);

                webClient.DownloadFile(Config.getDocumentAttachmentUrl(serverUrl, documentId, fieldApiId), filename);
            }
        }
    }
}
