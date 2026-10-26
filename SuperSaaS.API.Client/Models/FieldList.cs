using System;
using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    public class FieldList
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
