using System;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>Form endpoints.</summary>
    public class Forms : BaseApi
    {
        /// <summary>Creates the form endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Forms(IClient client) : base(client)
        {
        }

        /// <summary>Gets all forms filled in using a template form.</summary>
        /// <param name="formId">The id of the template form (super form).</param>
        /// <returns>The forms based on the template.</returns>
        public Form[] List(int formId)
        {
            return List(formId, DateTime.MinValue);
        }

        /// <summary>Gets the forms filled in using a template form, changed since <paramref name="fromTime"/>.</summary>
        /// <param name="formId">The id of the template form (super form).</param>
        /// <param name="fromTime">Only return forms changed after this time; <see cref="DateTime.MinValue"/> for all.</param>
        /// <returns>The forms based on the template.</returns>
        public Form[] List(int formId, DateTime fromTime)
        {
            var data = new JsonArgs
            {
                { "form_id", ApiParams.Format(formId) }
            };
            if (fromTime > DateTime.MinValue)
            {
                data.Add("from", ApiParams.Format(fromTime));
            }
            return Client.Get<Form[]>("/forms", data);
        }

        /// <summary>Gets a single form.</summary>
        /// <param name="formId">The id of the form.</param>
        /// <returns>The form.</returns>
        public Form Get(int formId)
        {
            var data = new JsonArgs
            {
                { "id", ApiParams.Format(formId) }
            };
            return Client.Get<Form>("/forms", data);
        }
    }
}
