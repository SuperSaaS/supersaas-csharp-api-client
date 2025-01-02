using System;

namespace SuperSaaS.API.Models
{
    public class SuperForm : BaseModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public SuperForm()
        {
        }
    }
}