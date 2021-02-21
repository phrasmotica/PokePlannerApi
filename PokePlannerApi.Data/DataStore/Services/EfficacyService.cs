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
    /// Service for accessing efficacy entries.
    /// </summary>
    public class EfficacyService : INamedEntryService<Type, EfficacyEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Type, EfficacyEntry> _converter;
        private readonly IDataStoreSource<EfficacyEntry> _dataSource;
        private readonly PokemonService _pokemonService;
        private readonly TypeService _typeService;

        public EfficacyService(
            IPokeApi pokeApi,
            IResourceConverter<Type, EfficacyEntry> converter,
            IDataStoreSource<EfficacyEntry> dataSource,
            PokemonService pokemonService,
            TypeService typeService)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
            _pokemonService = pokemonService;
            _typeService = typeService;
        }

        /// <inheritdoc />
        public async Task<EfficacyEntry> Get(NamedApiResource<Type> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<EfficacyEntry[]> Get(IEnumerable<NamedApiResource<Type>> resources)
        {
            var entries = new List<EfficacyEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetEfficacySet(int typeId, int versionGroupId)
        {
            var entry = await _typeService.Get(typeId);
            return await GetEfficacySet(new[] { entry.TypeId }, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetEfficacySetByPokemonId(int pokemonId, int versionGroupId)
        {
            var pokemon = await _pokemonService.Get(pokemonId);
            var types = pokemon.Types.SingleOrDefault(e => e.Id == versionGroupId)?.Data;
            return await GetEfficacySet(types.Select(t => t.TypeId), versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy set of the type with the given ID in the
        /// version group with the given ID.
        /// </summary>
        public async Task<EfficacySet> GetEfficacySet(IEnumerable<int> typeIds, int versionGroupId)
        {
            var entries = await Get(typeIds);
            var efficacySets = entries.Select(e => e.GetEfficacySet(versionGroupId));
            return efficacySets.Aggregate((e1, e2) => e1.Product(e2));
        }

        /// <summary>
        /// Returns the efficacy of the type with the given ID.
        /// </summary>
        /// <param name="typeId">The type ID.</param>
        public async Task<EfficacyEntry> Get(int typeId)
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

        /// <summary>
        /// Returns the efficacy of the type with the given name.
        /// </summary>
        /// <param name="name">The type's name.</param>
        private async Task<EfficacyEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var type = await _pokeApi.Get<Type>(name);
            var newEntry = await _converter.Convert(type);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns the efficacies of the types with the given IDs.
        /// </summary>
        /// <param name="typeIds">The type IDs.</param>
        private async Task<EfficacyEntry[]> Get(IEnumerable<int> typeIds)
        {
            var entries = new List<EfficacyEntry>();

            foreach (var id in typeIds)
            {
                entries.Add(await Get(id));
            }

            return entries.ToArray();
        }
    }
}
