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
    /// Service for managing the move entries in the data store.
    /// </summary>
    public class MoveService : INamedEntryService<Move, MoveEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Move, MoveEntry> _converter;
        private readonly IDataStoreSource<MoveEntry> _dataSource;

        public MoveService(
            IPokeApi pokeApi,
            IResourceConverter<Move, MoveEntry> converter,
            IDataStoreSource<MoveEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<MoveEntry> Get(NamedApiResource<Move> resource)
        {
            var move = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveId == move.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(move);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<MoveEntry> Get(NamedEntryRef<MoveEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var move = await _pokeApi.Get<Move>(entryRef.Key);
            var newEntry = await _converter.Convert(move);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<MoveEntry[]> Get(IEnumerable<NamedApiResource<Move>> resources)
        {
            var entries = new List<MoveEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
