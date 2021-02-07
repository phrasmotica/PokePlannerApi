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
    /// Service for managing the location entries in the data store.
    /// </summary>
    public class LocationService : NamedApiResourceServiceBase<Location, LocationEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocationService(
            IDataStoreSource<LocationEntry> dataStoreSource,
            IPokeAPI pokeApi,
            LocationCacheService locationCacheService,
            ILogger<LocationService> logger) : base(dataStoreSource, pokeApi, locationCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a location entry for the given location.
        /// </summary>
        protected override Task<LocationEntry> ConvertToEntry(Location location)
        {
            var displayNames = location.Names.Localise();

            return Task.FromResult(new LocationEntry
            {
                Key = location.Id,
                Name = location.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion
    }
}
