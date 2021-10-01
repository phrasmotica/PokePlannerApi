using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing evolution chain resources.
    /// </summary>
    public class EvolutionChainController : ResourceControllerBase
    {
        private readonly EvolutionChainService _evolutionChainService;

        public EvolutionChainController(
            EvolutionChainService evolutionChainService,
            ILogger<EvolutionChainController> logger) : base(logger)
        {
            _evolutionChainService = evolutionChainService;
        }

        /// <summary>
        /// Returns the evolution chain for the species with the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}")]
        public async Task<EvolutionChainEntry> GetEvolutionChainBySpeciesId(int speciesId)
        {
            _logger.LogInformation($"Getting evolution chain {speciesId}...");
            return await _evolutionChainService.GetBySpeciesId(speciesId);
        }
    }
}
