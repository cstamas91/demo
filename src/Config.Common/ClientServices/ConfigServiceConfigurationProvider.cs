using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Config.Common.ClientServices
{
    public class ConfigServiceConfigurationProvider : ConfigurationProvider
    {
        private readonly Uri _baseAddress;
        public ConfigServiceConfigurationProvider(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }
        
        public override void Load()
        {
            using var handler = new HttpClientHandler() { PreAuthenticate = true};
            using var configurationClient = new HttpClient(handler) { BaseAddress = _baseAddress };
            
            var response = configurationClient.GetAsync($"api/configs/{Guid.NewGuid()}")
                .GetAwaiter()
                .GetResult();

            if (!response.IsSuccessStatusCode)
                throw new ConfigurationException();

            string contentString = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            
            Console.WriteLine(contentString);
            var configurationArray = JsonSerializer.Deserialize<Configuration[]>(contentString);
            foreach (var configuration in configurationArray)
            {
                Data.Add(configuration.Key, configuration.Value);
            }
        }
    }
}
