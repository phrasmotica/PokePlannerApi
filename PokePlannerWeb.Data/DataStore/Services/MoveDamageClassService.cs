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
    /// Service for managing the move damage class entries in the data store.
    /// </summary>
    public class MoveDamageClassService : NamedApiResourceServiceBase<MoveDamageClass, MoveDamageClassEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveDamageClassService(
            IDataStoreSource<MoveDamageClassEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MoveDamageClassCacheService moveDamageClassCacheService,
            ILogger<MoveDamageClassService> logger) : base(dataStoreSource, pokeApi, moveDamageClassCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a move damage class entry for the given move damage class.
        /// </summary>
        protected override Task<MoveDamageClassEntry> ConvertToEntry(MoveDamageClass damageClass)
        {
            var displayNames = damageClass.Names.Localise();

            return Task.FromResult(new MoveDamageClassEntry
            {
                Key = damageClass.Id,
                Name = damageClass.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion
    }
}
