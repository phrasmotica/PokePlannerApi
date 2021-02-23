using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Pokemon resource endpoints.
    /// </summary>
    public class PokemonController : ResourceControllerBase
    {
        private readonly EfficacyService _efficacyService;
        private readonly PokemonService _pokemonService;

        public PokemonController(
            EfficacyService efficacyService,
            PokemonService pokemonService,
            ILogger<PokemonController> logger) : base(logger)
        {
            _efficacyService = efficacyService;
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Returns the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}")]
        public async Task<PokemonEntry> Get(int pokemonId)
        {
            return await _pokemonService.Get(pokemonId);
        }

        /// <summary>
        /// Returns the abilities of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/abilities")]
        public async Task<PokemonAbilityContext[]> GetPokemonAbilities(int pokemonId)
        {
            return await _pokemonService.GetPokemonAbilities(pokemonId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/efficacy/{versionGroupId:int}")]
        public async Task<EfficacySet> GetPokemonEfficacy(int pokemonId, int versionGroupId)
        {
            return await _efficacyService.GetEfficacySetByPokemonId(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the forms of the Pokemon with the given ID in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/forms/{versionGroupId:int}")]
        public async Task<List<PokemonFormEntry>> GetPokemonForms(int pokemonId, int versionGroupId)
        {
            return await _pokemonService.GetPokemonForms(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the moves of the Pokemon with the given ID in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/moves/{versionGroupId:int}")]
        public async Task<PokemonMoveContext[]> GetPokemonMoves(int pokemonId, int versionGroupId)
        {
            return await _pokemonService.GetPokemonMoves(pokemonId, versionGroupId);
        }
    }
}
