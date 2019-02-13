using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Users : BaseApi
    {
        public enum Roles {BLOCKED = -1, USER = 3, SUPERUSER = 4}; 

        public Users(IClient client) : base(client)
        {
        }

        public User[] List(bool form = false, int limit = 0, int offset = 0)
        {
            string path = "/users";
            JsonArgs data = new JsonArgs { };
            if (form)
            {
                data.Add("form", "true");
            }
            if (limit > 0) {
                data.Add("limit", limit.ToString());
            }
            if (offset > 0)
            {
                data.Add("offset", offset.ToString());
            }
            return this.Client.Get<User[]>(path, data);
        }

        public User Get(string userId)
        {
            string path = "/users/" + userId;
            return this.Client.Get<User>(path);
        }

        public User Get(int userId)
        {
            return this.Get(userId.ToString());
        }

        public void Create(Dictionary<string, string> attributes, string userId = null, bool webhook = false)
        {
            string path = "/users";
            if (userId != null) {
                path += "/" + userId;
            }
            JsonArgs userData = new JsonArgs { };
            foreach (KeyValuePair<string, string> entry in attributes) {
                userData.Add(entry.Key, entry.Value);
            }
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", userData }
            };
            JsonArgs query = null;
            if (webhook) {
                query = new JsonArgs
                {
                    { "webhook", "true" }
                };
            }
            this.Client.Post<User>(path, data, query);
        }

        public User Update(int userId, Dictionary<string, string> attributes, bool webhook = false)
        {
            string path = "/users/" + userId;
            JsonArgs query = null;
            JsonArgs userData = new JsonArgs { };
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                userData.Add(entry.Key, entry.Value);
            }
            NestedJsonArgs data = new NestedJsonArgs
            {
                { "user", userData }
            };
            if (webhook)
            {
                query = new JsonArgs
                {
                    { "webhook", "true" }
                };
            }
            return this.Client.Put<User>(path, data, query);
        }

        public void Delete(int userId)
        {
            string path = "/users/" + userId;
            this.Client.Delete<User>(path);
        }
    }
}
