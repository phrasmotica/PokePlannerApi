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
    /// Service for accessing type entries.
    /// </summary>
    public class TypeService : INamedEntryService<Type, TypeEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Type, TypeEntry> _converter;
        private readonly IDataStoreSource<TypeEntry> _dataSource;

        public TypeService(
            IPokeApi pokeApi,
            IResourceConverter<Type, TypeEntry> converter,
            IDataStoreSource<TypeEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<TypeEntry> Get(NamedApiResource<Type> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<TypeEntry[]> Get(IEnumerable<NamedApiResource<Type>> resources)
        {
            var entries = new List<TypeEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the type with the given name.
        /// </summary>
        /// <param name="name">The type's name.</param>
        private async Task<TypeEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Type>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns all types.
        /// </summary>
        public async Task<TypeEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<Type>();
            return await Get(resources.Results);
        }

        /// <summary>
        /// Returns all concrete types.
        /// </summary>
        public async Task<TypeEntry[]> GetConcrete()
        {
            var allTypes = await GetAll();
            return allTypes.Where(t => t.IsConcrete).ToArray();
        }

        /// <summary>
        /// Returns the type with the given ID.
        /// </summary>
        /// <param name="typeId">The type ID.</param>
        public async Task<TypeEntry> Get(int typeId)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.TypeId == typeId);
            if (hasEntry)
            {
                return entry;
            }

            var type = await _pokeApi.Get<Type>(typeId);
            var newEntry = await _converter.Convert(type);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
