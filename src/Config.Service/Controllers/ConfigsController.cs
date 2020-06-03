using System;
using System.Threading.Tasks;
using Config.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Config.Service.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ConfigsController : Controller
    {
        private readonly ConfigurationService _service;
        public ConfigsController(ConfigurationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("configs/{id}")]
        public async Task<IActionResult> Configs(Guid id)
        {
            return Ok(await _service.GetConfigs(id));
        }

        [HttpGet("connectionstrings/{id}")]
        public async Task<IActionResult> ConnectionStrings(Guid id)
        {
            return Ok(await _service.GetConnectionStrings(id));
        }
    }
}
