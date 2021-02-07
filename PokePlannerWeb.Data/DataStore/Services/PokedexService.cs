using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Services;
using PokePlannerWeb.Data.DataStore.Abstractions;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.Extensions;

namespace PokePlannerWeb.Data.DataStore.Services
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
            PokedexCacheService pokedexCacheService,
            ILogger<PokedexService> logger) : base(dataStoreSource, pokeApi, pokedexCacheService, logger)
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
