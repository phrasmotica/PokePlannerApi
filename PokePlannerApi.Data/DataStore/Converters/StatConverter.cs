using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class StatConverter : IResourceConverter<Stat, StatEntry>
    {
        /// <inheritdoc />
        public Task<StatEntry> Convert(Stat resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new StatEntry
            {
                StatId = resource.Id,
                Name = resource.Name,
                IsBattleOnly = resource.IsBattleOnly,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
