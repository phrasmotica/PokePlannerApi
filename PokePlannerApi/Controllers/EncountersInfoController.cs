using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing encounters info.
    /// </summary>
    public class EncountersInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public EncountersInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<EncountersInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for encounters in the location area with the given ID
        /// in the language with the given ID.
        /// </summary>
        [HttpGet("locationArea/{locationAreaId:int}/versionGroup/{versionGroupId:int}/language/{languageId:int}")]
        public async Task<List<EncountersInfo>> GetEncountersInfoByPokedex(int locationAreaId, int versionId, int languageId)
        {
            _logger.LogInformation($"Getting info for encounters in location area {locationAreaId} and version {versionId} in language {languageId}...");
            return await _graphQlClient.GetLocationAreaEncountersByVersion(languageId, locationAreaId, versionId);
        }
    }
}
