using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing a collection of named API resource entries in the data store.
    /// </summary>
    public abstract class NamedApiResourceServiceBase<TSource, TEntry> : NamedServiceBase<TSource, TEntry>
        where TSource : NamedApiResource, new()
        where TEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NamedApiResourceServiceBase(
            IDataStoreSource<TEntry> dataStoreSource,
            IPokeAPI pokeApi,
            NamedCacheServiceBase<TSource> cacheService,
            ILogger<NamedApiResourceServiceBase<TSource, TEntry>> logger) : base(dataStoreSource, pokeApi, cacheService, logger)
        {
            CacheService = cacheService;
        }

        #region CRUD methods

        /// <summary>
        /// Returns the entry with the given name.
        /// </summary>
        protected async Task<TEntry> GetByName(string name)
        {
            return await DataStoreSource.GetOne(e => e.Name == name);
        }

        /// <summary>
        /// Removes the entry with the given name and creates a new one.
        /// </summary>
        protected void UpdateByName(string name, TEntry entry)
        {
            RemoveByName(name);
            Create(entry);
        }

        /// <summary>
        /// Removes the entry with the given name.
        /// </summary>
        protected void RemoveByName(string name)
        {
            DataStoreSource.DeleteOne(p => p.Name == name);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new entry for the given named API resource and returns it.
        /// </summary>
        public override async Task<TEntry> Upsert(NamedApiResource<TSource> res)
        {
            if (res == null)
            {
                return null;
            }

            // check for existing entry by name
            var existingEntry = await GetByName(res.Name);
            if (existingEntry != null)
            {
                return existingEntry;
            }

            var resource = await CacheService.Upsert(res);
            return await CreateEntry(resource);
        }

        /// <summary>
        /// Creates new entries for the given named API resources and returns them.
        /// </summary>
        public override async Task<IEnumerable<TEntry>> UpsertMany(IEnumerable<NamedApiResource<TSource>> resources)
        {
            // check for existing entries by name
            var entries = await GetManyByNames(resources.Select(r => r.Name));
            var existingEntries = entries.Where(e => e != null);
            if (existingEntries.ToList().Count == resources.ToList().Count)
            {
                return existingEntries;
            }

            var entryList = new List<TEntry>();

            foreach (var res in resources)
            {
                var entry = await Upsert(res);
                entryList.Add(entry);
            }

            return entryList;
        }

        /// <summary>
        /// Creates or updates the entry with the given ID for the source object as needed.
        /// </summary>
        public override async Task<IEnumerable<TEntry>> UpsertMany(IEnumerable<TSource> sources)
        {
            var entries = new List<TEntry>();

            foreach (var source in sources)
            {
                var existingEntry = await GetByName(source.Name);
                entries.Add(existingEntry ?? await CreateEntry(source));
            }

            return entries;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the source object with the given ID.
        /// </summary>
        protected override async Task<TSource> FetchSource(int key)
        {
            var typeName = typeof(TSource).Name;
            Logger.LogInformation($"Fetching {typeName} source object with ID {key}...");
            return await CacheService.Upsert(key);
        }

        /// <summary>
        /// Returns the entries with the given names.
        /// </summary>
        protected async Task<IEnumerable<TEntry>> GetManyByNames(IEnumerable<string> names)
        {
            var entries = new List<TEntry>();

            foreach (var name in names)
            {
                entries.Add(await GetByName(name));
            }

            return entries;
        }

        /// <summary>
        /// Upserts many entries into the data store, via the cache.
        /// </summary>
        public override async Task<IEnumerable<TEntry>> UpsertMany(NamedApiResourceList<TSource> resources)
        {
            var sourceType = typeof(TSource).Name;
            var entryType = typeof(TEntry).Name;
            Logger.LogInformation($"Upserting {resources.Results.Count} {entryType} entries for {sourceType} in data store...");

            return await UpsertMany(resources.Results);
        }

        /// <summary>
        /// Creates or updates the entry for the given source object as needed.
        /// </summary>
        protected override async Task<TEntry> Upsert(TSource source)
        {
            var existingEntry = await GetByName(source.Name);
            return existingEntry ?? await CreateEntry(source);
        }

        #endregion
    }
}
