using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class TypeConverter : IResourceConverter<Type, TypeEntry>
    {
        private readonly GenerationService _generationService;

        public TypeConverter(GenerationService generationService)
        {
            _generationService = generationService;
        }

        /// <inheritdoc />
        public async Task<TypeEntry> Convert(Type resource)
        {
            var displayNames = resource.Names.Localise();
            var generation = await _generationService.Get(resource.Generation);

            return new TypeEntry
            {
                TypeId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                IsConcrete = resource.Pokemon.Any(), // would like to use presence of move damage class but Fairy doesn't have it...
                Generation = generation,
            };
        }
    }
}
