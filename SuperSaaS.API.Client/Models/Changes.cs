namespace SuperSaaS.API.Models
{
    /// <summary>Recently changed appointments or slots of a schedule.</summary>
    public class Changes : BaseModel
    {
        /// <summary>The changed appointments.</summary>
        public Appointment[] bookings { get; set; }

        /// <summary>The changed slots (capacity schedules).</summary>
        public Slot[] slots { get; set; }
    }
}
