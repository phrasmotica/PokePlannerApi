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
    /// Service for accessing move category class entries.
    /// </summary>
    public class MoveCategoryService : INamedEntryService<MoveCategory, MoveCategoryEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<MoveCategory, MoveCategoryEntry> _converter;
        private readonly IDataStoreSource<MoveCategoryEntry> _dataSource;

        public MoveCategoryService(
            IPokeApi pokeApi,
            IResourceConverter<MoveCategory, MoveCategoryEntry> converter,
            IDataStoreSource<MoveCategoryEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<MoveCategoryEntry> Get(NamedApiResource<MoveCategory> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<MoveCategoryEntry[]> Get(IEnumerable<NamedApiResource<MoveCategory>> resources)
        {
            var entries = new List<MoveCategoryEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the move category with the given name.
        /// </summary>
        /// <param name="name">The move category's name.</param>
        private async Task<MoveCategoryEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<MoveCategory>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
