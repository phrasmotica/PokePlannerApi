using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting version groups.
    /// </summary>
    public class GenerationController : ResourceControllerBase
    {
        private readonly GenerationService _generationService;

        public GenerationController(
            GenerationService generationService,
            ILogger<GenerationController> logger) : base(logger)
        {
            _generationService = generationService;
        }

        /// <summary>
        /// Returns all generations.
        /// </summary>
        [HttpGet]
        public async Task<GenerationEntry[]> GetGenerations()
        {
            _logger.LogInformation("Getting generations...");
            return await _generationService.GetAll();
        }
    }
}
