using System;
using System.Threading.Tasks;
using Config.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Config.Service.Controllers
{
    [Route("api/configs")]
    [ApiController]
    public class ConfigsController : Controller
    {
        private readonly ConfigurationService _service;
        public ConfigsController(ConfigurationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetConfigs(id));
        }
    }
}
