using System;
namespace SuperSaaS.API.Models
{
    public class Promotion : BaseModel
    {
        public int id { get; set; }
        public string? code { get; set; }
        public string? description { get; set; }
        public int usage { get; set; }

         public Promotion()
        {
        }
    }
}
