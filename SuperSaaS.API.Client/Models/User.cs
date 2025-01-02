using System;
namespace SuperSaaS.API.Models
{
    public class User : BaseModel
    {
        public static readonly int[] ROLES = new int[] { 3, 4, -1 };

        public string password { get; set; }
        public string full_name { get; set; }
        public string timezone { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public int credit { get; set; }
        public string email { get; set; }
        public string field_1 { get; set; }
        public string field_2 { get; set; }
        public int fk { get; set; }
        public string full_name { get; set; }
        public int id { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int role { get; set; }
        public string super_field { get; set; }
        public bool? webhook { get; set; }
        public Group group { get; set; }

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



        public User()
        {
        }
    }
}
