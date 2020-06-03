using System;
using System.Net.Http;
using System.Text.Json;
using System.Data.Common;
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
            using var handler = new HttpClientHandler() { PreAuthenticate = true };
            using var configurationClient = new HttpClient(handler) { BaseAddress = _baseAddress };
            
            LoadAppSettings(configurationClient);
            LoadConnectionStrings(configurationClient);
        }

        private void LoadConnectionStrings(HttpClient configurationClient)
        {
            var response = configurationClient.GetAsync($"api/connectionstrings/{Guid.NewGuid()}")
                .GetAwaiter()
                .GetResult();

            if (!response.IsSuccessStatusCode)
                throw new ConfigurationException();

            string contentString = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            
            Console.WriteLine(contentString);
            var configurationArray = JsonSerializer.Deserialize<ConnectionString[]>(contentString);
            var connectionStringBuilder = new DbConnectionStringBuilder();
            connectionStringBuilder["MultipleActiveResultSets"] = "true";
            connectionStringBuilder["IntegratedSecurity"] = "true";

            foreach (var connectionString in configurationArray)
            {
                connectionStringBuilder["InitialCatalog"] = connectionString.InitialCatalog;
                connectionStringBuilder["DataSource"] = connectionString.DataSource;

                Data.Add($"ConnectionStrings:{connectionString.Name}", connectionStringBuilder.ConnectionString);
            }
        }

        private void LoadAppSettings(HttpClient configurationClient)
        {
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
