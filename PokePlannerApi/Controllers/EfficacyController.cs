using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting Pokemon type efficacies.
    /// </summary>
    public class EfficacyController : ResourceControllerBase
    {
        private readonly EfficacyService _efficacyService;

        public EfficacyController(
            EfficacyService efficacyService,
            ILogger<EfficacyController> logger) : base(logger)
        {
            _efficacyService = efficacyService;
        }

        /// <summary>
        /// Returns the efficacy of the types with the given IDs in the version group with the given ID.
        /// </summary>
        [HttpGet]
        public async Task<EfficacySet> GetEfficacyInVersionGroupByTypeIds(int versionGroupId, int type1, int type2)
        {
            _logger.LogInformation($"Getting efficacy for types {type1}/{type2} in version group {versionGroupId}...");
            return await _efficacyService.GetTypeEfficacyByTypeIds(new[] { type1, type2 }, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/{versionGroupId:int}")]
        public async Task<EfficacySet> GetEfficacyInVersionGroupById(int pokemonId, int versionGroupId)
        {
            _logger.LogInformation($"Getting efficacy for Pokemon {pokemonId}...");
            return await _efficacyService.GetTypeEfficacyByPokemonId(pokemonId, versionGroupId);
        }
    }
}
