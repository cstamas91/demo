using System.Text.Json.Serialization;

namespace Config.Common
{
    public class Configuration
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
