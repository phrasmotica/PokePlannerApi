using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class PokedexConverter : IResourceConverter<Pokedex, PokedexEntry>
    {
        /// <inheritdoc />
        public Task<PokedexEntry> Convert(Pokedex resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new PokedexEntry
            {
                PokedexId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
