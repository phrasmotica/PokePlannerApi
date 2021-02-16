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
            var pokemonForm = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonFormId == pokemonForm.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(pokemonForm);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<PokemonFormEntry> Get(NamedEntryRef<PokemonFormEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonFormId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var pokemonForm = await _pokeApi.Get<PokemonForm>(entryRef.Key);
            var newEntry = await _converter.Convert(pokemonForm);
            await _dataSource.Create(newEntry);

            return newEntry;
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

        public Task<PokemonFormEntry> Get(int formId)
        {
            throw new NotImplementedException();
        }
    }
}
