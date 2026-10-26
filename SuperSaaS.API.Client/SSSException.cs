using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SuperSaaS.API.Models;

namespace SuperSaaS.API
{
    public class SSSException : Exception
    {
        // HTTP status of the failed response, null when the failure was not an HTTP error response.
        public int? StatusCode { get; private set; }

        // The server's "errors" array; empty when the body had none.
        public IList<ApiError> Errors { get; private set; }

        public SSSException()
        {
            this.Errors = new List<ApiError>();
        }

        public SSSException(string message)
        : base(message)
        {
            this.Errors = new List<ApiError>();
        }

        public SSSException(string message, Exception inner)
        : base(message, inner)
        {
            this.Errors = new List<ApiError>();
        }

        public SSSException(string message, int statusCode, IList<ApiError> errors)
        : base(message)
        {
            this.StatusCode = statusCode;
            this.Errors = errors ?? new List<ApiError>();
        }

        // Parses {"errors":[{"status":"404","title":"Not found"}]} (or {"errors":["message"]}).
        // Never throws; returns an empty list for empty, non-JSON or unexpected bodies.
        public static IList<ApiError> ParseErrors(string body)
        {
            List<ApiError> errors = new List<ApiError>();
            if (string.IsNullOrEmpty(body))
            {
                return errors;
            }
            try
            {
                JObject root = JToken.Parse(body) as JObject;
                if (root == null)
                {
                    return errors;
                }
                JArray items = root["errors"] as JArray;
                if (items == null)
                {
                    return errors;
                }
                foreach (JToken item in items)
                {
                    JObject obj = item as JObject;
                    if (obj != null)
                    {
                        errors.Add(new ApiError {
                            Status = TokenString(obj["status"]),
                            Title = TokenString(obj["title"]),
                            Code = TokenString(obj["code"])
                        });
                    }
                    else if (item.Type == JTokenType.String)
                    {
                        errors.Add(new ApiError { Title = (string)item });
                    }
                }
            }
            catch (Exception)
            {
                errors.Clear();
            }
            return errors;
        }

        private static string TokenString(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                return null;
            }
            return token.ToString();
        }
    }
}
