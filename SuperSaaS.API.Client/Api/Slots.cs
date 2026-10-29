using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>Slot endpoints. Slots are only available for capacity type schedules.</summary>
    public class Slots : BaseApi
    {
        /// <summary>Creates the slot endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Slots(IClient client) : base(client)
        {
        }

        /// <summary>Gets the slots of a capacity schedule, with optional pagination.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="limit">Maximum number of slots to return; 0 for the API default (100, max 2500).</param>
        /// <param name="offset">Number of slots to skip.</param>
        /// <returns>The slots.</returns>
        public Slot[] List(int scheduleId, int limit = 0, int offset = 0)
        {
            return List(scheduleId, DateTime.MinValue, DateTime.MinValue, limit, offset);
        }

        /// <summary>Gets the slots of a capacity schedule in a time range, with optional pagination.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="startTime">Only slots from this time; <see cref="DateTime.MinValue"/> for no limit.</param>
        /// <param name="finishTime">Only slots up to this time; <see cref="DateTime.MinValue"/> for no limit.</param>
        /// <param name="limit">Maximum number of slots to return; 0 for the API default (100, max 2500).</param>
        /// <param name="offset">Number of slots to skip.</param>
        /// <returns>The slots.</returns>
        public Slot[] List(int scheduleId, DateTime startTime, DateTime finishTime, int limit = 0, int offset = 0)
        {
            JsonArgs data = ApiParams.ScheduleQuery(scheduleId);
            if (startTime > DateTime.MinValue)
            {
                data.Add("start", ApiParams.Format(startTime));
            }
            if (finishTime > DateTime.MinValue)
            {
                data.Add("finish", ApiParams.Format(finishTime));
            }
            if (limit > 0)
            {
                data.Add("limit", ApiParams.Format(limit));
            }
            if (offset > 0)
            {
                data.Add("offset", ApiParams.Format(offset));
            }
            return Client.Get<Slot[]>("/slots", data);
        }

        /// <summary>Gets a single slot.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="slotId">The id of the slot.</param>
        /// <returns>The slot.</returns>
        public Slot Get(int scheduleId, int slotId)
        {
            return Client.Get<Slot>(SlotPath(slotId), ApiParams.ScheduleQuery(scheduleId));
        }

        /// <summary>Creates a slot in a capacity schedule.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="attributes">
        /// Slot attributes: <c>start</c>, <c>finish</c>, <c>title</c>, <c>description</c>, <c>location</c>,
        /// <c>capacity</c> (<c>"-"</c> or blank is unlimited, <c>"0"</c> means no places), <c>price</c> and
        /// <c>color</c>.
        /// </param>
        /// <returns>A slot with only its <see cref="Slot.id"/> set (0 when the server returned no id).</returns>
        public Slot Create(int scheduleId, Dictionary<string, string> attributes)
        {
            var model = new Slot();
            int id = PostAndGetResourceId<Slot>("/slots", SlotData(attributes), ApiParams.ScheduleQuery(scheduleId));
            if (id > 0)
            {
                model.id = id;
            }
            return model;
        }

        /// <summary>Updates a slot.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="slotId">The id of the slot.</param>
        /// <param name="attributes">The slot attributes to change; <c>null</c> values are not sent.</param>
        /// <returns>The API response, if any.</returns>
        public Slot Update(int scheduleId, int slotId, Dictionary<string, string> attributes)
        {
            return Client.Put<Slot>(SlotPath(slotId), SlotData(attributes), ApiParams.ScheduleQuery(scheduleId));
        }

        /// <summary>Deletes a slot, including the bookings on it.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="slotId">The id of the slot.</param>
        public void Delete(int scheduleId, int slotId)
        {
            Client.Delete<Slot>(SlotPath(slotId), null, ApiParams.ScheduleQuery(scheduleId));
        }

        private static string SlotPath(int slotId)
        {
            return "/slots/" + ApiParams.Format(slotId);
        }

        private static NestedJsonArgs SlotData(Dictionary<string, string> attributes)
        {
            return new NestedJsonArgs
            {
                { "slot", ApiParams.ToJsonArgs(attributes, skipNullValues: true) }
            };
        }
    }
}
