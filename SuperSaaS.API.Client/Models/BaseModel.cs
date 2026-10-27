using System.Collections.Generic;

namespace SuperSaaS.API.Models
{
    /// <summary>Base class of the API models.</summary>
    public class BaseModel
    {
        /// <summary>Validation errors returned by the API, keyed by attribute name.</summary>
        public Dictionary<string, string[]> errors { get; set; }

        /// <summary>Creates an empty model.</summary>
        public BaseModel()
        {
        }
    }
}
