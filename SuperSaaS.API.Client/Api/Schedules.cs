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
