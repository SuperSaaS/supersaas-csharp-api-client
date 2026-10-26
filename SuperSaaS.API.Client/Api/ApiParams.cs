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
