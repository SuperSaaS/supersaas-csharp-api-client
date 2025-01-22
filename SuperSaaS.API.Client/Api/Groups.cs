using System;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Groups : BaseApi
    {
        public Groups(IClient client) : base(client)
        {
        }

        public Group[] List()
        {
            string path = "/groups";
            return this.Client.Get<Group[]>(path, new JsonArgs());
        }
    }
}