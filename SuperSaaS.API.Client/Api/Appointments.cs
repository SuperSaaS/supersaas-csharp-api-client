using System;
using System.Collections.Generic;
using System.Linq;
using SuperSaaS.API.Models;

#nullable enable

namespace SuperSaaS.API.Api
{
    public class Appointments : BaseApi
    {
        public Appointments(IClient client) : base(client)
        {
        }

        public Appointment[] Agenda(int scheduleId, string user, DateTime fromTime, bool slot)
        {
            string path = "/agenda/" + scheduleId;
            var data = new JsonArgs { { "user", user } };

            if (fromTime > DateTime.MinValue) data.Add("from", fromTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (slot) data.Add("slot", "true");

            return this.Client.Get<Appointment[]>(path, data) ?? Array.Empty<Appointment>();
        }

        public Appointment[] Range(
            int scheduleId,
            bool today = false,
            DateTime? fromTime = null,
            DateTime? to = null,
            bool? slot = false,
            string? user = null,
            int? resourceId = null,
            int? serviceId = null,
            int? limit = null,
            int? offset = null)
        {
            string path = $"/range/{scheduleId}";
            var parameters = new JsonArgs();

            if (today) parameters.Add("today", "true");
            if (fromTime.HasValue) parameters.Add("from", fromTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (to.HasValue) parameters.Add("to", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (slot == true) parameters.Add("slot", "true");
            if (!string.IsNullOrEmpty(user)) parameters.Add("user", user);
            if (limit.HasValue) parameters.Add("limit", limit.Value.ToString());
            if (offset.HasValue) parameters.Add("offset", offset.Value.ToString());
            if (resourceId.HasValue) parameters.Add("resource_id", resourceId.Value.ToString());
            if (serviceId.HasValue) parameters.Add("service_id", serviceId.Value.ToString());

            return this.Client.Get<Appointment[]>(path, parameters) ?? Array.Empty<Appointment>();
        }

        public Changes Changes(
            int scheduleId,
            DateTime? fromTime = null,
            DateTime? to = null,
            bool? slot = false,
            string? user = null,
            int? limit = null,
            int? offset = null)
        {
            string path = $"/changes/{scheduleId}";
            var parameters = new JsonArgs();

            if (fromTime.HasValue) parameters.Add("from", fromTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (to.HasValue) parameters.Add("to", to.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (slot == true) parameters.Add("slot", "true");
            if (!string.IsNullOrEmpty(user)) parameters.Add("user", user);
            if (limit.HasValue) parameters.Add("limit", limit.Value.ToString());
            if (offset.HasValue) parameters.Add("offset", offset.Value.ToString());

            return this.Client.Get<Changes>(path, parameters) ?? new Changes();
        }

        public Appointment[] Available(
            int scheduleId,
            DateTime fromTime,
            int lengthMinutes = 0,
            string? resource = null,
            bool? full = null,
            int? limit = null)
        {
            string path = "/free/" + scheduleId;
            var data = new JsonArgs { { "from", fromTime.ToString("yyyy-MM-dd HH:mm:ss") } };

            if (lengthMinutes > 0) data.Add("length", lengthMinutes.ToString());
            if (!string.IsNullOrEmpty(resource)) data.Add("resource", resource);
            if (full == true) data.Add("full", "true");
            if (limit.HasValue) data.Add("maxresults", limit.Value.ToString());

            return this.Client.Get<Appointment[]>(path, data) ?? Array.Empty<Appointment>();
        }

        public Appointment[] List(int scheduleId, bool form, DateTime startTime, DateTime finish, int limit = 0)
        {
            string path = "/bookings";
            var data = new JsonArgs { { "schedule_id", scheduleId.ToString() } };

            if (startTime > DateTime.MinValue) data.Add("start", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (finish > DateTime.MinValue) data.Add("finish", finish.ToString("yyyy-MM-dd HH:mm:ss"));
            if (form) data.Add("form", "true");
            if (limit > 0) data.Add("limit", limit.ToString());

            var response = this.Client.Get<Appointment[]>(path, data);
            return response ?? Array.Empty<Appointment>();
        }

        public Appointment Get(int scheduleId, int appointmentId)
        {
            string path = "/bookings/" + appointmentId;
            var data = new JsonArgs{ { "schedule_id", scheduleId.ToString() } };

            var result = this.Client.Get<Appointment>(path, data);
            return result ?? new Appointment();
        }

        public Appointment Create(int scheduleId, string userId, Dictionary<string, string> attributes, bool form = false, bool webhook = false)
        {
            string path = "/bookings";
            var appointmentData = new JsonArgs();
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                appointmentData.Add(entry.Key, entry.Value);
            }
            var data = new NestedJsonArgs { { "booking", appointmentData } };
            var query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() },
                { "user_id", userId }
            };
            if (webhook)
            {
                query.Add("webhook", "true");
            }
            if (form)
            {
                query.Add("form", "true");
            }

            Client.Post<Appointment>(path, data, query);

            // Build a local model
            Appointment model = new Appointment();
            int id = Client.GetResourceIdFromHeader();
            if (id > 0)
            {
                model.id = id;
            }
            return model;
        }

        public Appointment Update(int scheduleId, int appointmentId, Dictionary<string, string> attributes, bool form = false, bool webhook = false)
        {
            string path = "/bookings/" + appointmentId;
            var appointmentData = new JsonArgs();
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                appointmentData.Add(entry.Key, entry.Value);
            }
            var data = new NestedJsonArgs
            {
                { "booking", appointmentData }
            };
            var query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            if (webhook) query.Add("webhook", "true");
            if (form) query.Add("form", "true");
  
            var result = this.Client.Put<Appointment>(path, data, query);
            return result ?? new Appointment();
        }

        public void Delete(int scheduleId, int appointmentId, bool webhook = false)
        {
            var query = new JsonArgs { { "schedule_id", scheduleId.ToString() } };
            if (webhook) query.Add("webhook", "true");

            string path = "/bookings/" + appointmentId;
            this.Client.Delete<Appointment>(path, new NestedJsonArgs(), query);
        }

        private T[] MapSlotsOrBookings<T>(object? obj, bool slot = false) where T : BaseModel
        {
            if (obj is T[] array && slot)
            {
                return array ?? Array.Empty<T>();
            }
            else if (obj is Dictionary<string, object> dict)
            {
                if (dict.TryGetValue("slots", out var slotsObj) && slotsObj is object[] slotsArr)
                {
                    return slotsArr.Select(x => (T)Activator.CreateInstance(typeof(T), new[] { x })!).ToArray();
                }
                else if (dict.TryGetValue("bookings", out var bookingsObj) && bookingsObj is object[] bookingsArr)
                {
                    return bookingsArr.Select(x => (T)Activator.CreateInstance(typeof(T), new[] { x })!).ToArray();
                }
            }
            return Array.Empty<T>();
        }

        private string ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be a positive integer");
            return id.ToString();
        }

        private string ValidateDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue) throw new ArgumentException("DateTime cannot be null");
            return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private string ValidateUser(string user)
        {
            if (string.IsNullOrEmpty(user)) throw new ArgumentException("User cannot be null or empty");
            return user;
        }
    }
}
