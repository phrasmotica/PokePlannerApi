﻿using System.Linq;
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
    /// Service for managing the stat entries in the data store.
    /// </summary>
    public class StatService : NamedApiResourceServiceBase<Stat, StatEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StatService(
            IDataStoreSource<StatEntry> dataStoreSource,
            IPokeAPI pokeApi,
            ILogger<StatService> logger) : base(dataStoreSource, pokeApi, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a stat entry for the given stat.
        /// </summary>
        protected override Task<StatEntry> ConvertToEntry(Stat stat)
        {
            var displayNames = stat.Names.Localise();

            return Task.FromResult(new StatEntry
            {
                Key = stat.Id,
                Name = stat.Name,
                IsBattleOnly = stat.IsBattleOnly,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all stats.
        /// </summary>
        public async Task<StatEntry[]> GetAll()
        {
            var allStats = await UpsertAll();
            return allStats.ToArray();
        }

        /// <summary>
        /// Returns all stats in the version group with the given ID.
        /// </summary>
        public async Task<StatEntry[]> GetBaseStats(int versionGroupId)
        {
            Logger.LogInformation($"Getting base stats in version group with ID {versionGroupId}...");
            var allStats = await GetAll();
            return allStats.Where(e => !e.IsBattleOnly).ToArray();
        }

        #endregion
    }
}
