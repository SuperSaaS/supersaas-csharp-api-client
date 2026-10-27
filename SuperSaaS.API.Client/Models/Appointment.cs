using System;

namespace SuperSaaS.API.Models
{
    /// <summary>An appointment, reservation or booking.</summary>
    public class Appointment : BaseModel
    {
        /// <summary>Address entered with the appointment.</summary>
        public string address { get; set; }

        /// <summary>Country entered with the appointment (two-letter ISO code).</summary>
        public string country { get; set; }

        /// <summary>Who created the appointment.</summary>
        public string created_by { get; set; }

        /// <summary>Description of the appointment.</summary>
        public string description { get; set; }

        /// <summary>When the appointment was created.</summary>
        public DateTime created_on { get; set; }

        /// <summary>Whether the appointment has been deleted.</summary>
        public bool deleted { get; set; }

        /// <summary>Email address entered with the appointment.</summary>
        public string email { get; set; }

        /// <summary>Custom field 1 of the user.</summary>
        public string field_1 { get; set; }

        /// <summary>Custom field 2 of the user.</summary>
        public string field_2 { get; set; }

        /// <summary>Custom field 1 of the appointment.</summary>
        public string field_1_r { get; set; }

        /// <summary>Custom field 2 of the appointment.</summary>
        public string field_2_r { get; set; }

        /// <summary>End time of the appointment.</summary>
        public string finish { get; set; }

        /// <summary>Full name entered with the appointment.</summary>
        public string full_name { get; set; }

        /// <summary>The id of the appointment.</summary>
        public int id { get; set; }

        /// <summary>Mobile phone number entered with the appointment.</summary>
        public string mobile { get; set; }

        /// <summary>Name of the user who made the appointment.</summary>
        public string name { get; set; }

        /// <summary>Phone number entered with the appointment.</summary>
        public string phone { get; set; }

        /// <summary>Price of the appointment.</summary>
        public string price { get; set; }

        /// <summary>Name of the booked resource.</summary>
        public string res_name { get; set; }

        /// <summary>The id of the booked resource.</summary>
        public int resource_id { get; set; }

        /// <summary>Role of the user who made the appointment.</summary>
        public int role { get; set; }

        /// <summary>The id of the schedule.</summary>
        public int schedule_id { get; set; }

        /// <summary>Name of the schedule.</summary>
        public string schedule_name { get; set; }

        /// <summary>The id of the booked service.</summary>
        public int service_id { get; set; }

        /// <summary>Name of the booked service.</summary>
        public string service_name { get; set; }

        /// <summary>The id of the booked slot (capacity schedules).</summary>
        public int slot_id { get; set; }

        /// <summary>Start time of the appointment.</summary>
        public DateTime start { get; set; }

        /// <summary>Status of the appointment.</summary>
        public string status { get; set; }

        /// <summary>Custom field visible to superusers only.</summary>
        public string super_field { get; set; }

        /// <summary>Who last updated the appointment.</summary>
        public DateTime updated_by { get; set; }

        /// <summary>When the appointment was last updated.</summary>
        public string updated_on { get; set; }

        /// <summary>The id of the user the appointment belongs to.</summary>
        public int user_id { get; set; }

        /// <summary>Whether the appointment is on the waiting list.</summary>
        public bool waitlisted { get; set; }

        /// <summary>The form attached to the appointment, if requested.</summary>
        public Form form { get; set; }

        /// <summary>The slot of the appointment (capacity schedules).</summary>
        public Slot slot { get; set; }

        /// <summary>Creates an empty appointment.</summary>
        public Appointment()
        {
        }
    }
}
