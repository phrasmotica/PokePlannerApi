using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing basic info about locations.
    /// </summary>
    public class LocationInfoController : ResourceControllerBase
    {
        private readonly PokeAPIGraphQLClient _graphQlClient;

        public LocationInfoController(
            PokeAPIGraphQLClient graphQLClient,
            ILogger<LocationInfoController> logger) : base(logger)
        {
            _graphQlClient = graphQLClient;
        }

        /// <summary>
        /// Returns info for the locations in the region with the given ID in
        /// the language with the given ID.
        /// </summary>
        [HttpGet("region/{regionId:int}/language/{languageId:int}")]
        public async Task<List<LocationInfo>> GetLocationInfo(int languageId, int regionId)
        {
            _logger.LogInformation($"Getting info for locations in region {regionId} in language {languageId}...");
            return await _graphQlClient.GetLocationsByRegion(languageId, regionId);
        }
    }
}
