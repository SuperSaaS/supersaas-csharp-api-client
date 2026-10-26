using System;
namespace SuperSaaS.API.Models
{
    public class Slot : BaseModel
    {
        public string capacity { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public DateTime finish { get; set; }
        public int id { get; set; }
        public int level { get; set; }
        public string location { get; set; }
        public string price { get; set; }
        public DateTime start { get; set; }
        public string title { get; set; }
        public int waiting { get; set; }

        public Appointment[] appointments { get; set; }
    }
}
