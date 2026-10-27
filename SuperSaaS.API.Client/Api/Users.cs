using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    /// <summary>User endpoints.</summary>
    public class Users : BaseApi
    {
        /// <summary>User roles.</summary>
        public enum Roles
        {
            /// <summary>A blocked user.</summary>
            BLOCKED = -1,

            /// <summary>A regular user.</summary>
            USER = 3,

            /// <summary>A superuser.</summary>
            SUPERUSER = 4
        }

        /// <summary>Creates the user endpoints for <paramref name="client"/>.</summary>
        /// <param name="client">The client used to send requests.</param>
        public Users(IClient client) : base(client)
        {
        }

        /// <summary>Gets all users, with optional form data and pagination.</summary>
        /// <param name="form">Include the form data of each user.</param>
        /// <param name="limit">Maximum number of users to return; 0 for the API default.</param>
        /// <param name="offset">Number of users to skip.</param>
        /// <returns>The users.</returns>
        public User[] List(bool form = false, int limit = 0, int offset = 0)
        {
            var data = new JsonArgs();
            if (form)
            {
                data.Add("form", "true");
            }
            if (limit > 0)
            {
                data.Add("limit", ApiParams.Format(limit));
            }
            if (offset > 0)
            {
                data.Add("offset", ApiParams.Format(offset));
            }
            return Client.Get<User[]>("/users", data);
        }

        /// <summary>Gets a single user by id or by your own foreign key (e.g. <c>"123fk"</c>).</summary>
        /// <param name="userId">The SuperSaaS user id, or a foreign key ending in <c>fk</c>.</param>
        /// <returns>The user.</returns>
        public User Get(string userId)
        {
            return Client.Get<User>("/users/" + userId);
        }

        /// <summary>Gets a single user.</summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The user.</returns>
        public User Get(int userId)
        {
            return Get(ApiParams.Format(userId));
        }

        /// <summary>Creates a user, optionally under your own foreign key.</summary>
        /// <param name="attributes">User attributes such as <c>name</c>, <c>email</c> and <c>full_name</c>.</param>
        /// <param name="userId">Optional foreign key for the new user (e.g. <c>"123fk"</c>).</param>
        /// <param name="webhook">Trigger the webhooks configured for user creation.</param>
        public void Create(Dictionary<string, string> attributes, string userId = null, bool webhook = false)
        {
            string path = "/users";
            if (userId != null)
            {
                path += "/" + userId;
            }
            var data = new NestedJsonArgs
            {
                { "user", ApiParams.ToJsonArgs(attributes) }
            };
            Client.Post<User>(path, data, WebhookQuery(webhook));
        }

        /// <summary>Updates a user.</summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="attributes">The user attributes to change.</param>
        /// <param name="webhook">Trigger the webhooks configured for user changes.</param>
        /// <returns>The API response, if any.</returns>
        public User Update(int userId, Dictionary<string, string> attributes, bool webhook = false)
        {
            var data = new NestedJsonArgs
            {
                { "user", ApiParams.ToJsonArgs(attributes) }
            };
            return Client.Put<User>("/users/" + ApiParams.Format(userId), data, WebhookQuery(webhook));
        }

        /// <summary>Deletes a user.</summary>
        /// <param name="userId">The id of the user.</param>
        public void Delete(int userId)
        {
            Client.Delete<User>("/users/" + ApiParams.Format(userId));
        }

        private static JsonArgs WebhookQuery(bool webhook)
        {
            return webhook ? new JsonArgs { { "webhook", "true" } } : null;
        }
    }
}
