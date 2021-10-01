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
            return resource is null ? null : await Get(resource.Name);
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

        /// <summary>
        /// Returns the move with the given name.
        /// </summary>
        /// <param name="name">The move's name.</param>
        private async Task<MoveEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Move>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
