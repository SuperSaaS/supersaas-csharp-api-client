using System;

namespace SuperSaaS.API.Models
{
    /// <summary>A slot of a capacity schedule.</summary>
    public class Slot : BaseModel
    {
        /// <summary>Description of the slot.</summary>
        public string description { get; set; }

        /// <summary>End time of the slot.</summary>
        public DateTime finish { get; set; }

        /// <summary>The id of the slot.</summary>
        public int id { get; set; }

        /// <summary>Location of the slot.</summary>
        public string location { get; set; }

        /// <summary>Start time of the slot.</summary>
        public DateTime start { get; set; }

        /// <summary>Title of the slot.</summary>
        public string title { get; set; }

        /// <summary>The bookings in the slot.</summary>
        public Appointment[] appointments { get; set; }
    }
}
