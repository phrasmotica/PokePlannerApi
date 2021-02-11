﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;

namespace PokePlannerApi.Data
{
    /// <summary>
    /// Client for accessing named resources from PokeAPI.
    /// </summary>
    public class NamedApiResourceClient
    {
        private readonly IPokeAPI _pokeApi;
        private readonly ILogger<NamedApiResourceClient> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NamedApiResourceClient(
            IPokeAPI pokeApi,
            ILogger<NamedApiResourceClient> logger)
        {
            _pokeApi = pokeApi;
            _logger = logger;
        }

        /// <summary>
        /// Returns the resource of the given type with the given ID.
        /// </summary>
        protected async Task<TResource> Get<TResource>(int id) where TResource : NamedApiResource
        {
            var typeName = typeof(TResource).Name;
            _logger.LogInformation($"Fetching {typeName} resource with ID {id}...");
            return await _pokeApi.Get<TResource>(id);
        }

        /// <summary>
        /// Returns the resource of the given type with the given name.
        /// </summary>
        protected async Task<TResource> GetByName<TResource>(string name) where TResource : NamedApiResource
        {
            var typeName = typeof(TResource).Name;
            _logger.LogInformation($"Fetching {typeName} resource with name {name}...");
            return await _pokeApi.Get<TResource>(name);
        }

        /// <summary>
        /// Returns the resources of the given type with the given IDs.
        /// </summary>
        protected async Task<IEnumerable<TResource>> GetMany<TResource>(IEnumerable<int> ids) where TResource : NamedApiResource
        {
            var resources = new List<TResource>();

            foreach (var id in ids)
            {
                resources.Add(await Get<TResource>(id));
            }

            return resources;
        }

        /// <summary>
        /// Returns the resources of the given type with the given names.
        /// </summary>
        protected async Task<IEnumerable<TResource>> GetManyByName<TResource>(IEnumerable<string> names) where TResource : NamedApiResource
        {
            var resources = new List<TResource>();

            foreach (var name in names)
            {
                resources.Add(await GetByName<TResource>(name));
            }

            return resources;
        }
    }
}