#if HAS_SYSTEM_JSON
using System.Text.Json.Serialization;
using Json = System.Text.Json.Serialization.JsonSerializer;
#else
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
#endif

namespace Sentry.Protocol.Tests
{
    internal class JsonSerializer
    {
#if HAS_SYSTEM_JSON

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
        };

        public static string SerializeObject<T>(T @object) => Json.ToString(@object, Options);
        public static dynamic DeserializeObject(string json) => Json.Parse<dynamic>(json);
#else
        private static readonly StringEnumConverter StringEnumConverter = new StringEnumConverter();

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new[] {StringEnumConverter}
        };

        public static string SerializeObject<T>(T @object) => JsonConvert.SerializeObject(@object, Settings);
        public static dynamic DeserializeObject(string json) => JsonConvert.DeserializeObject(json);
#endif
    }
}
