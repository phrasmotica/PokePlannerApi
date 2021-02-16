using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class VersionGroupConverter : IResourceConverter<VersionGroup, VersionGroupEntry>
    {
        private readonly GenerationService _generationService;
        private readonly PokedexService _pokedexService;
        private readonly VersionService _versionService;

        public VersionGroupConverter(
            GenerationService generationService,
            PokedexService pokedexService,
            VersionService versionService)
        {
            _generationService = generationService;
            _pokedexService = pokedexService;
            _versionService = versionService;
        }

        /// <inheritdoc />
        public async Task<VersionGroupEntry> Convert(VersionGroup resource)
        {
            var displayNames = await GetDisplayNames(resource);
            var generation = await _generationService.Get(resource.Generation);
            var versions = await _versionService.Get(resource.Versions);
            var pokedexes = await _pokedexService.Get(resource.Pokedexes);

            return new VersionGroupEntry
            {
                VersionGroupId = resource.Id,
                Name = resource.Name,
                Order = resource.Order,
                DisplayNames = displayNames.ToList(),
                Generation = generation.ToRef(),
                Versions = versions.Select(v => v.ToRef()).ToList(),
                Pokedexes = pokedexes.Select(p => p.ToRef()).ToList(),
            };
        }

        /// <summary>
        /// Returns the display names of the given version group in all locales.
        /// </summary>
        private async Task<IEnumerable<LocalString>> GetDisplayNames(VersionGroup versionGroup)
        {
            var versions = await _versionService.Get(versionGroup.Versions);
            var versionsNames = versions.Select(v => v.DisplayNames.OrderBy(n => n.Language).ToList());
            var namesList = versionsNames.Aggregate(
                (nv1, nv2) => nv1.Zip(
                    nv2, (n1, n2) => new LocalString
                    {
                        Language = n1.Language,
                        Value = n1.Value + "/" + n2.Value
                    }
                ).ToList()
            );

            return namesList;
        }
    }
}
