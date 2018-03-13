using System;
namespace SuperSaaS.API.Models
{
    public class Resource : BaseModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public Resource()
        {
        }
    }
}
