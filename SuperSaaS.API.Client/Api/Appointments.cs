using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>Appointment and booking endpoints.</summary>
    public class Appointments : BaseApi
    {
        /// <summary>Creates the appointment endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Appointments(IClient client) : base(client)
        {
        }

        /// <summary>Gets the agenda (upcoming appointments) of a user in a schedule.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="fromTime">Only return appointments from this time; <see cref="DateTime.MinValue"/> for the API default.</param>
        /// <returns>The appointments.</returns>
        public Appointment[] Agenda(int scheduleId, int userId, DateTime fromTime)
        {
            return Client.Get<Appointment[]>("/agenda/" + ApiParams.Format(scheduleId),
                AgendaQuery(userId, fromTime, false));
        }

        /// <summary>Gets the agenda slots of a user in a capacity schedule.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="fromTime">Only return slots from this time; <see cref="DateTime.MinValue"/> for the API default.</param>
        /// <returns>The slots with the user's bookings.</returns>
        public Slot[] AgendaSlots(int scheduleId, int userId, DateTime fromTime)
        {
            return Client.Get<Slot[]>("/agenda/" + ApiParams.Format(scheduleId), AgendaQuery(userId, fromTime, true));
        }

        /// <summary>Gets available (free) times in a schedule.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="fromTime">Start searching from this time.</param>
        /// <param name="lengthMinutes">Length of the requested appointment in minutes; 0 for the schedule default.</param>
        /// <param name="resource">Only search the availability of this resource.</param>
        /// <param name="full">Return full slot or appointment details.</param>
        /// <param name="limit">Maximum number of results; 0 for the API default.</param>
        /// <returns>The available times.</returns>
        public Appointment[] Available(int scheduleId, DateTime fromTime, int lengthMinutes = 0, string resource = null,
            bool full = false, int limit = 0)
        {
            var data = new JsonArgs
            {
                { "from", ApiParams.Format(fromTime) }
            };
            if (lengthMinutes > 0)
            {
                data.Add("length", ApiParams.Format(lengthMinutes));
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
                data.Add("maxresults", ApiParams.Format(limit));
            }
            return Client.Get<Appointment[]>("/free/" + ApiParams.Format(scheduleId), data);
        }

        /// <summary>Gets the appointments of a schedule.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="form">Include the form data of each appointment.</param>
        /// <param name="limit">Maximum number of appointments to return; 0 for the API default.</param>
        /// <returns>The appointments.</returns>
        public Appointment[] List(int scheduleId, bool form, int limit = 0)
        {
            return List(scheduleId, form, DateTime.MinValue, limit);
        }

        /// <summary>Gets the appointments of a schedule starting from <paramref name="startTime"/>.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="form">Include the form data of each appointment.</param>
        /// <param name="startTime">Only return appointments from this time; <see cref="DateTime.MinValue"/> for the API default.</param>
        /// <param name="limit">Maximum number of appointments to return; 0 for the API default.</param>
        /// <returns>The appointments.</returns>
        public Appointment[] List(int scheduleId, bool form, DateTime startTime, int limit = 0)
        {
            JsonArgs data = ScheduleQuery(scheduleId);
            if (startTime > DateTime.MinValue)
            {
                data.Add("from", ApiParams.Format(startTime));
            }
            if (form)
            {
                data.Add("form", "true");
            }
            if (limit > 0)
            {
                data.Add("limit", ApiParams.Format(limit));
            }
            return Client.Get<Appointment[]>("/bookings", data);
        }

        /// <summary>Gets a single appointment.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="appointmentId">The id of the appointment.</param>
        /// <returns>The appointment.</returns>
        public Appointment Get(int scheduleId, int appointmentId)
        {
            return Client.Get<Appointment>("/bookings/" + ApiParams.Format(appointmentId), ScheduleQuery(scheduleId));
        }

        /// <summary>Creates an appointment (booking) for a user.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="userId">The id of the user the appointment is for.</param>
        /// <param name="attributes">
        /// Appointment attributes such as <c>full_name</c>, <c>email</c>, <c>start</c>/<c>finish</c>, or
        /// <c>slot_id</c> for capacity schedules.
        /// </param>
        /// <param name="form">Attach the form data.</param>
        /// <param name="webhook">Trigger the webhooks configured for new appointments.</param>
        /// <returns>
        /// A new <see cref="Appointment"/> whose <c>id</c> is taken from the Location header of the response; the
        /// other properties are not filled in.
        /// </returns>
        public Appointment Create(int scheduleId, int userId, Dictionary<string, string> attributes, bool form = false,
            bool webhook = false)
        {
            JsonArgs query = ScheduleQuery(scheduleId);
            query.Add("user_id", ApiParams.Format(userId));
            AddFlags(query, form, webhook);

            var model = new Appointment();
            int id = PostAndGetResourceId<Appointment>("/bookings", BookingData(attributes), query);
            if (id > 0)
            {
                model.id = id;
            }
            return model;
        }

        /// <summary>Updates an appointment.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="appointmentId">The id of the appointment.</param>
        /// <param name="attributes">The appointment attributes to change.</param>
        /// <param name="form">Attach the form data.</param>
        /// <param name="webhook">Trigger the webhooks configured for appointment changes.</param>
        /// <returns>The API response, if any.</returns>
        public Appointment Update(int scheduleId, int appointmentId, Dictionary<string, string> attributes,
            bool form = false, bool webhook = false)
        {
            JsonArgs query = ScheduleQuery(scheduleId);
            AddFlags(query, form, webhook);
            return Client.Put<Appointment>("/bookings/" + ApiParams.Format(appointmentId), BookingData(attributes),
                query);
        }

        /// <summary>Deletes an appointment.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="appointmentId">The id of the appointment.</param>
        public void Delete(int scheduleId, int appointmentId)
        {
            Client.Delete<Appointment>("/bookings/" + ApiParams.Format(appointmentId), null, ScheduleQuery(scheduleId));
        }

        /// <summary>Gets the appointments of a schedule that changed since <paramref name="fromTime"/>.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <param name="fromTime">Return changes made after this time.</param>
        /// <returns>The changed appointments in <see cref="Models.Changes.bookings"/>.</returns>
        public Changes Changes(int scheduleId, DateTime fromTime)
        {
            var data = new JsonArgs
            {
                { "from", ApiParams.Format(fromTime) }
            };
            return Client.Get<Changes>("/changes/" + ApiParams.Format(scheduleId), data);
        }

        /// <summary>Gets the slots of a capacity schedule that changed since <paramref name="fromTime"/>.</summary>
        /// <param name="scheduleId">The id of the capacity schedule.</param>
        /// <param name="fromTime">Return changes made after this time.</param>
        /// <returns>The changed slots in <see cref="Models.Changes.slots"/>.</returns>
        public Changes ChangesSlots(int scheduleId, DateTime fromTime)
        {
            var data = new JsonArgs
            {
                { "from", ApiParams.Format(fromTime) },
                { "slot", "true" }
            };
            return Client.Get<Changes>("/changes/" + ApiParams.Format(scheduleId), data);
        }

        private static JsonArgs AgendaQuery(int userId, DateTime fromTime, bool slot)
        {
            var data = new JsonArgs
            {
                { "user", ApiParams.Format(userId) }
            };
            if (slot)
            {
                data.Add("slot", "true");
            }
            if (fromTime > DateTime.MinValue)
            {
                data.Add("from", ApiParams.Format(fromTime));
            }
            return data;
        }

        private static JsonArgs ScheduleQuery(int scheduleId)
        {
            return new JsonArgs
            {
                { "schedule_id", ApiParams.Format(scheduleId) }
            };
        }

        private static NestedJsonArgs BookingData(Dictionary<string, string> attributes)
        {
            return new NestedJsonArgs
            {
                { "booking", ApiParams.ToJsonArgs(attributes) }
            };
        }

        private static void AddFlags(JsonArgs query, bool form, bool webhook)
        {
            if (webhook)
            {
                query.Add("webhook", "true");
            }
            if (form)
            {
                query.Add("form", "true");
            }
        }
    }
}
