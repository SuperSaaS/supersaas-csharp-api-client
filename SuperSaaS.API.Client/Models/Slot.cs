using System;
namespace SuperSaaS.API.Models
{
    public class Slot : BaseModel
    {
        public string description { get; set; }
        public DateTime finish { get; set; }
        public int id { get; set; }
        public string location { get; set; }
        public DateTime start { get; set; }
        public string title { get; set; }

        public Appointment[] appointments { get; set; }

        public Slot()
        {
        }
    }
}
