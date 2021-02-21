using System;
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
    /// Service for accessing Pokemon form entries.
    /// </summary>
    public class PokemonFormService : INamedEntryService<PokemonForm, PokemonFormEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<PokemonForm, PokemonFormEntry> _converter;
        private readonly IDataStoreSource<PokemonFormEntry> _dataSource;

        public PokemonFormService(
            IPokeApi pokeApi,
            IResourceConverter<PokemonForm, PokemonFormEntry> converter,
            IDataStoreSource<PokemonFormEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<PokemonFormEntry> Get(NamedApiResource<PokemonForm> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<PokemonFormEntry> Get(EntryRef<PokemonFormEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<PokemonFormEntry[]> Get(IEnumerable<NamedApiResource<PokemonForm>> resources)
        {
            var entries = new List<PokemonFormEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the entry for the Pokemon form with the given ID.
        /// </summary>
        /// <param name="formId">The Pokemon form's ID.</param>
        public async Task<PokemonFormEntry> Get(int formId)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonFormId == formId);
            if (hasEntry)
            {
                return entry;
            }

            var pokemonForm = await _pokeApi.Get<PokemonForm>(formId);
            var newEntry = await _converter.Convert(pokemonForm);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns the Pokemon forms in the given reference objects.
        /// </summary>
        /// <param name="entryRef">The reference objects.</param>
        public async Task<PokemonFormEntry[]> Get(IEnumerable<EntryRef<PokemonFormEntry>> entryRefs)
        {
            var entries = new List<PokemonFormEntry>();

            foreach (var er in entryRefs)
            {
                entries.Add(await Get(er));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the Pokemon form with the given name.
        /// </summary>
        /// <param name="name">The Pokemon form's name.</param>
        private async Task<PokemonFormEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<PokemonForm>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
