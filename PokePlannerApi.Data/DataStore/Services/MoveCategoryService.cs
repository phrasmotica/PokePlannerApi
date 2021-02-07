using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Extensions;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the move category class entries in the data store.
    /// </summary>
    public class MoveCategoryService : NamedApiResourceServiceBase<MoveCategory, MoveCategoryEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveCategoryService(
            IDataStoreSource<MoveCategoryEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MoveCategoryCacheService moveTargetCacheService,
            ILogger<MoveCategoryService> logger) : base(dataStoreSource, pokeApi, moveTargetCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a move category entry for the given move category.
        /// </summary>
        protected override Task<MoveCategoryEntry> ConvertToEntry(MoveCategory moveCategory)
        {
            var displayDescriptions = moveCategory.Descriptions.Localise();

            return Task.FromResult(new MoveCategoryEntry
            {
                Key = moveCategory.Id,
                Name = moveCategory.Name,
                Descriptions = displayDescriptions.ToList()
            });
        }

        #endregion
    }
}
