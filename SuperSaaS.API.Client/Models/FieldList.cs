using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SuperSaaS.API.Models
{
    public class FieldList : BaseModel
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? label { get; set; }
        public bool advanced { get; set; }
        private object? _spec;
        public object? spec
        {
            get => _spec;
            set
            {
                if (value is List<object> list)
                {
                    var tempList = new List<object>();
                    foreach (var item in list)
                    {
                        if (item?.ToString() is string str)
                        {
                            var deserialized = JsonSerializer.Deserialize<object>(str);
                            if (deserialized != null)
                            {
                                tempList.Add(deserialized);
                            }
                        }
                    }
                    _spec = tempList;
                }
                else
                {
                    _spec = value;
                }
            }
        }

        public FieldList()
        {
            name = string.Empty;
            type = string.Empty;
            label = string.Empty;
            advanced = false;
            _spec = null;
        }

        public static FieldList FromDictionary(Dictionary<string, object> dict)
        {
            var field = new FieldList();
            if (dict.ContainsKey("name")) field.name = dict["name"]?.ToString();
            if (dict.ContainsKey("type")) field.type = dict["type"]?.ToString();
            if (dict.ContainsKey("label")) field.label = dict["label"]?.ToString();
            if (dict.ContainsKey("advanced") && dict["advanced"] is bool adv) field.advanced = adv;
            if (dict.ContainsKey("spec")) field.spec = dict["spec"];
            return field;
        }
    }
}