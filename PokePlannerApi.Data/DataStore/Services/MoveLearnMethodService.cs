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
    /// Service for accessing move learn method entries.
    /// </summary>
    public class MoveLearnMethodService : INamedEntryService<MoveLearnMethod, MoveLearnMethodEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<MoveLearnMethod, MoveLearnMethodEntry> _converter;
        private readonly IDataStoreSource<MoveLearnMethodEntry> _dataSource;

        public MoveLearnMethodService(
            IPokeApi pokeApi,
            IResourceConverter<MoveLearnMethod, MoveLearnMethodEntry> converter,
            IDataStoreSource<MoveLearnMethodEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        public async Task<MoveLearnMethodEntry> Get(NamedApiResource<MoveLearnMethod> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        public async Task<MoveLearnMethodEntry> Get(EntryRef<MoveLearnMethodEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        public async Task<MoveLearnMethodEntry[]> Get(IEnumerable<NamedApiResource<MoveLearnMethod>> resources)
        {
            var entries = new List<MoveLearnMethodEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the move learn method with the given name.
        /// </summary>
        /// <param name="name">The move learn method's name.</param>
        private async Task<MoveLearnMethodEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<MoveLearnMethod>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
