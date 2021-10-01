using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about types.
    /// </summary>
    public class TypeInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public TypeInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<TypeInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the types in the language with the given ID.
        /// </summary>
        [HttpGet("{languageId:int}")]
        public async Task<List<TypeInfo>> GetTypeInfo(int languageId)
        {
            _logger.LogInformation($"Getting info for types in language {languageId}...");
            return await _graphQlClient.GetTypeInfo(languageId);
        }
    }
}
