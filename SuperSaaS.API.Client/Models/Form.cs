using System;
using System.Collections.Generic;

namespace SuperSaaS.API.Models
{
    public class Form : BaseModel
    {
        public Object? content { get; set; }
        public DateTime created_on { get; set; }
        public bool deleted { get; set; }
        public int id { get; set; }
        public int reservation_process_id { get; set; }
        public int super_form_id { get; set; }
        public DateTime updated_on { get; set; }
        public int user_id { get; set; }
        public string? updated_name { get; set; }

        public Form()
        {
        }

        public static Form FromDictionary(Dictionary<string, object> dict)
        {
            var form = new Form();
            if (dict.ContainsKey("content")) form.content = dict["content"];
            if (dict.ContainsKey("created_on") && dict["created_on"] is DateTime dt) form.created_on = dt;
            if (dict.ContainsKey("deleted") && dict["deleted"] is bool del) form.deleted = del;
            if (dict.ContainsKey("id") && dict["id"] is int id) form.id = id;
            if (dict.ContainsKey("reservation_process_id") && dict["reservation_process_id"] is int rpi) form.reservation_process_id = rpi;
            if (dict.ContainsKey("super_form_id") && dict["super_form_id"] is int sfi) form.super_form_id = sfi;
            if (dict.ContainsKey("updated_on") && dict["updated_on"] is DateTime uo) form.updated_on = uo;
            if (dict.ContainsKey("user_id") && dict["user_id"] is int ui) form.user_id = ui;
            if (dict.ContainsKey("updated_name")) form.updated_name = dict["updated_name"]?.ToString();
            return form;
        }
    }
}
