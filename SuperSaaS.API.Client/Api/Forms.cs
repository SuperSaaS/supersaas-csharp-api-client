using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Forms : BaseApi
    {
        public Forms(IClient client) : base(client)
        {
        }

        public Form[] List(int formId) {
            return this.List(formId, DateTime.MinValue);
        }

        public Form[] List(int formId, DateTime fromTime)
        {
            string path = "/forms";
            JsonArgs data = new JsonArgs
            {
                { "form_id", formId.ToString() }
            };
            if (fromTime > DateTime.MinValue) {
                data.Add("from", fromTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return this.Client.Get<Form[]>(path, data);
        }

        public Form Get(int formId)
        {
            string path = "/forms";
            JsonArgs data = new JsonArgs
            {
                { "id", formId.ToString() }
            };
            return this.Client.Get<Form>(path, data);
        }
    }
}
