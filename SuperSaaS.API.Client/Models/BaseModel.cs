using System;
using System.Collections.Generic;

namespace SuperSaaS.API.Models
{
    public class BaseModel
    {
        public Dictionary<string, string[]> errors { get; set; }

        public BaseModel()
        {
        }
    }
}
