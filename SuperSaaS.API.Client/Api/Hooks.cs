using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>Webhook endpoints.</summary>
    public class Hooks : BaseApi
    {
        /// <summary>Creates the webhook endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Hooks(IClient client) : base(client)
        {
        }

        /// <summary>Subscribes <paramref name="targetUrl"/> to an event.</summary>
        /// <param name="targetUrl">The URL the server posts the event to.</param>
        /// <param name="evt">
        /// The trigger letter (or a word starting with it): <c>U</c> user created, <c>M</c> user modified, <c>N</c>
        /// new appointment, <c>C</c> changed appointment, <c>R</c> reminder, <c>F</c> follow-up, <c>S</c> form
        /// submitted, <c>O</c> form changed, <c>H</c> mail, <c>P</c> credit purchase.
        /// </param>
        /// <param name="parentId">
        /// The schedule id (N, C, R, F) or super form id (S, O); for U, M, H and P it is ignored by the server but
        /// must be present.
        /// </param>
        /// <returns>The created hook.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="targetUrl"/> or <paramref name="evt"/> is empty.</exception>
        public Hook Create(string targetUrl, string evt, int parentId)
        {
            // The server reads these params from the query string as well as from the body.
            var query = new JsonArgs
            {
                { "target_url", ApiParams.Required(targetUrl, nameof(targetUrl)) },
                { "event", ApiParams.Required(evt, nameof(evt)) },
                { "parent_id", ApiParams.Format(parentId) }
            };
            return Client.Post<Hook>("/hooks", null, query);
        }

        /// <summary>Deletes a hook.</summary>
        /// <param name="hookId">The id of the hook.</param>
        /// <param name="parentId">The id of the schedule or super form the hook belongs to.</param>
        public void Delete(int hookId, int parentId)
        {
            var query = new JsonArgs
            {
                { "id", ApiParams.Format(hookId) },
                { "parent_id", ApiParams.Format(parentId) }
            };
            Client.Delete<Hook>("/hooks", null, query);
        }
    }
}
