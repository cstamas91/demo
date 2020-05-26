using Microsoft.Extensions.Configuration;

namespace Config.Common.ClientServices
{
    public static class ConfigExtensions
    {
        public static IConfigurationBuilder AddConfigService(
            this IConfigurationBuilder configuration,
            string configServiceBaseAddress)
        {
            configuration.Add(new ConfigServiceConfigurationSource(configServiceBaseAddress));
            return configuration;
        }
    }
}
