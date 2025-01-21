using System;

namespace SuperSaaS.API.Models
{
    public class Group : BaseModel
    {
        public int id { get; set; }
        public string? name { get; set; }
        
        public Group()
        {
        }
    }
}
