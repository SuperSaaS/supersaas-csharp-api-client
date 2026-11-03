using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>Schedule and resource endpoints.</summary>
    public class Schedules : BaseApi
    {
        /// <summary>Creates the schedule endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Schedules(IClient client) : base(client)
        {
        }

        /// <summary>Gets all account schedules.</summary>
        /// <returns>The schedules of the account.</returns>
        public Schedule[] List()
        {
            return Client.Get<Schedule[]>("/schedules");
        }

        /// <summary>
        /// Gets the fields of the input form for bookings on a schedule; without a schedule id, the user fields.
        /// </summary>
        /// <param name="scheduleId">The id of the schedule, or <c>null</c> for the user fields.</param>
        /// <returns>The input fields.</returns>
        public FieldList[] InputFieldList(int? scheduleId = null)
        {
            JsonArgs data = scheduleId.HasValue ? ApiParams.ScheduleQuery(scheduleId.Value) : null;
            return Client.Get<FieldList[]>("/input_field_list", data);
        }

        /// <summary>Gets all services/resources of a schedule. Does not work for capacity type schedules.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <returns>The resources or services of the schedule.</returns>
        public Resource[] Resources(int scheduleId)
        {
            return Client.Get<Resource[]>("/resources", ApiParams.ScheduleQuery(scheduleId));
        }
    }
}
