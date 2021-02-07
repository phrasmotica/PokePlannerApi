using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Services;
using PokePlannerWeb.Data.DataStore.Abstractions;
using PokePlannerWeb.Data.DataStore.Models;

namespace PokePlannerWeb.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing a collection of entries in the data store.
    /// </summary>
    public abstract class ServiceBase<TSource, TEntry>
        where TSource : ApiResource, new()
        where TEntry : EntryBase
    {
        /// <summary>
        /// The cache service for the resource type.
        /// </summary>
        protected readonly CacheServiceBase<TSource> CacheService;

        /// <summary>
        /// The data store source.
        /// </summary>
        protected IDataStoreSource<TEntry> DataStoreSource;

        /// <summary>
        /// The PokeAPI data fetcher.
        /// </summary>
        protected IPokeAPI PokeApi;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<ServiceBase<TSource, TEntry>> Logger;

        /// <summary>
        /// Connect to data store and initialise logger.
        /// </summary>
        public ServiceBase(
            IDataStoreSource<TEntry> dataStoreSource,
            IPokeAPI pokeApi,
            CacheServiceBase<TSource> cacheService,
            ILogger<ServiceBase<TSource, TEntry>> logger)
        {
            CacheService = cacheService;
            DataStoreSource = dataStoreSource;
            PokeApi = pokeApi;
            Logger = logger;
        }

        /// <summary>
        /// Gets the time to live for entries.
        /// </summary>
        protected virtual TimeSpan TimeToLive { get; } = TimeSpan.FromDays(365);

        #region CRUD methods

        /// <summary>
        /// Returns the entry with the given key.
        /// </summary>
        protected async Task<TEntry> Get(int key)
        {
            return await DataStoreSource.GetOne(t => t.Key == key);
        }

        /// <summary>
        /// Returns all entries.
        /// </summary>
        protected async Task<IEnumerable<TEntry>> GetAllEntries()
        {
            return await DataStoreSource.GetAll();
        }

        /// <summary>
        /// Creates a new entry and returns it.
        /// </summary>
        protected TEntry Create(TEntry entry)
        {
            entry.CreationTime = DateTime.UtcNow;
            DataStoreSource.Create(entry);
            return entry;
        }

        /// <summary>
        /// Removes the entry with the given key and creates a new one.
        /// </summary>
        protected void Update(int key, TEntry entry)
        {
            Remove(key);
            Create(entry);
        }

        /// <summary>
        /// Removes the entry with the given key.
        /// </summary>
        protected void Remove(int key)
        {
            DataStoreSource.DeleteOne(t => t.Key == key);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the entry with the given ID, creating the entry if it doesn't exist.
        /// </summary>
        public async Task<TEntry> Upsert(int key)
        {
            var entry = await Get(key);
            if (entry == null)
            {
                var sourceType = typeof(TSource).Name;
                var entryType = typeof(TEntry).Name;
                Logger.LogInformation($"Creating {entryType} entry for {sourceType} {key} in data store...");

                entry = await FetchSourceAndCreateEntry(key);
            }
            else if (IsStale(entry))
            {
                // update entry if it's stale
                var sourceType = typeof(TSource).Name;
                var entryType = typeof(TEntry).Name;
                Logger.LogInformation($"{entryType} entry with key {key} is stale.");
                Logger.LogInformation($"Updating {entryType} entry for {sourceType} {key} in data store...");

                entry = await FetchSourceAndUpdateEntry(key);
            }

            // TODO: a way of recreating the entry if it's different from the newly-computed candidate entry
            // might be able to compare hashes of their JSON representations
            // does require computing the entry every time but might not be so bad now that we have the cache

            return entry;
        }

        /// <summary>
        /// Creates a new entry for the given API resource and returns it.
        /// </summary>
        public virtual async Task<TEntry> Upsert(UrlNavigation<TSource> res)
        {
            var source = await CacheService.Upsert(res);
            return await Upsert(source);
        }

        /// <summary>
        /// Returns the entry with the given ID, creating the entry if it doesn't exist.
        /// </summary>
        public async Task<IEnumerable<TEntry>> UpsertMany(IEnumerable<int> keys)
        {
            var entries = new List<TEntry>();

            foreach (var key in keys)
            {
                var entry = await Upsert(key);
                entries.Add(entry);
            }

            return entries;
        }

        /// <summary>
        /// Creates new entries for the given API resources and returns them.
        /// </summary>
        public virtual async Task<IEnumerable<TEntry>> UpsertMany(IEnumerable<UrlNavigation<TSource>> resources)
        {
            var sources = await CacheService.UpsertMany(resources);
            return await UpsertMany(sources);
        }

        /// <summary>
        /// Upserts many entries into the data store.
        /// </summary>
        public virtual async Task<IEnumerable<TEntry>> UpsertMany(ApiResourceList<TSource> resources)
        {
            var sourceType = typeof(TSource).Name;
            var entryType = typeof(TEntry).Name;
            Logger.LogInformation($"Upserting {resources.Results.Count} {entryType} entries for {sourceType} in data store...");

            return await UpsertMany(resources.Results);
        }

        /// <summary>
        /// Creates or updates the entry with the given ID for the source object as needed.
        /// </summary>
        public virtual async Task<IEnumerable<TEntry>> UpsertMany(IEnumerable<TSource> sources)
        {
            var entries = new List<TEntry>();

            foreach (var source in sources)
            {
                var entry = await ConvertToEntry(source);
                var existingEntry = await Get(entry.Key);
                entries.Add(existingEntry ?? Create(entry));
            }

            return entries;
        }

        /// <summary>
        /// Upserts all entries into the data store.
        /// </summary>
        public async Task<IEnumerable<TEntry>> UpsertAll()
        {
            var resourcesPage = await PokeApi.GetPage<TSource>();

            var allEntries = await GetAllEntries();
            if (!allEntries.Any() || allEntries.ToList().Count != resourcesPage.Count)
            {
                const int pageSize = 20;

                var entryList = new List<TEntry>();

                var pagesUsed = 0;
                ApiResourceList<TSource> page;
                do
                {
                    page = await PokeApi.GetPage<TSource>(pageSize, pageSize * pagesUsed++);
                    var entries = await UpsertMany(page);
                    entryList.AddRange(entries);
                } while (!string.IsNullOrEmpty(page.Next));

                return entryList;
            }

            // if we have the right number of entries then we're probably good
            return allEntries;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the entries with the given keys.
        /// </summary>
        protected async Task<IEnumerable<TEntry>> GetMany(IEnumerable<int> keys)
        {
            var entries = new List<TEntry>();

            foreach (var key in keys)
            {
                entries.Add(await Get(key));
            }

            return entries;
        }

        /// <summary>
        /// Creates or updates the entry with the given ID for the source object as needed.
        /// </summary>
        protected virtual async Task<TEntry> Upsert(TSource source)
        {
            // this method of upserting by key requires (expensive) entry conversions
            var entry = await ConvertToEntry(source);
            var existingEntry = await Get(entry.Key);
            return existingEntry ?? Create(entry);
        }

        /// <summary>
        /// Fetches the source object with the given ID and creates an entry for it.
        /// </summary>
        protected async Task<TEntry> FetchSourceAndCreateEntry(int key)
        {
            // fetch source
            var source = await FetchSource(key);

            // create entry
            return await CreateEntry(source);
        }

        /// <summary>
        /// Fetches the source object with the given ID, updates the entry for it and returns the entry.
        /// </summary>
        protected async Task<TEntry> FetchSourceAndUpdateEntry(int key)
        {
            // fetch source
            var source = await FetchSource(key);

            // update entry
            await UpdateEntry(key, source);

            return await Get(key);
        }

        /// <summary>
        /// Returns the source object required to create an entry with the given ID.
        /// </summary>
        protected abstract Task<TSource> FetchSource(int key);

        /// <summary>
        /// Creates a new entry for the source object and returns it.
        /// </summary>
        protected async Task<TEntry> CreateEntry(TSource source)
        {
            var entry = await ConvertToEntry(source);
            return Create(entry);
        }

        /// <summary>
        /// Updates the entry with the given ID for the source object.
        /// </summary>
        protected async Task UpdateEntry(int key, TSource source)
        {
            var entry = await ConvertToEntry(source);
            Update(key, entry);
        }

        /// <summary>
        /// Returns an entry for the given source object.
        /// </summary>
        protected abstract Task<TEntry> ConvertToEntry(TSource source);

        /// <summary>
        /// Returns whether the entry is considered stale.
        /// </summary>
        protected bool IsStale(TEntry entry)
        {
            return entry.CreationTime < DateTime.UtcNow - TimeToLive;
        }

        #endregion
    }
}
