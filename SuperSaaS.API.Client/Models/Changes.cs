using System;
namespace SuperSaaS.API.Models
{
    public class Changes : BaseModel
    {
        public Appointment[] bookings { get; set; }
        public Slot[] slots { get; set; }
    }
}
