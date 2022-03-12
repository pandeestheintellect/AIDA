using Newtonsoft.Json;
using System;

namespace DigiSigner.Client
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Branding
    {
        [JsonProperty("email_from_field")]
        public string EmailFromField
        {
            get; set;
        }

        [JsonProperty("reply_to_email")]
        public String ReplyToEmail
        {
            get; set;
        }
    }
}
