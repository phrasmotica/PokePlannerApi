using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting LocationAreaEncounter resources.
    /// </summary>
    public class EncounterController : ResourceControllerBase
    {
        private readonly EncountersService _encountersService;

        public EncounterController(
            EncountersService encountersService,
            ILogger<EncounterController> logger) : base(logger)
        {
            _encountersService = encountersService;
        }

        /// <summary>
        /// Returns the locations where the Pokemon with the given ID can be found in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}")]
        public async Task<EncountersEntry> GetCaptureLocations(int pokemonId)
        {
            _logger.LogInformation($"Getting capture locations for Pokemon {pokemonId}...");
            return await _encountersService.GetEncounters(pokemonId);
        }
    }
}
