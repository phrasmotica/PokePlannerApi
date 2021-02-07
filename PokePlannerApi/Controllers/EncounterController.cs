using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting LocationAreaEncounter resources.
    /// </summary>
    public class EncounterController : ResourceControllerBase
    {
        /// <summary>
        /// The encounters service.
        /// </summary>
        private readonly EncountersService EncountersService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterController(EncountersService encountersService, ILogger<EncounterController> logger) : base(logger)
        {
            EncountersService = encountersService;
        }

        /// <summary>
        /// Returns the locations where the Pokemon with the given ID can be found in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}")]
        public async Task<EncountersEntry> GetCaptureLocations(int pokemonId)
        {
            Logger.LogInformation($"Getting capture locations for Pokemon {pokemonId}...");
            return await EncountersService.GetEncounters(pokemonId);
        }
    }
}
