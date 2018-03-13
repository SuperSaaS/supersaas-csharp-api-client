using System;
namespace SuperSaaS.API.Models
{
    public class Schedule : BaseModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public Schedule()
        {
        }
    }
}
