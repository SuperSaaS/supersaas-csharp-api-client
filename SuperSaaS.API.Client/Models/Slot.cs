using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSaaS.API.Models
{
    public class Slot : BaseModel
    {
        public string? description { get; set; }
        public DateTime finish { get; set; }
        public int id { get; set; }
        public string? location { get; set; }
        public DateTime start { get; set; }
        public string? title { get; set; }
        public int count { get; set; }

        private List<Appointment> _bookings = new();
        public List<Appointment> bookings 
        {
            get => _bookings; 
            set => _bookings = value; 
        }

        public Slot()
        {
            _bookings = new List<Appointment>();
        }

        public static Slot FromDictionary(Dictionary<string, object> dict)
        {
            var slot = new Slot();
            return slot;
        }
    }
}

