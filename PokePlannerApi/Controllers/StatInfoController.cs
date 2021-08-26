using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about stats.
    /// </summary>
    public class StatInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public StatInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<StatInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the stats in the language with the given ID.
        /// </summary>
        [HttpGet("{languageId:int}")]
        public async Task<List<StatInfo>> GetStatInfo(int languageId)
        {
            _logger.LogInformation($"Getting info for version groups in language {languageId}...");
            return await _graphQlClient.GetStatInfo(languageId);
        }
    }
}
