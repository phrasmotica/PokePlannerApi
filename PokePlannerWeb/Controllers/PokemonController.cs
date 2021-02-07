using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;

namespace PokePlannerWeb.Controllers
{
    /// <summary>
    /// Controller for calling Pokemon resource endpoints.
    /// </summary>
    public class PokemonController : ResourceControllerBase
    {
        /// <summary>
        /// The Pokemon service.
        /// </summary>
        private readonly PokemonService PokemonService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonController(PokemonService pokemonService, ILogger<PokemonController> logger) : base(logger)
        {
            PokemonService = pokemonService;
        }

        /// <summary>
        /// Returns the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}")]
        public async Task<PokemonEntry> GetPokemonById(int pokemonId)
        {
            Logger.LogInformation($"Getting Pokemon {pokemonId}...");
            return await PokemonService.GetPokemon(pokemonId);
        }

        /// <summary>
        /// Returns the forms of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/forms/{versionGroupId:int}")]
        public async Task<PokemonFormEntry[]> GetPokemonFormsById(int pokemonId, int versionGroupId)
        {
            Logger.LogInformation($"Getting forms of Pokemon {pokemonId} in version group {versionGroupId}...");
            return await PokemonService.GetPokemonForms(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the moves of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/moves/{versionGroupId:int}")]
        public async Task<PokemonMoveContext[]> GetPokemonMovesById(int pokemonId, int versionGroupId)
        {
            Logger.LogInformation($"Getting moves of Pokemon {pokemonId} in version group {versionGroupId}...");
            return await PokemonService.GetPokemonMoves(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the abilities of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/abilities")]
        public async Task<PokemonAbilityContext[]> GetPokemonAbilitiesById(int pokemonId)
        {
            Logger.LogInformation($"Getting abilities of Pokemon {pokemonId}...");
            return await PokemonService.GetPokemonAbilities(pokemonId);
        }
    }
}
