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
    /// Service for accessing move damage class entries.
    /// </summary>
    public class MoveDamageClassService : INamedEntryService<MoveDamageClass, MoveDamageClassEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<MoveDamageClass, MoveDamageClassEntry> _converter;
        private readonly IDataStoreSource<MoveDamageClassEntry> _dataSource;

        public MoveDamageClassService(
            IPokeApi pokeApi,
            IResourceConverter<MoveDamageClass, MoveDamageClassEntry> converter,
            IDataStoreSource<MoveDamageClassEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<MoveDamageClassEntry> Get(NamedApiResource<MoveDamageClass> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<MoveDamageClassEntry> Get(NamedEntryRef<MoveDamageClassEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<MoveDamageClassEntry[]> Get(IEnumerable<NamedApiResource<MoveDamageClass>> resources)
        {
            var entries = new List<MoveDamageClassEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the move damage class with the given name.
        /// </summary>
        /// <param name="name">The move damage class' name.</param>
        private async Task<MoveDamageClassEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<MoveDamageClass>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
