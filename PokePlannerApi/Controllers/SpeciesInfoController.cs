using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about Pokemon species.
    /// </summary>
    public class SpeciesInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public SpeciesInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<SpeciesInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the Pokemon species in the generation with the
        /// given ID in the language with the given ID.
        /// </summary>
        [HttpGet("pokedex/{pokedexId:int}/versionGroup/{versionGroupId:int}/language/{languageId:int}")]
        public async Task<List<PokemonSpeciesInfo>> GetSpeciesInfoByPokedex(int pokedexId, int versionGroupId, int languageId)
        {
            _logger.LogInformation($"Getting info for Pokemon species in pokedex {languageId} and version group {versionGroupId} in language {languageId}...");
            return await _graphQlClient.GetSpeciesInfoByPokedex(languageId, pokedexId, versionGroupId);
        }
    }
}
