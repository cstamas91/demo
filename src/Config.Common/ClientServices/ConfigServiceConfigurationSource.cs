using System;
using Microsoft.Extensions.Configuration;

namespace Config.Common.ClientServices
{
    public class ConfigServiceConfigurationSource : IConfigurationSource
    {
        private readonly string _baseAddress;
        public ConfigServiceConfigurationSource(string baseAddress)
        {
            _baseAddress = baseAddress;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigServiceConfigurationProvider(new Uri(_baseAddress));
        }
    }
}
