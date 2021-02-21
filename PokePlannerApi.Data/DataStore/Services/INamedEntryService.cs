using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    public interface INamedEntryService<TResource, TEntry>
        where TResource : NamedApiResource
        where TEntry : EntryBase
    {
        /// <summary>
        /// Returns an entry for the resource in the given reference object.
        /// </summary>
        /// <param name="resource">The reference object.</param>
        Task<TEntry> Get(NamedApiResource<TResource> resource);

        /// <summary>
        /// Returns the entry in the given reference object.
        /// </summary>
        /// <param name="entryRef">The reference object.</param>
        Task<TEntry> Get(EntryRef<TEntry> entryRef);

        /// <summary>
        /// Returns entries for the resources in the given reference objects.
        /// </summary>
        /// <param name="resources">The reference objects.</param>
        Task<TEntry[]> Get(IEnumerable<NamedApiResource<TResource>> resources);
    }
}
