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
            return this.Client.Get<Schedule[]>(path, new JsonArgs());
        }

        public Resource[] Resources(int scheduleId)
        {
            string path = "/resources";
            var data = new JsonArgs
            {
                { "schedule_id", ValidateId(scheduleId).ToString() }
            };
            return this.Client.Get<Resource[]>(path, data);
        }

        public FieldList[] GetFieldList(int scheduleId)
        {
            string path = "/field_list";
            var query = new JsonArgs
            {
                { "schedule_id", ValidateId(scheduleId).ToString() }
            };
            return this.Client.Get<FieldList[]>(path, query);
        }
    }
}