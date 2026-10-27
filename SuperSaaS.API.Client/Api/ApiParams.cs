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

        internal static JsonArgs ToJsonArgs(Dictionary<string, string> attributes)
        {
            var args = new JsonArgs();
            foreach (KeyValuePair<string, string> entry in attributes)
            {
                args.Add(entry.Key, entry.Value);
            }
            return args;
        }
    }
}
