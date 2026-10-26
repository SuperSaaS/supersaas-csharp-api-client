using System;
using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    public class Hook
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }
    }
}
