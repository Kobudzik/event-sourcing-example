using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace EventSourcingExample.WebUI.Controllers
{
    public class InfoController : ApiControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public InfoController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("check-health")]
        public async Task<ActionResult<HealthReport>> CheckHealth()
            => await _healthCheckService.CheckHealthAsync();

        [HttpGet("version")]
        public ActionResult<string> GetVersion()
            => GetType().Assembly.GetName()?.Version.ToString();
    }
}