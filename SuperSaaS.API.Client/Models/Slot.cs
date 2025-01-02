using System;
using System.Collections.Generic;
using System.Linq;

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
        public int count { get; set; }
        public string title { get; set; }

        private List<Appointment> _bookings;
        public List<Appointment> bookings
        {
            get => _bookings;
            set
            {
                if (value is List<Dictionary<string, object>> dictList)
                {
                    _bookings = dictList.Select(dict => new Appointment(dict)).ToList();
                }
                else
                {
                    _bookings = value;
                }
            }
        }

        public Slot()
        {
            _bookings = new List<Appointment>();
        }
    }
}

