using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class EfficacyConverter : IResourceConverter<Type, EfficacyEntry>
    {
        private readonly TypeService _typeService;
        private readonly VersionGroupService _versionGroupService;

        public EfficacyConverter(
            TypeService generationService,
            VersionGroupService versionGroupService)
        {
            _typeService = generationService;
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<EfficacyEntry> Convert(Type resource)
        {
            var efficacySets = await GetEfficacySets(resource);

            return new EfficacyEntry
            {
                TypeId = resource.Id,
                Name = resource.Name,
                EfficacySets = efficacySets,
            };
        }

        /// <summary>
        /// Returns the efficacy sets of the given type in all version groups.
        /// </summary>
        private async Task<List<WithId<EfficacySet>>> GetEfficacySets(Type type)
        {
            var efficacy = new List<WithId<EfficacySet>>();

            var versionGroups = await _versionGroupService.GetAll();
            foreach (var vg in versionGroups)
            {
                var efficacySet = new EfficacySet();

                // populate damage relations - we can do this with the 'from' relations alone
                var damageRelations = type.DamageRelations;

                foreach (var typeFrom in damageRelations.DoubleDamageFrom)
                {
                    var o = await _typeService.Get(typeFrom);
                    efficacySet.Add(o.TypeId, 2);
                }

                foreach (var typeFrom in damageRelations.HalfDamageFrom)
                {
                    var o = await _typeService.Get(typeFrom);
                    efficacySet.Add(o.TypeId, 0.5);
                }

                foreach (var typeFrom in damageRelations.NoDamageFrom)
                {
                    var o = await _typeService.Get(typeFrom);
                    efficacySet.Add(o.TypeId, 0);
                }

                efficacy.Add(new WithId<EfficacySet>(vg.VersionGroupId, efficacySet));
            }

            return efficacy;
        }
    }
}
