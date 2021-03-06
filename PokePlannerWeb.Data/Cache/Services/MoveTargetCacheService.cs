﻿using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of move target resources in the cache.
    /// </summary>
    public class MoveTargetCacheService : NamedCacheServiceBase<MoveTarget>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveTargetCacheService(
            INamedCacheSource<MoveTarget> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MoveTargetCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
