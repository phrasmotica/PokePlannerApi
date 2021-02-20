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
    /// Service for accessing generation entries.
    /// </summary>
    public class GenerationService : INamedEntryService<Generation, GenerationEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Generation, GenerationEntry> _converter;
        private readonly IDataStoreSource<GenerationEntry> _dataSource;

        public GenerationService(
            IPokeApi pokeApi,
            IResourceConverter<Generation, GenerationEntry> converter,
            IDataStoreSource<GenerationEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<GenerationEntry> Get(NamedApiResource<Generation> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<GenerationEntry> Get(NamedEntryRef<GenerationEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<GenerationEntry[]> Get(IEnumerable<NamedApiResource<Generation>> resources)
        {
            var entries = new List<GenerationEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns all generations.
        /// </summary>
        public async Task<GenerationEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<Generation>();
            return await Get(resources.Results);
        }

        /// <summary>
        /// Returns the generation with the given name.
        /// </summary>
        /// <param name="name">The generation's name.</param>
        private async Task<GenerationEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Generation>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
