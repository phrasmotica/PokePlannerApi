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
            var moveDamageClass = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveDamageClassId == moveDamageClass.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(moveDamageClass);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<MoveDamageClassEntry> Get(NamedEntryRef<MoveDamageClassEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveDamageClassId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var moveDamageClass = await _pokeApi.Get<MoveDamageClass>(entryRef.Key);
            var newEntry = await _converter.Convert(moveDamageClass);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
