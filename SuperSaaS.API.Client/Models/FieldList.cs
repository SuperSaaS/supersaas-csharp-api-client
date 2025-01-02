using System;
using System.Collections.Generic;
using System.;

namespace SuperSaaS.API.Models
{
    public class FieldList : BaseModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public bool advanced { get; set; }

        // Backing field for Spec
        private object _spec;

        // Property with custom getter and setter for Spec
        public object spec
        {
            get => _spec;
            set
            {
                // If the value is an array (in C#, a List<object>), parse each element as JSON
                // Otherwise, assign the value directly
                if (value is List<object> list)
                {
                    var tempList = new List<object>();
                    foreach (var item in list)
                    {
                        // Assuming each item in the list is a JSON string that needs to be parsed
                        // Newtonsoft.Json library is used for parsing
                        tempList.Add(JsonSerializer.Deserialize<object>(item.ToString()));

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
        }
    }
}