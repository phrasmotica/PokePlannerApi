using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;
using Pokemon = PokeApiNet.Pokemon;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing Pokemon entries.
    /// </summary>
    public class PokemonService : INamedEntryService<Pokemon, PokemonEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Pokemon, PokemonEntry> _converter;
        private readonly IDataStoreSource<PokemonEntry> _dataSource;

        public PokemonService(
            IPokeApi pokeApi,
            IResourceConverter<Pokemon, PokemonEntry> converter,
            IDataStoreSource<PokemonEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<PokemonEntry> Get(NamedApiResource<Pokemon> resource)
        {
            var pokemon = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonId == pokemon.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(pokemon);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<PokemonEntry> Get(NamedEntryRef<PokemonEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var pokemon = await _pokeApi.Get<Pokemon>(entryRef.Key);
            var newEntry = await _converter.Convert(pokemon);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<PokemonEntry[]> Get(IEnumerable<NamedApiResource<Pokemon>> resources)
        {
            var entries = new List<PokemonEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        public Task<PokemonEntry> Get(int pokemonId)
        {
            throw new NotImplementedException();
        }

        public Task<PokemonEntry[]> Get(IEnumerable<NamedEntryRef<PokemonEntry>> entryRefs)
        {
            throw new NotImplementedException();
        }

        public Task<PokemonAbilityContext[]> GetPokemonAbilities(int pokemonId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PokemonFormEntry>> GetPokemonForms(int pokemonId, int versionGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<PokemonMoveContext[]> GetPokemonMoves(int pokemonId, int versionGroupId)
        {
            throw new NotImplementedException();
        }
    }
}
