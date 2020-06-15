using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Config.Client.Controllers
{
    static class ConfigKeys
    {
        public static string DbName = "DbName";
    }
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {

        private readonly ILogger<MessagesController> _logger;
        private readonly IConfiguration _configuration;

        public MessagesController(ILogger<MessagesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            yield return new WeatherForecast {Message = _configuration.GetConnectionString("Default")};
        }
    }
}
