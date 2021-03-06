﻿using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Models;

namespace PokePlannerWeb.Data.Cache.Abstractions
{
    /// <summary>
    /// Interface for data sources that cache named PokeAPI resources.
    /// </summary>
    public interface INamedCacheSource<TResource> : ICacheSource<TResource> where TResource : NamedApiResource
    {
        /// <summary>
        /// Returns the cache entry for the resource with the given name.
        /// </summary>
        Task<CacheEntry<TResource>> GetCacheEntry(string name);
    }
}
