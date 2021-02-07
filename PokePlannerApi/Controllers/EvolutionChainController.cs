using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing evolution chain resources.
    /// </summary>
    public class EvolutionChainController : ResourceControllerBase
    {
        /// <summary>
        /// The evolution chain service.
        /// </summary>
        private readonly EvolutionChainService EvolutionChainService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionChainController(EvolutionChainService evolutionChainService, ILogger<EvolutionChainController> logger) : base(logger)
        {
            EvolutionChainService = evolutionChainService;
        }

        /// <summary>
        /// Returns the evolution chain for the species with the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}")]
        public async Task<EvolutionChainEntry> GetEvolutionChainBySpeciesId(int speciesId)
        {
            Logger.LogInformation($"Getting evolution chain {speciesId}...");
            return await EvolutionChainService.GetBySpeciesId(speciesId);
        }
    }
}
