using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the move damage class entries in the data store.
    /// </summary>
    public class MoveTargetService : NamedApiResourceServiceBase<MoveTarget, MoveTargetEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveTargetService(
            IDataStoreSource<MoveTargetEntry> dataStoreSource,
            IPokeAPI pokeApi,
            ILogger<MoveTargetService> logger) : base(dataStoreSource, pokeApi, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a move target entry for the given move target.
        /// </summary>
        protected override Task<MoveTargetEntry> ConvertToEntry(MoveTarget target)
        {
            var displayNames = target.Names.Localise();

            return Task.FromResult(new MoveTargetEntry
            {
                Key = target.Id,
                Name = target.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion
    }
}
