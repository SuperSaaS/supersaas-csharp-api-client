using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Appointments : BaseApi
    {
        public Appointments(IClient client) : base(client)
        {
        }

        public Appointment[] Agenda(int scheduleId, int userId, DateTime fromTime)
        {
            string path = "/agenda/" + scheduleId.ToString();
            JsonArgs data = new JsonArgs {
                {"user", userId.ToString()}
            };
            if (fromTime > DateTime.MinValue)
            {
                data.Add("from", fromTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return this.Client.Get<Appointment[]>(path, data);
        }

        public Slot[] AgendaSlots(int scheduleId, int userId, DateTime fromTime)
        {
            string path = "/agenda/" + scheduleId.ToString();
            JsonArgs data = new JsonArgs {
                {"user", userId.ToString()},
                {"slot", "true"}
            };
            if (fromTime > DateTime.MinValue) {
                data.Add("from", fromTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return this.Client.Get<Slot[]>(path, data);
        }

        public Appointment[] Available(int scheduleId, DateTime fromTime, int lengthMinutes = 0, string resource = null, bool full = false, int limit = 0)
        {
            string path = "/bookings";
            JsonArgs data = new JsonArgs {
                {"schedule_id", scheduleId.ToString()},
                {"from", fromTime.ToString("yyyy-MM-dd HH:mm:ss")}
            };
            if (lengthMinutes > 0)
            {
                data.Add("length", lengthMinutes.ToString());
            }
            if (resource != null)
            {
                data.Add("resource", resource);
            }
            if (full)
            {
                data.Add("full", "true");
            }
            if (limit > 0)
            {
                data.Add("limit", limit.ToString());
            }
            return this.Client.Get<Appointment[]>(path, data);
        }

        public Appointment[] List(int scheduleId, bool form, int limit = 0) {
            return this.List(scheduleId, form, DateTime.MinValue, limit);
        }

        public Appointment[] List(int scheduleId, bool form, DateTime startTime, int limit = 0)
        {
            string path = "/bookings";
            JsonArgs data = new JsonArgs {
                {"schedule_id", scheduleId.ToString()},
            };
            if (startTime > DateTime.MinValue)
            {
                data.Add("from", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (form)
            {
                data.Add("form", "true");
            }
            if (limit > 0)
            {
                data.Add("limit", limit.ToString());
            }
            return this.Client.Get<Appointment[]>(path, data);
        }

        public Appointment Get(int scheduleId, int appointmentId)
        {
            string path = "/bookings/" + appointmentId;
            JsonArgs data = new JsonArgs
            {
                { "schedule_id", scheduleId.ToString() }
            };
            return this.Client.Get<Appointment>(path, data);
        }

        public Appointment Create(int scheduleId, int userId, Dictionary<string, string> attributes, bool form = false, bool webhook = false)
        {
            string path = "/bookings";
            JsonArgs appointmentData = new JsonArgs { };
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                appointmentData.Add(entry.Key, entry.Value);
            }
            NestedJsonArgs data = new NestedJsonArgs
            {
                {"booking", appointmentData }
            };
            JsonArgs query = new JsonArgs { 
                { "schedule_id", scheduleId.ToString() },
                { "user_id", userId.ToString() }
            };
            if (webhook)
            {
                query.Add("webhook", "true");
            }
            if (form)
            {
                query.Add("form", "true");
            }
            return this.Client.Post<Appointment>(path, data, query);
        }

        public Appointment Update(int scheduleId, int appointmentId, Dictionary<string, string> attributes, bool form = false, bool webhook = false)
        {
            string path = "/bookings/" + appointmentId.ToString();
            JsonArgs appointmentData = new JsonArgs { };
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                appointmentData.Add(entry.Key, entry.Value);
            }
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "appointment", appointmentData }
            };
            JsonArgs query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            if (webhook)
            {
                query.Add("webhook", "true");
            }
            if (form)
            {
                query.Add("form", "true");
            }
            return this.Client.Put<Appointment>(path, data, query);
        }

        public void Delete(int scheduleId, int appointmentId)
        {
            JsonArgs query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            string path = "/bookings/" + appointmentId;
            this.Client.Delete<Appointment>(path, null, query);
        }

        public Appointment[] Changes(int scheduleId, DateTime fromTime)
        {
            string path = "/changes/" + scheduleId.ToString();
            JsonArgs data = new JsonArgs {
                {"from", fromTime.ToString("yyyy-MM-dd HH:mm:ss")}
            };
            return this.Client.Get<Appointment[]>(path, data);
        }

        public Slot[] ChangesSlots(int scheduleId, DateTime fromTime)
        {
            string path = "/changes/" + scheduleId.ToString();
            JsonArgs data = new JsonArgs {
                {"from", fromTime.ToString("yyyy-MM-dd HH:mm:ss")},
                {"slot", "true"}
            };
            return this.Client.Get<Slot[]>(path, data);
        }
    
    }
}
