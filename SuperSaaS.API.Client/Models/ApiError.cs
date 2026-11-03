using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    /// <summary>One entry of the <c>errors</c> array the server returns with an error response.</summary>
    public class ApiError
    {
        /// <summary>HTTP status of the error, e.g. <c>"404"</c>.</summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>Human readable description of the error.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Machine readable error code, e.g. <c>"taken"</c>.</summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
