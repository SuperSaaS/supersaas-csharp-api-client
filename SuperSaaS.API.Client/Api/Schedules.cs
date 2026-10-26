using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Schedules : BaseApi
    {
        public Schedules(IClient client) : base(client)
        {
        }

        public Schedule[] List()
        {
            string path = "/schedules";
            return this.Client.Get<Schedule[]>(path);
        }

        // Fields of the input form for bookings on a schedule; without scheduleId the user fields.
        public FieldList[] InputFieldList(int? scheduleId = null)
        {
            string path = "/input_field_list";
            JsonArgs data = null;
            if (scheduleId.HasValue)
            {
                data = new JsonArgs {
                    { "schedule_id", scheduleId.Value.ToString() }
                };
            }
            return this.Client.Get<FieldList[]>(path, data);
        }

        public Resource[] Resources(int scheduleId)
        {
            string path = "/resources";
            JsonArgs data = new JsonArgs
            {
                { "schedule_id", scheduleId.ToString() }
            };
            return this.Client.Get<Resource[]>(path, data);
        }
    }
}
