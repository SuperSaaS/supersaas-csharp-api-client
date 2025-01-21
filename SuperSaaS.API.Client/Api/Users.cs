using System;
using System.Collections.Generic;
using SuperSaaS.API.Models;

#nullable enable

namespace SuperSaaS.API.Api
{
    /// <summary>
    /// REF: https://www.supersaas.com/info/dev/user_api
    /// </summary>
    public class Users : BaseApi
    {
        public Users(IClient client) : base(client)
        {
        }

        public User[] List(bool? form = null, int? limit = null, int? offset = null)
        {
            string path = GetUserPath(null);
            var data = new JsonArgs();

            if (form.HasValue && form.Value)
            {
                data.Add("form", "true");
            }
            if (limit.HasValue)
            {
                data.Add("limit", ValidateNumber(limit.Value).ToString());
            }
            if (offset.HasValue)
            {
                data.Add("offset", ValidateNumber(offset.Value).ToString());
            }

            return this.Client.Get<User[]>(path, data) ?? Array.Empty<User>();
        }

        public User Get(object userId, bool? form = null)
        {
            string path = GetUserPath(userId);
            var data = new JsonArgs();
            if (form == true)
            {
                data.Add("form", "true");
            }

            var result = this.Client.Get<User>(path, data);
            return result ?? new User();
        }

        public object Create(
            Dictionary<string, object> attributes,
            object? userId = null,
            bool? webhook = null,
            string? duplicate = null)
        {
            string path = GetUserPath(userId);
            var query = new JsonArgs();

            if (webhook == true) query.Add("webhook", "true");
            if (!string.IsNullOrEmpty(duplicate)) query.Add("duplicate", ValidateDuplicate(duplicate));

            var userData = new JsonArgs();

            if (attributes.ContainsKey("name")) userData.Add("name", ValidateName(attributes["name"]?.ToString() ?? ""));
            if (attributes.ContainsKey("email")) userData.Add("email", attributes["email"]?.ToString() ?? "");
            if (attributes.ContainsKey("password")) userData.Add("password", attributes["password"]?.ToString() ?? "");
            if (attributes.TryGetValue("full_name", out var fullName)) 
                userData.Add("full_name", fullName?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("address", out var address)) 
                userData.Add("address", address?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("mobile", out var mobile)) 
                userData.Add("mobile", mobile?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("phone", out var phone)) 
                userData.Add("phone", phone?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("country", out var country)) 
                userData.Add("country", country?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("timezone", out var timezone)) 
                userData.Add("timezone", timezone?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("field_1", out var field1)) 
                userData.Add("field_1", field1?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("field_2", out var field2)) 
                userData.Add("field_2", field2?.ToString() ?? string.Empty);
            if (attributes.TryGetValue("super_field", out var superField)) 
                userData.Add("super_field", superField?.ToString() ?? string.Empty);
    
            if (attributes.TryGetValue("credit", out var credit))
            {
                var creditValue = credit?.ToString();
                if (!string.IsNullOrEmpty(creditValue))
                    userData.Add("credit", ValidateNumber(int.Parse(creditValue)).ToString());
            }

            if (attributes.ContainsKey("role"))
            {
                string roleStr = attributes["role"]?.ToString() ?? "0";
                int parsedRole = int.TryParse(roleStr, out int r) ? r : 0;
                parsedRole = ValidateOptions<int>(parsedRole, User.ROLES);
                userData.Add("role", parsedRole.ToString());
            }
            if (attributes.TryGetValue("group", out var group))
            {
                var groupValue = group?.ToString();
                if (!string.IsNullOrEmpty(groupValue))
                    userData.Add("group", ValidateNumber(int.Parse(groupValue)).ToString());
            }

            var data = new NestedJsonArgs
            {
                { "user", userData }
            };

            var result = this.Client.Post<User>(path, data, query);
            return result ?? new User();
        }

        public User Update(
            object userId,
            Dictionary<string, object> attributes,
            bool? webhook = null,
            string? notfound = null)
        {
            string path = GetUserPath(userId);
            var query = new JsonArgs();

            if (webhook == true) query.Add("webhook", "true");
            if (!string.IsNullOrEmpty(notfound)) query.Add("notfound", ValidateNotfound(notfound));
            var userData = new JsonArgs();

            if (attributes.ContainsKey("name")) userData.Add("name", ValidateName(attributes["name"]?.ToString() ?? ""));

            if (attributes.ContainsKey("role"))
            {
                string roleStr = attributes["role"]?.ToString() ?? "0";
                int parsedRole = int.TryParse(roleStr, out int r) ? r : 0;
                parsedRole = ValidateOptions<int>(parsedRole, User.ROLES);
                userData.Add("role", parsedRole.ToString());
            }

            var data = new NestedJsonArgs
            {
                { "user", userData }
            };

            var result = this.Client.Put<User>(path, data, query);
            return result ?? new User();
        }

        public void Delete(object userId, bool? webhook = null)
        {
            string path = GetUserPath(userId);
            var parameters = new JsonArgs();

            if (webhook == true) parameters.Add("webhook", "true");

            this.Client.Delete<User>(path, new NestedJsonArgs(), parameters);
        }

        public FieldList[] GetFieldList()
        {
            string path = "/field_list";
            var result = this.Client.Get<FieldList[]>(path, new JsonArgs());
            return result ?? Array.Empty<FieldList>();
        }

        private string GetUserPath(object? userId)
        {
            if (userId == null || userId.ToString() == string.Empty)
            {
                return "/users";
            }
            return $"/users/{ValidateUser(userId)}";
        }
    }
}
