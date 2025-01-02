using System;
namespace SuperSaaS.API.Models
{
    public class Appointment : BaseModel
    {
        public string address { get; set; }
        public string country { get; set; }
        public string created_by { get; set; }
        public string description { get; set; }
        public DateTime created_on { get; set; }
        public bool deleted { get; set; }
        public string email { get; set; }
        public string field_1 { get; set; }
        public string field_2 { get; set; }
        public string field_1_r { get; set; }
        public string field_2_r { get; set; }
        public DateTime finish { get; set; }
        public string full_name { get; set; }
        public int form_id { get; set; }
        public int id { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string price { get; set; }
        public string res_name { get; set; }
        public int resource_id { get; set; }
        public int schedule_id { get; set; }
        public string schedule_name { get; set; }
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int slot_id { get; set; }
        public DateTime start { get; set; }
        public string status { get; set; }
        public string super_field { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_on { get; set; }
        public int user_id { get; set; }
        public bool waitlisted { get; set; }

        private Form _form;
        public Form form 
        {
            get => _form;
            set 
            {
                if (value is Dictionary<string, object> dict)
                {
                    _form = new Form(dict);
                }
                else
                {
                    _form = value;
                }
            }
        }

        private Slot _slot;
        public Slot slot
        {
            get => _slot;
            set 
            {
                if (value is Dictionary<string, object> dict)
                {
                    _slot = new Slot(dict);
                }
                else
                {
                    _slot = value;
                }
            }
        }

        public Appointment()
        {
        }
    }
}
