using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Species resource endpoints.
    /// </summary>
    public class SpeciesController : ResourceControllerBase
    {
        private readonly PokemonSpeciesService _pokemonSpeciesService;

        public SpeciesController(
            PokemonSpeciesService pokemonSpeciesService,
            ILogger<SpeciesController> logger) : base(logger)
        {
            _pokemonSpeciesService = pokemonSpeciesService;
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
                    _logger.LogInformation($"Getting first {limit.Value} Pokemon species starting at {offset.Value}...");
                    return await _pokemonSpeciesService.GetPokemonSpecies(limit.Value, offset.Value);
                }

                _logger.LogInformation($"Getting first {limit.Value} Pokemon species...");
                return await _pokemonSpeciesService.GetPokemonSpecies(limit.Value, 0);
            }

            _logger.LogInformation("Getting all Pokemon species...");
            return await _pokemonSpeciesService.GetPokemonSpecies();
        }

        /// <summary>
        /// Returns the varieties of the Pokemon species with the given ID in the version group with
        /// the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}/varieties/{versionGroupId:int}")]
        public async Task<PokemonEntry[]> GetPokemonSpeciesVarieties(int speciesId, int versionGroupId)
        {
            _logger.LogInformation($"Getting varieties of Pokemon species {speciesId} in version group {versionGroupId}...");
            return await _pokemonSpeciesService.GetPokemonSpeciesVarieties(speciesId, versionGroupId);
        }

        /// <summary>
        /// Returns the forms of each variety of the Pokemon species with the given ID in the
        /// version group with the given ID.
        /// </summary>
        [HttpGet("{speciesId:int}/forms/{versionGroupId:int}")]
        public async Task<IEnumerable<WithId<List<PokemonFormEntry>>>> GetPokemonSpeciesForms(int speciesId, int versionGroupId)
        {
            _logger.LogInformation($"Getting forms of varieties of Pokemon species {speciesId} in version group {versionGroupId}...");
            return await _pokemonSpeciesService.GetPokemonSpeciesForms(speciesId, versionGroupId);
        }
    }
}
