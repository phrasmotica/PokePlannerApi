using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Base controller for getting resources.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ResourceControllerBase : ControllerBase
    {
        protected readonly ILogger<ResourceControllerBase> _logger;

        public ResourceControllerBase(ILogger<ResourceControllerBase> logger)
        {
            _logger = logger;
        }
    }
}
