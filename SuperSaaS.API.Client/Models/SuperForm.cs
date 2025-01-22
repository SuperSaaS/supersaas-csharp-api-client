using System;
using System.Collections.Generic; 

namespace SuperSaaS.API.Models
{
    public class SuperForm : BaseModel
    {
        public int id { get; set; }
        public string? name { get; set; }

        public SuperForm()
        {
            name = string.Empty;
            id = 0;
        }

        public static SuperForm FromDictionary(Dictionary<string, object> dict)
        {
            var form = new SuperForm();
            if (dict.ContainsKey("id") && dict["id"] is int id) form.id = id;
            if (dict.ContainsKey("name")) form.name = dict["name"]?.ToString();
            return form;
        }
    }
}