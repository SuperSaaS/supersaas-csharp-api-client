namespace SuperSaaS.API.Models
{
    /// <summary>A resource or service of a schedule.</summary>
    public class Resource : BaseModel
    {
        /// <summary>The id of the resource.</summary>
        public int id { get; set; }

        /// <summary>The name of the resource.</summary>
        public string name { get; set; }

        /// <summary>Creates an empty resource.</summary>
        public Resource()
        {
        }
    }
}
