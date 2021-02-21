using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class TypeConverter : IResourceConverter<Type, TypeEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly GenerationService _generationService;
        private readonly VersionGroupService _versionGroupService;

        public TypeConverter(
            IPokeApi pokeApi,
            GenerationService generationService,
            VersionGroupService versionGroupService)
        {
            _pokeApi = pokeApi;
            _generationService = generationService;
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<TypeEntry> Convert(Type resource)
        {
            var displayNames = resource.Names.Localise();
            var efficacyMap = await GetEfficacyMap(resource);
            var generation = await _generationService.Get(resource.Generation);

            return new TypeEntry
            {
                TypeId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                IsConcrete = resource.Pokemon.Any(), // would like to use presence of move damage class but Fairy doesn't have it...
                EfficacyMap = efficacyMap,
                Generation = generation,
            };
        }

        /// <summary>
        /// Returns the efficacy of the given type in all version groups.
        /// </summary>
        private async Task<EfficacyMap> GetEfficacyMap(Type type)
        {
            var efficacy = new EfficacyMap();

            var versionGroups = await _versionGroupService.GetAll();
            foreach (var vg in versionGroups)
            {
                var efficacySet = new EfficacySet();

                // populate damage relations - we can do this with the 'from' relations alone
                var damageRelations = type.DamageRelations;

                foreach (var typeFrom in damageRelations.DoubleDamageFrom)
                {
                    var o = await _pokeApi.Get(typeFrom);
                    efficacySet.Add(o.Id, 2);
                }

                foreach (var typeFrom in damageRelations.HalfDamageFrom)
                {
                    var o = await _pokeApi.Get(typeFrom);
                    efficacySet.Add(o.Id, 0.5);
                }

                foreach (var typeFrom in damageRelations.NoDamageFrom)
                {
                    var o = await _pokeApi.Get(typeFrom);
                    efficacySet.Add(o.Id, 0);
                }

                efficacy.SetEfficacySet(vg.VersionGroupId, efficacySet);
            }

            return efficacy;
        }
    }
}
