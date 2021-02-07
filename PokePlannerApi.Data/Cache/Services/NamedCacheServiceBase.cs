using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;
using PokePlannerApi.Data.Extensions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing a collection of named PokeAPI resources in the cache.
    /// </summary>
    public class NamedCacheServiceBase<TResource> : CacheServiceBase<TResource> where TResource : NamedApiResource, new()
    {
        /// <summary>
        /// The cache source.
        /// </summary>
        protected new INamedCacheSource<TResource> CacheSource => base.CacheSource as INamedCacheSource<TResource>;

        /// <summary>
        /// Connect to cache and initialise logger.
        /// </summary>
        public NamedCacheServiceBase(
            INamedCacheSource<TResource> cacheSource,
            IPokeAPI pokeApi,
            ILogger<NamedCacheServiceBase<TResource>> logger) : base(cacheSource, pokeApi, logger)
        {
        }

        #region Public methods

        /// <summary>
        /// Returns the resource with the given ID, creating a cache entry if it doesn't exist.
        /// </summary>
        public override async Task<TResource> Upsert(UrlNavigation<TResource> res)
        {
            if (res == null)
            {
                return null;
            }

            var namedRes = res as NamedApiResource<TResource>;

            var name = namedRes.Name;
            var entry = await CacheSource.GetCacheEntry(name);
            if (entry == null)
            {
                var entryType = typeof(TResource).Name;
                Logger.LogInformation($"Caching {entryType} with name {name}...");

                var resource = await PokeApi.Get(res);
                return await Create(resource);
            }
            else if (IsStale(entry))
            {
                // update cache entry if it's stale
                var entryType = typeof(TResource).Name;
                Logger.LogInformation($"Cached {entryType} with name {name} is stale - updating...");

                var resource = await PokeApi.Get(res);
                await Update(resource);
                return resource;
            }

            return entry.Resource;
        }

        /// <summary>
        /// Upserts all entries into the cache.
        /// </summary>
        public async Task<IEnumerable<TResource>> UpsertAll()
        {
            var resourcesPage = await PokeApi.GetNamedPage<TResource>();

            var allResources = await GetAllResources();
            if (!allResources.Any() || allResources.ToList().Count != resourcesPage.Count)
            {
                const int pageSize = 20;

                var entryList = new List<TResource>();

                var pagesUsed = 0;
                NamedApiResourceList<TResource> page;
                do
                {
                    page = await PokeApi.GetNamedPage<TResource>(pageSize, pageSize * pagesUsed++);
                    var entries = await UpsertMany(page.Results);
                    entryList.AddRange(entries);
                } while (!string.IsNullOrEmpty(page.Next));

                return entryList;
            }

            // if we have the right number of entries then we're probably good
            return allResources;
        }

        /// <summary>
        /// Returns a minimal copy of the given resource, caching the resource if needed.
        /// </summary>
        public override async Task<TResource> GetMinimal(UrlNavigation<TResource> res)
        {
            var resource = await Upsert(res);
            return resource?.MinimiseNamed();
        }

        #endregion
    }
}
