using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;

namespace PokePlannerApi.Data
{
    /// <summary>
    /// Client for accessing resources from PokeAPI.
    /// </summary>
    public class ApiResourceClient
    {
        private readonly IPokeAPI _pokeApi;
        private readonly ILogger<ApiResourceClient> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiResourceClient(
            IPokeAPI pokeApi,
            ILogger<ApiResourceClient> logger)
        {
            _pokeApi = pokeApi;
            _logger = logger;
        }

        /// <summary>
        /// Returns the resource of the given type with the given ID.
        /// </summary>
        protected async Task<TResource> Get<TResource>(int id) where TResource : ResourceBase
        {
            var typeName = typeof(TResource).Name;
            _logger.LogInformation($"Fetching {typeName} resource with ID {id}...");
            return await _pokeApi.Get<TResource>(id);
        }

        /// <summary>
        /// Returns the resources of the given type with the given IDs.
        /// </summary>
        protected async Task<IEnumerable<TResource>> GetMany<TResource>(IEnumerable<int> ids) where TResource : ResourceBase
        {
            var resources = new List<TResource>();

            foreach (var id in ids)
            {
                resources.Add(await Get<TResource>(id));
            }

            return resources;
        }
    }
}
