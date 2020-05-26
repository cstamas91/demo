using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config.Common;

namespace Config.Service.Services
{
    public class ConfigurationService
    {
        public Task<IEnumerable<Configuration>> GetConfigs(Guid id)
        {
            return Task.FromResult<IEnumerable<Configuration>>(new []
            {
                new Configuration
                {
                    Key = "DbName", Value = "MyDb"
                },
                new Configuration
                {
                    Key = "DbServerName", Value = "ProdServer"
                },
                new Configuration
                {
                    Key = "LogTarget", Value = "EventLog"
                },
                new Configuration
                {
                    Key = "LogName", Value = "ConfigClient"
                }
            });
        }
    }
}
