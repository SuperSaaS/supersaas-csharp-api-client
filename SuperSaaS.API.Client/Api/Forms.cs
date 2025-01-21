using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

#nullable enable

namespace SuperSaaS.API.Api
{
    public class Forms : BaseApi
    {
        public Forms(IClient client) : base(client)
        {
        }

        public Form[] List(
            int templateFormId, 
            DateTime? fromTime = null, 
            string? user = null, 
            int? limit = null, 
            int? offset = null)
        {
            string path = "/forms";
            var data = new JsonArgs
            {
                { "form_id", ValidateId(templateFormId).ToString() }
            };

            if (fromTime.HasValue) data.Add("from", ValidateDateTime(fromTime.Value));
            if (!string.IsNullOrEmpty(user))
            {
                var validated = ValidateUser(user);
                string safeValue = validated?.ToString() ?? "";
                data.Add("user", safeValue);
            }
            if (limit.HasValue) data.Add("limit", ValidateNumber(limit.Value).ToString());
            if (offset.HasValue) data.Add("offset", ValidateNumber(offset.Value).ToString());
            return this.Client.Get<Form[]>(path, data) ?? Array.Empty<Form>();;
        }

        public Form Get(int formId)
        {
            string path = "/forms";
            var data = new JsonArgs
            {
                { "id", ValidateId(formId).ToString() }
            };

            return this.Client.Get<Form>(path, data) ?? new Form();
        }

        public SuperForm[] GetForms()
        {
            string path = "/super_forms";
            return this.Client.Get<SuperForm[]>(path, new JsonArgs()) ?? Array.Empty<SuperForm>();
        }
    }
}
