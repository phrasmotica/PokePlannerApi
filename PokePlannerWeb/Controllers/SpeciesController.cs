using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;

namespace PokePlannerWeb.Controllers
{
    /// <summary>
    /// Controller for calling Species resource endpoints.
    /// </summary>
    public class SpeciesController : ResourceControllerBase
    {
        /// <summary>
        /// The Pokemon species service.
        /// </summary>
        private readonly PokemonSpeciesService PokemonSpeciesService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SpeciesController(PokemonSpeciesService pokemonSpeciesService, ILogger<SpeciesController> logger) : base(logger)
        {
            PokemonSpeciesService = pokemonSpeciesService;
        }

        /// <summary>
        /// Returns all Pokemon species, optionally up to the given limit from the given offset.
        /// </summary>
        public async Task<PokemonSpeciesEntry[]> GetPokemonSpecies(int? limit = null, int? offset = null)
        {
            if (limit.HasValue)
            {
                if (offset.HasValue)
                {
                    Logger.LogInformation($"Getting first {limit.Value} Pokemon species starting at {offset.Value}...");
                    return await PokemonSpeciesService.GetPokemonSpecies(limit.Value, offset.Value);
                }

                Logger.LogInformation($"Getting first {limit.Value} Pokemon species...");
                return await PokemonSpeciesService.GetPokemonSpecies(limit.Value, 0);
            }

            Logger.LogInformation("Getting all Pokemon species...");
            return await PokemonSpeciesService.GetPokemonSpecies();
        }

        /// <summary>
        /// Returns the varieties of the Pokemon species with the given ID in the version group with
        /// the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}/varieties/{versionGroupId:int}")]
        public async Task<PokemonEntry[]> GetPokemonSpeciesVarieties(int speciesId, int versionGroupId)
        {
            Logger.LogInformation($"Getting varieties of Pokemon species {speciesId} in version group {versionGroupId}...");
            return await PokemonSpeciesService.GetPokemonSpeciesVarieties(speciesId, versionGroupId);
        }

        /// <summary>
        /// Returns the forms of each variety of the Pokemon species with the given ID in the
        /// version group with the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}/forms/{versionGroupId:int}")]
        public async Task<IEnumerable<WithId<PokemonFormEntry[]>>> GetPokemonSpeciesForms(int speciesId, int versionGroupId)
        {
            Logger.LogInformation($"Getting forms of varieties of Pokemon species {speciesId} in version group {versionGroupId}...");
            return await PokemonSpeciesService.GetPokemonSpeciesForms(speciesId, versionGroupId);
        }
    }
}
