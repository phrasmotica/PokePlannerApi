using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;
using PokePlannerWeb.Data.Cache.Models;
using PokePlannerWeb.Data.Extensions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing a collection of PokeAPI resources in the cache.
    /// </summary>
    public class CacheServiceBase<TResource> where TResource : ResourceBase, new()
    {
        /// <summary>
        /// The cache source.
        /// </summary>
        protected ICacheSource<TResource> CacheSource;

        /// <summary>
        /// The PokeAPI data fetcher.
        /// </summary>
        protected IPokeAPI PokeApi;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<CacheServiceBase<TResource>> Logger;

        /// <summary>
        /// Connect to cache and initialise logger.
        /// </summary>
        public CacheServiceBase(
            ICacheSource<TResource> cacheSource,
            IPokeAPI pokeApi,
            ILogger<CacheServiceBase<TResource>> logger)
        {
            CacheSource = cacheSource;
            PokeApi = pokeApi;
            Logger = logger;
        }

        /// <summary>
        /// Gets the time to live for cache entries.
        /// </summary>
        protected virtual TimeSpan TimeToLive { get; } = TimeSpan.FromDays(365);

        #region CRUD methods

        /// <summary>
        /// Returns the resource with the given ID.
        /// </summary>
        protected async Task<TResource> Get(int id)
        {
            return await CacheSource.GetOne(t => t.Id == id);
        }

        /// <summary>
        /// Returns all resources.
        /// </summary>
        protected async Task<IEnumerable<TResource>> GetAllResources()
        {
            return await CacheSource.GetAll();
        }

        /// <summary>
        /// Creates a new entry and returns it.
        /// </summary>
        protected async Task<TResource> Create(TResource resource)
        {
            await CacheSource.Create(resource);
            return resource;
        }

        /// <summary>
        /// Removes the cache entry for the given resource and creates a new one.
        /// </summary>
        protected async Task Update(TResource resource)
        {
            Remove(resource.Id);
            await Create(resource);
        }

        /// <summary>
        /// Removes the cache entry for the resource with the given ID.
        /// </summary>
        protected void Remove(int id)
        {
            CacheSource.DeleteOne(t => t.Id == id);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the resource with the given ID, creating a cache entry if it doesn't exist.
        /// </summary>
        public async Task<TResource> Upsert(int id)
        {
            var entry = await CacheSource.GetCacheEntry(id);
            if (entry == null)
            {
                var entryType = typeof(TResource).Name;
                Logger.LogInformation($"Caching {entryType} with ID {id}...");

                var resource = await PokeApi.Get<TResource>(id);
                return await Create(resource);
            }
            else if (IsStale(entry))
            {
                // update cache entry if it's stale
                var entryType = typeof(TResource).Name;
                Logger.LogInformation($"Cached {entryType} with ID {id} is stale - updating...");

                var resource = await PokeApi.Get<TResource>(id);
                await Update(resource);
                return resource;
            }

            return entry.Resource;
        }

        /// <summary>
        /// Returns the resource with the given ID, creating a cache entry if it doesn't exist.
        /// </summary>
        public virtual async Task<TResource> Upsert(UrlNavigation<TResource> res)
        {
            if (res == null)
            {
                return null;
            }

            var resource = await PokeApi.Get(res);
            return await Upsert(resource.Id);
        }

        /// <summary>
        /// Creates new entries for the API resources with the given IDs and returns them.
        /// </summary>
        public virtual async Task<IEnumerable<TResource>> UpsertMany(IEnumerable<int> ids)
        {
            var entryList = new List<TResource>();

            foreach (var id in ids)
            {
                var entry = await Upsert(id);
                entryList.Add(entry);
            }

            return entryList;
        }

        /// <summary>
        /// Creates new entries for the given API resources and returns them.
        /// </summary>
        public virtual async Task<IEnumerable<TResource>> UpsertMany(IEnumerable<UrlNavigation<TResource>> resources)
        {
            var entryList = new List<TResource>();

            foreach (var res in resources)
            {
                var entry = await Upsert(res);
                entryList.Add(entry);
            }

            return entryList;
        }

        /// <summary>
        /// Returns a minimal copy of the given resource, caching the resource if needed.
        /// </summary>
        public virtual async Task<TResource> GetMinimal(UrlNavigation<TResource> res)
        {
            var resource = await Upsert(res);
            return resource?.Minimise();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns whether the cache entry is considered stale.
        /// </summary>
        protected bool IsStale(CacheEntry<TResource> entry)
        {
            return entry.CreationTime < DateTime.UtcNow - TimeToLive;
        }

        #endregion
    }
}
