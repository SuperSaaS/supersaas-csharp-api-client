using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    /// <summary>A field of a booking or user input form.</summary>
    public class FieldList
    {
        /// <summary>Name of the field, used as attribute key.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Type of the field.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Label of the field as shown on the form.</summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>Whether the field is required.</summary>
        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}
