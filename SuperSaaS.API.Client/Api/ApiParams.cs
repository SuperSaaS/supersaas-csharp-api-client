using System;
using System.Collections.Generic;
using System.Globalization;

namespace SuperSaaS.API.Api
{
    /// <summary>
    /// Formats request parameters the way the SuperSaaS API expects them, independent of the current culture.
    /// </summary>
    internal static class ApiParams
    {
        internal const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        internal static string Format(DateTime value)
        {
            return value.ToString(DateTimeFormat, CultureInfo.InvariantCulture);
        }

        internal static string Format(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Returns <paramref name="value"/>, or throws when it is <c>null</c> or empty.</summary>
        /// <param name="value">The required value.</param>
        /// <param name="paramName">The name of the parameter, used in the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is <c>null</c> or empty.</exception>
        internal static string Required(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Required parameter is missing.", paramName);
            }
            return value;
        }

        internal static JsonArgs ScheduleQuery(int scheduleId)
        {
            return new JsonArgs
            {
                { "schedule_id", Format(scheduleId) }
            };
        }

        /// <summary>Copies <paramref name="attributes"/> into request arguments.</summary>
        /// <param name="attributes">The attributes to send.</param>
        /// <param name="skipNullValues">Leave out <c>null</c> values (meaning "leave unchanged").</param>
        internal static JsonArgs ToJsonArgs(Dictionary<string, string> attributes, bool skipNullValues = false)
        {
            var args = new JsonArgs();
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                if (skipNullValues && entry.Value == null)
                {
                    continue;
                }
                args.Add(entry.Key, entry.Value);
            }
            return args;
        }
    }
}
