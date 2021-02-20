using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing stat entries.
    /// </summary>
    public class StatService : INamedEntryService<Stat, StatEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Stat, StatEntry> _converter;
        private readonly IDataStoreSource<StatEntry> _dataSource;

        public StatService(
            IPokeApi pokeApi,
            IResourceConverter<Stat, StatEntry> converter,
            IDataStoreSource<StatEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<StatEntry> Get(NamedApiResource<Stat> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<StatEntry> Get(NamedEntryRef<StatEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<StatEntry[]> Get(IEnumerable<NamedApiResource<Stat>> resources)
        {
            var entries = new List<StatEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the stat with the given name.
        /// </summary>
        /// <param name="name">The stat's name.</param>
        private async Task<StatEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Stat>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns all stats.
        /// </summary>
        public async Task<StatEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<Stat>();
            return await Get(resources.Results);
        }

        /// <summary>
        /// Returns all stats in the version group with the given ID.
        /// </summary>
        public async Task<StatEntry[]> GetBaseStats(int versionGroupId)
        {
            var allStats = await GetAll();
            return allStats.Where(e => !e.IsBattleOnly).ToArray();
        }
    }
}
