using System;
namespace SuperSaaS.API.Models
{
    public class User : BaseModel
    {
        public string address { get; set; }
        public string country { get; set; }
        public DateTime created_on { get; set; }
        public int credit { get; set; }
        public string email { get; set; }
        public string field_1 { get; set; }
        public string field_2 { get; set; }
        public string fk { get; set; }
        public string full_name { get; set; }
        public int id { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int role { get; set; }
        public string super_field { get; set; }

        public Form form { get; set; }

        public User()
        {
        }
    }
}
