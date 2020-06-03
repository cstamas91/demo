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

    public class ConnectionString
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("initialCatalog")]
        public string InitialCatalog { get; set; }
        [JsonPropertyName("dataSource")]
        public string DataSource { get; set; }
    }
}
