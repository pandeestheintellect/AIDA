using Newtonsoft.Json;
using System.Collections.Generic;

namespace DigiSigner.Client
{
    public class DocumentFields
   {
        [JsonProperty("document_fields")]
        public List<DocumentField> Fileds
        {
            get; set;
        }
    }
}
