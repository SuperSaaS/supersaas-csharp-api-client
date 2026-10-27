namespace SuperSaaS.API.Models
{
    /// <summary>A schedule of the account.</summary>
    public class Schedule : BaseModel
    {
        /// <summary>The id of the schedule.</summary>
        public int id { get; set; }

        /// <summary>The name of the schedule.</summary>
        public string name { get; set; }

        /// <summary>Creates an empty schedule.</summary>
        public Schedule()
        {
        }
    }
}
