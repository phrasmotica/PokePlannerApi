using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the pokedex entries in the data store.
    /// </summary>
    public class PokedexService : NamedApiResourceServiceBase<Pokedex, PokedexEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PokedexService(
            IDataStoreSource<PokedexEntry> dataStoreSource,
            IPokeAPI pokeApi,
            ILogger<PokedexService> logger) : base(dataStoreSource, pokeApi, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a pokedex entry for the given pokedex.
        /// </summary>
        protected override Task<PokedexEntry> ConvertToEntry(Pokedex pokedex)
        {
            var displayNames = pokedex.Names.Localise();

            return Task.FromResult(new PokedexEntry
            {
                Key = pokedex.Id,
                Name = pokedex.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all pokedexes.
        /// </summary>
        public async Task<PokedexEntry[]> GetAll()
        {
            var allPokedexes = await UpsertAll();
            return allPokedexes.ToArray();
        }

        #endregion
    }
}
