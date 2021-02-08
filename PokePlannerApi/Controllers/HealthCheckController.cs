using Microsoft.AspNetCore.Mvc;

namespace PokePlannerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string HealthCheck()
        {
            return "Service is healthy!";
        }
    }
}
