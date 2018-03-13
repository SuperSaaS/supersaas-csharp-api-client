using System;

namespace SuperSaaS.API.Models
{
    public class Form : BaseModel
    {
        public Object content { get; set; }
        public DateTime created_on { get; set; }
        public bool deleted { get; set; }
        public int id { get; set; }
        public int reservation_process_id { get; set; }
        public int super_form_id { get; set; }
        public DateTime updated_on { get; set; }
        public int user_id { get; set; }

        public Form()
        {
        }
    }
}
