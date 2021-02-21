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
    /// Service for accessing item entries.
    /// </summary>
    public class ItemService : INamedEntryService<Item, ItemEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Item, ItemEntry> _converter;
        private readonly IDataStoreSource<ItemEntry> _dataSource;

        public ItemService(
            IPokeApi pokeApi,
            IResourceConverter<Item, ItemEntry> converter,
            IDataStoreSource<ItemEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<ItemEntry> Get(NamedApiResource<Item> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<ItemEntry[]> Get(IEnumerable<NamedApiResource<Item>> resources)
        {
            var entries = new List<ItemEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the item with the given name.
        /// </summary>
        /// <param name="name">The item's name.</param>
        private async Task<ItemEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Item>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
