using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting Pokemon type efficacies.
    /// </summary>
    public class EfficacyController : ResourceControllerBase
    {
        /// <summary>
        /// The Pokemon service.
        /// </summary>
        private readonly EfficacyService EfficacyService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EfficacyController(EfficacyService efficacyService, ILogger<EfficacyController> logger) : base(logger)
        {
            EfficacyService = efficacyService;
        }

        /// <summary>
        /// Returns the efficacy of the types with the given IDs in the version group with the given ID.
        /// </summary>
        [HttpGet]
        public async Task<EfficacySet> GetEfficacyInVersionGroupByTypeIds(int versionGroupId, int type1, int type2)
        {
            Logger.LogInformation($"Getting efficacy for types {type1}/{type2} in version group {versionGroupId}...");
            return await EfficacyService.GetTypeEfficacyByTypeIds(new[] { type1, type2 }, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/{versionGroupId:int}")]
        public async Task<EfficacySet> GetEfficacyInVersionGroupById(int pokemonId, int versionGroupId)
        {
            Logger.LogInformation($"Getting efficacy for Pokemon {pokemonId}...");
            return await EfficacyService.GetTypeEfficacyByPokemonId(pokemonId, versionGroupId);
        }
    }
}
