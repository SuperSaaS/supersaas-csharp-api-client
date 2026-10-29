using Newtonsoft.Json;

namespace SuperSaaS.API.Models
{
    /// <summary>A webhook subscription.</summary>
    public class Hook
    {
        /// <summary>The id of the hook.</summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>The id of the schedule or super form the hook belongs to.</summary>
        [JsonProperty("parent_id")]
        public int ParentId { get; set; }
    }
}
