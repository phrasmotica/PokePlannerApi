using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    public interface IEntryService<TResource, TEntry>
        where TResource : ApiResource
        where TEntry : EntryBase
    {
        /// <summary>
        /// Returns an entry for the resource in the given reference object.
        /// </summary>
        /// <param name="resource">The reference object.</param>
        Task<TEntry> Get(ApiResource<TResource> resource);

        /// <summary>
        /// Returns entries for the resources in the given reference objects.
        /// </summary>
        /// <param name="resources">The reference objects.</param>
        Task<TEntry[]> Get(IEnumerable<ApiResource<TResource>> resources);
    }
}
