using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing ability entries.
    /// </summary>
    public class AbilityService : INamedEntryService<Ability, AbilityEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Ability, AbilityEntry> _converter;
        private readonly IDataStoreSource<AbilityEntry> _dataSource;

        public AbilityService(
            IPokeApi pokeApi,
            IResourceConverter<Ability, AbilityEntry> converter,
            IDataStoreSource<AbilityEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<AbilityEntry> Get(NamedApiResource<Ability> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<AbilityEntry> Get(NamedEntryRef<AbilityEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<AbilityEntry[]> Get(IEnumerable<NamedApiResource<Ability>> resources)
        {
            var entries = new List<AbilityEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the ability with the given name.
        /// </summary>
        /// <param name="name">The ability's name.</param>
        private async Task<AbilityEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Ability>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
