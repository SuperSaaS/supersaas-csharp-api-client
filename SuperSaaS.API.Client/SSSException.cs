using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSaaS.API.Models;

namespace SuperSaaS.API
{
    /// <summary>
    /// Thrown when an API request fails. For HTTP errors the message is the response body returned by SuperSaaS,
    /// <see cref="StatusCode"/> holds the HTTP status and <see cref="Errors"/> the parsed <c>errors</c> array; for
    /// other failures (network, timeout) the message is that of the underlying exception, which is available as
    /// <see cref="Exception.InnerException"/>.
    /// </summary>
    public class SSSException : Exception
    {
        /// <summary>Creates an exception without a message.</summary>
        public SSSException()
        {
            Errors = new List<ApiError>();
        }

        /// <summary>Creates an exception with a message.</summary>
        /// <param name="message">The error message.</param>
        public SSSException(string message)
            : base(message)
        {
            Errors = new List<ApiError>();
        }

        /// <summary>Creates an exception with a message and the exception that caused it.</summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The exception that caused this one.</param>
        public SSSException(string message, Exception inner)
            : base(message, inner)
        {
            Errors = new List<ApiError>();
        }

        /// <summary>Creates an exception for an HTTP error response.</summary>
        /// <param name="message">The error message, normally the response body.</param>
        /// <param name="statusCode">The HTTP status code of the response.</param>
        /// <param name="errors">The errors parsed from the response, see <see cref="ParseErrors"/>.</param>
        public SSSException(string message, int statusCode, IList<ApiError> errors)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors ?? new List<ApiError>();
        }

        /// <summary>HTTP status of the failed response; <c>null</c> when the failure was not an HTTP error.</summary>
        public int? StatusCode { get; private set; }

        /// <summary>The server's <c>errors</c> array; empty when the response had none.</summary>
        public IList<ApiError> Errors { get; private set; }

        /// <summary>
        /// Parses the <c>errors</c> array of an error response, either
        /// <c>{"errors":[{"status":"404","title":"Not found"}]}</c> or <c>{"errors":["message"]}</c>.
        /// </summary>
        /// <param name="body">The response body.</param>
        /// <returns>The errors; an empty list for empty, non-JSON or unexpected bodies. Never throws.</returns>
        public static IList<ApiError> ParseErrors(string body)
        {
            var errors = new List<ApiError>();
            if (string.IsNullOrEmpty(body))
            {
                return errors;
            }
            try
            {
                if (!(JToken.Parse(body) is JObject root) || !(root["errors"] is JArray items))
                {
                    return errors;
                }
                foreach (JToken item in items)
                {
                    if (item is JObject obj)
                    {
                        errors.Add(new ApiError
                        {
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
            catch (JsonException)
            {
                errors.Clear();
            }
            return errors;
        }

        private static string TokenString(JToken token)
        {
            return token == null || token.Type == JTokenType.Null ? null : token.ToString();
        }
    }
}
