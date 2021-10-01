using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing info about the moves a Pokemon learns.
    /// </summary>
    public class MovesInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public MovesInfoController(
            PokeAPIGraphQLClient graphQlClient,
            ILogger<MovesInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQlClient;
        }

        /// <summary>
        /// Returns info for moves learned by the Pokemon with the given ID in
        /// the version group with the given ID in the language with the given
        /// ID.
        /// </summary>
        [HttpGet("pokemon/{pokemonId:int}/versionGroup/{versionGroupId:int}/language/{languageId:int}")]
        public async Task<List<PokemonMoveInfo>> GetMovesInfo(int languageId, int pokemonId, int versionGroupId)
        {
            _logger.LogInformation($"Getting info for moves learned by Pokemon {pokemonId} in version group {versionGroupId} in language {languageId}...");
            return await _graphQlClient.GetPokemonMovesInfoByVersionGroup(languageId, pokemonId, versionGroupId);
        }
    }
}
