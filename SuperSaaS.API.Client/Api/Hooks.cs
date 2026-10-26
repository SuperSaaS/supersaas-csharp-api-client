using System;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Hooks : BaseApi
    {
        public Hooks(IClient client) : base(client)
        {
        }

        // evt is the trigger letter (or a word starting with it): U user created, M user modified,
        // N new appointment, C changed appointment, R reminder, F follow-up, S form submitted,
        // O form changed, H mail, P credit purchase.
        // parentId is the schedule id (N, C, R, F) or super form id (S, O); for U, M, H and P it is
        // ignored by the server but must be present.
        public Hook Create(string targetUrl, string evt, int parentId)
        {
            string path = "/hooks";
            // The server reads these params from the query string as well as from the body.
            JsonArgs query = new JsonArgs {
                { "target_url", targetUrl },
                { "event", evt },
                { "parent_id", parentId.ToString() }
            };
            return this.Client.Post<Hook>(path, null, query);
        }

        public void Delete(int hookId, int parentId)
        {
            string path = "/hooks";
            JsonArgs query = new JsonArgs {
                { "id", hookId.ToString() },
                { "parent_id", parentId.ToString() }
            };
            this.Client.Delete<Hook>(path, null, query);
        }
    }
}
