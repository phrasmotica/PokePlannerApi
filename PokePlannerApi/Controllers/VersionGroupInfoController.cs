using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about version groups.
    /// </summary>
    public class VersionGroupInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public VersionGroupInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<VersionGroupInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the version groups in the language with the given ID.
        /// </summary>
        [HttpGet("{languageId:int}")]
        public async Task<List<VersionGroupInfo>> GetVersionGroupInfo(int languageId)
        {
            _logger.LogInformation($"Getting info for version groups in language {languageId}...");
            return await _graphQlClient.GetVersionGroupInfo(languageId);
        }
    }
}
