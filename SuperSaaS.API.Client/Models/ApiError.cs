using System;
using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    // One entry of the "errors" array the server returns with a non-2xx response.
    public class ApiError
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
