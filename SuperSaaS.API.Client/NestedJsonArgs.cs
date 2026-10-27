using System.Collections.Generic;

namespace SuperSaaS.API
{
    /// <summary>
    /// A JSON request body with one level of nesting, e.g. <c>{ "user": { "name": "..." } }</c>.
    /// </summary>
    public class NestedJsonArgs : Dictionary<string, JsonArgs>
    {
    }
}
