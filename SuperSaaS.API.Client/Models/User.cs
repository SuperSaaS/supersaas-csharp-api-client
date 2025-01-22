using System;
using System.Collections.Generic;

namespace SuperSaaS.API.Models
{
    public class User : BaseModel
    {
        public static readonly int[] ROLES = new int[] { 3, 4, -1 };

        public string password { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string timezone { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public DateTime created_on { get; set; } = DateTime.MinValue;
        public DateTime updated_on { get; set; } = DateTime.MinValue;
        public int credit { get; set; } = 0;
        public string location { get; set; } = "";
        public string email { get; set; } = string.Empty;
        public string field_1 { get; set; } = string.Empty;
        public string field_2 { get; set; } = string.Empty;
        public int fk { get; set; } = 0;
        public int id { get; set; } = 0;
        public string mobile { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;

        public int role { get; set; } = 3;

        public string super_field { get; set; } = string.Empty;
        public bool? webhook { get; set; } = null;

        public Group group { get; set; } = new Group();

        private Form _form = new Form();
        public Form form
        {
            get => _form;
            set
            {
                _form = value ?? new Form();
            }
        }

        public User()
        {
        }
    }
}
