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
    /// Service for managing the move learn method entries in the data store.
    /// </summary>
    public class MoveLearnMethodService : NamedApiResourceServiceBase<MoveLearnMethod, MoveLearnMethodEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveLearnMethodService(
            IDataStoreSource<MoveLearnMethodEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MoveLearnMethodCacheService moveLearnMethodCacheService,
            ILogger<MoveLearnMethodService> logger) : base(dataStoreSource, pokeApi, moveLearnMethodCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a move learn method entry for the given move learn method.
        /// </summary>
        protected override Task<MoveLearnMethodEntry> ConvertToEntry(MoveLearnMethod moveLearnMethod)
        {
            var displayNames = moveLearnMethod.Names.Localise();
            var displayDescriptions = moveLearnMethod.Descriptions.Localise();

            return Task.FromResult(new MoveLearnMethodEntry
            {
                Key = moveLearnMethod.Id,
                Name = moveLearnMethod.Name,
                DisplayNames = displayNames.ToList(),
                Descriptions = displayDescriptions.ToList()
            });
        }

        #endregion
    }
}
