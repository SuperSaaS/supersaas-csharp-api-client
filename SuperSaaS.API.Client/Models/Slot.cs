using System;

namespace SuperSaaS.API.Models
{
    /// <summary>A slot of a capacity schedule.</summary>
    public class Slot : BaseModel
    {
        /// <summary>Number of places: <c>"-"</c> or blank is unlimited, <c>"0"</c> means no places.</summary>
        public string capacity { get; set; }

        /// <summary>Color of the slot.</summary>
        public string color { get; set; }

        /// <summary>Description of the slot.</summary>
        public string description { get; set; }

        /// <summary>End time of the slot.</summary>
        public DateTime finish { get; set; }

        /// <summary>The id of the slot.</summary>
        public int id { get; set; }

        /// <summary>Number of places taken.</summary>
        public int level { get; set; }

        /// <summary>Location of the slot.</summary>
        public string location { get; set; }

        /// <summary>Price of the slot.</summary>
        public string price { get; set; }

        /// <summary>Start time of the slot.</summary>
        public DateTime start { get; set; }

        /// <summary>Title of the slot.</summary>
        public string title { get; set; }

        /// <summary>Number of people on the waiting list.</summary>
        public int waiting { get; set; }

        /// <summary>The bookings in the slot.</summary>
        public Appointment[] appointments { get; set; }
    }
}
