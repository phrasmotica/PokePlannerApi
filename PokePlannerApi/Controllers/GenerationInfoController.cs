using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about generations.
    /// </summary>
    public class GenerationInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public GenerationInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<GenerationInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the generations in the language with the given ID.
        /// </summary>
        [HttpGet("{languageId:int}")]
        public async Task<List<GenerationInfo>> GetGenerationInfo(int languageId)
        {
            _logger.LogInformation($"Getting info for generations in language {languageId}...");
            return await _graphQlClient.GetGenerationInfo(languageId);
        }
    }
}
