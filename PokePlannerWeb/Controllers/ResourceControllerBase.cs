using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PokePlannerWeb.Controllers
{
    /// <summary>
    /// Base controller for getting resources.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ResourceControllerBase : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ResourceControllerBase> Logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceControllerBase(ILogger<ResourceControllerBase> logger)
        {
            Logger = logger;
        }
    }
}
