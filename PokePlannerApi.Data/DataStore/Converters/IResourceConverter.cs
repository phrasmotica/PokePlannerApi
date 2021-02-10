using System.Threading.Tasks;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public interface IResourceConverter<TResource, TEntry>
    {
        /// <summary>
        /// Converts the given resource to a corresponding entry.
        /// </summary>
        /// <param name="resource">The resource to convert.</param>
        Task<TEntry> ConvertToEntry(TResource resource);
    }
}
