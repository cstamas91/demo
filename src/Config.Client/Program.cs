using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Config.Common.ClientServices;

namespace Config.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, builder) =>
                    {
                        var configuration = builder.Build();
                        string baseAddress = configuration
                            .GetSection("ConfigurationService")
                            .GetValue<string>("BaseAddress");
                        
                        builder.AddConfigService(baseAddress);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
