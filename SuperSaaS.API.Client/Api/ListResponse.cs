using System;
using Newtonsoft.Json;

namespace SuperSaaS.API.Api
{
    /// <summary>
    /// A list response that the API wraps in an object, e.g. <c>{"bookings": [...]}</c> from <c>/agenda</c> or
    /// <c>{"slots": [...]}</c> from <c>/free</c>. Reads the array under <c>bookings</c> or <c>slots</c>, and also
    /// accepts a bare array.
    /// </summary>
    [JsonConverter(typeof(ListResponseConverter))]
    internal sealed class ListResponse<T>
    {
        public T[] Items { get; set; }

        /// <summary>Returns the items of <paramref name="response"/>; <c>null</c> when there is no response.</summary>
        public static T[] ItemsOf(ListResponse<T> response)
        {
            return response?.Items;
        }
    }

    internal sealed class ListResponseConverter : JsonConverter
    {
        private static readonly string[] ListKeys = { "bookings", "slots" };

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ListResponse<>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            Type itemType = objectType.GetGenericArguments()[0];
            Type arrayType = itemType.MakeArrayType();
            object items = null;

            if (reader.TokenType == JsonToken.StartArray)
            {
                items = serializer.Deserialize(reader, arrayType);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
                {
                    var name = (string)reader.Value;
                    reader.Read();
                    if (items == null && Array.IndexOf(ListKeys, name) >= 0 && reader.TokenType == JsonToken.StartArray)
                    {
                        items = serializer.Deserialize(reader, arrayType);
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
                items = items ?? Array.CreateInstance(itemType, 0);
            }
            else
            {
                throw new JsonSerializationException("Expected a JSON object or array for " + arrayType.Name +
                                                     " but got " + reader.TokenType + ".");
            }

            object response = Activator.CreateInstance(objectType);
            objectType.GetProperty(nameof(ListResponse<object>.Items)).SetValue(response, items);
            return response;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
