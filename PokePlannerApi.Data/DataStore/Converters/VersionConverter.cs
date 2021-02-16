using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class VersionConverter : IResourceConverter<Version, VersionEntry>
    {
        /// <inheritdoc />
        public Task<VersionEntry> Convert(Version resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new VersionEntry
            {
                VersionId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
