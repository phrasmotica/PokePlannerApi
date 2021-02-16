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
            var generation = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.GenerationId == generation.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(generation);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<GenerationEntry> Get(NamedEntryRef<GenerationEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.GenerationId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var generation = await _pokeApi.Get<Generation>(entryRef.Key);
            var newEntry = await _converter.Convert(generation);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
