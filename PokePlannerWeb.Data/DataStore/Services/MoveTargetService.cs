﻿using System.Linq;
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
    public class MoveTargetService : NamedApiResourceServiceBase<MoveTarget, MoveTargetEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveTargetService(
            IDataStoreSource<MoveTargetEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MoveTargetCacheService moveTargetCacheService,
            ILogger<MoveTargetService> logger) : base(dataStoreSource, pokeApi, moveTargetCacheService, logger)
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
