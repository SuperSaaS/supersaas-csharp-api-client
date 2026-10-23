using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    // Slots are only available for capacity type schedules.
    public class Slots : BaseApi
    {
        public Slots(IClient client) : base(client)
        {
        }

        public Slot[] List(int scheduleId, int limit = 0, int offset = 0)
        {
            return this.List(scheduleId, DateTime.MinValue, DateTime.MinValue, limit, offset);
        }

        public Slot[] List(int scheduleId, DateTime startTime, DateTime finishTime, int limit = 0, int offset = 0)
        {
            string path = "/slots";
            JsonArgs data = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            if (startTime > DateTime.MinValue)
            {
                data.Add("start", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (finishTime > DateTime.MinValue)
            {
                data.Add("finish", finishTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (limit > 0)
            {
                data.Add("limit", limit.ToString());
            }
            if (offset > 0)
            {
                data.Add("offset", offset.ToString());
            }
            return this.Client.Get<Slot[]>(path, data);
        }

        public Slot Get(int scheduleId, int slotId)
        {
            string path = "/slots/" + slotId.ToString();
            JsonArgs data = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            return this.Client.Get<Slot>(path, data);
        }

        // Attributes: start, finish, title, description, location, capacity ("-" or blank is unlimited, "0" no places), price, color.
        public Slot Create(int scheduleId, Dictionary<string, string> attributes)
        {
            string path = "/slots";
            JsonArgs query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            Client.Post<Slot>(path, this.SlotData(attributes), query);
            Slot model = new Slot();
            int id = Client.GetResourceIdFromHeader();
            if (id > 0)
            {
                model.id = id;
            }
            return model;
        }

        public Slot Update(int scheduleId, int slotId, Dictionary<string, string> attributes)
        {
            string path = "/slots/" + slotId.ToString();
            JsonArgs query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            return this.Client.Put<Slot>(path, this.SlotData(attributes), query);
        }

        // Also deletes the bookings on the slot.
        public void Delete(int scheduleId, int slotId)
        {
            string path = "/slots/" + slotId.ToString();
            JsonArgs query = new JsonArgs {
                { "schedule_id", scheduleId.ToString() }
            };
            this.Client.Delete<Slot>(path, null, query);
        }

        private NestedJsonArgs SlotData(Dictionary<string, string> attributes)
        {
            JsonArgs slotData = new JsonArgs { };
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                slotData.Add(entry.Key, entry.Value);
            }
            return new NestedJsonArgs
            {
                { "slot", slotData }
            };
        }
    }
}
