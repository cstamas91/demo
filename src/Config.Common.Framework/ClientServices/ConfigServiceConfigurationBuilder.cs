using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Xml;

namespace Config.Common.Framework.ClientServices
{
    public class ConfigServiceConfigurationBuilder : ConfigurationBuilder
    {
        private readonly Dictionary<string, string> _keyValues = new Dictionary<string, string>();

        public override void Initialize(string name, NameValueCollection config)
        {
            string configClientBaseAddress = config["baseAddress"];
            if (string.IsNullOrWhiteSpace(configClientBaseAddress))
                throw new ConfigurationException();
            
            using var handler = new HttpClientHandler {PreAuthenticate = true};
            using var configurationClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(configClientBaseAddress)
            };
            
            var response = configurationClient.GetAsync($"api/configs/{Guid.NewGuid()}")
                .GetAwaiter()
                .GetResult();
            
            if (!response.IsSuccessStatusCode)
                throw new ConfigurationException();
            
            string contentString = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            var configurationArray = JsonSerializer.Deserialize<Configuration[]>(contentString);
            foreach (var configuration in configurationArray)
            {
                _keyValues.Add(configuration.Key, configuration.Value);
            }
        }

        public override XmlNode ProcessRawXml(XmlNode rawXml)
        {
            var doc = rawXml.OwnerDocument;
            if (doc == null)
                return rawXml;
            
            foreach (var pair in _keyValues)
            {
                var node = doc.CreateElement("add");
                node.SetAttribute("key", pair.Key);
                node.SetAttribute("value", pair.Value);
                rawXml.AppendChild(node);
            }

            return rawXml;
        }
    }
}
