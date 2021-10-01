using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Type resource endpoints.
    /// </summary>
    public class TypeController : ResourceControllerBase
    {
        private readonly TypeService _typeService;

        public TypeController(
            TypeService typeService,
            ILogger<TypeController> logger) : base(logger)
        {
            _typeService = typeService;
        }

        /// <summary>
        /// Returns all concrete types.
        /// </summary>
        [HttpGet]
        public async Task<TypeEntry[]> GetConcreteTypes()
        {
            _logger.LogInformation($"Getting concrete types...");
            return await _typeService.GetConcrete();
        }
    }
}
