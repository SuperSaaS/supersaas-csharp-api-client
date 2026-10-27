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

        /// <summary>Gets all services/resources of a schedule. Does not work for capacity type schedules.</summary>
        /// <param name="scheduleId">The id of the schedule.</param>
        /// <returns>The resources or services of the schedule.</returns>
        public Resource[] Resources(int scheduleId)
        {
            var data = new JsonArgs
            {
                { "schedule_id", ApiParams.Format(scheduleId) }
            };
            return Client.Get<Resource[]>("/resources", data);
        }
    }
}
