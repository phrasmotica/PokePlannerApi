using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Extensions;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the type entries in the data store.
    /// </summary>
    public class TypeService : NamedApiResourceServiceBase<Type, TypeEntry>
    {
        /// <summary>
        /// The generations service.
        /// </summary>
        private readonly GenerationService GenerationsService;

        /// <summary>
        /// The version groups service.
        /// </summary>
        private readonly VersionGroupService VersionGroupsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeService(
            IDataStoreSource<TypeEntry> dataStoreSource,
            IPokeAPI pokeApi,
            TypeCacheService typeCacheService,
            GenerationService generationsService,
            VersionGroupService versionGroupsService,
            ILogger<TypeService> logger) : base(dataStoreSource, pokeApi, typeCacheService, logger)
        {
            GenerationsService = generationsService;
            VersionGroupsService = versionGroupsService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a type entry for the given type.
        /// </summary>
        protected override async Task<TypeEntry> ConvertToEntry(Type type)
        {
            var displayNames = type.Names.Localise();
            var efficacyMap = await GetEfficacyMap(type);
            var generation = await GenerationsService.Upsert(type.Generation);

            return new TypeEntry
            {
                Key = type.Id,
                Name = type.Name,
                DisplayNames = displayNames.ToList(),
                IsConcrete = type.Pokemon.Any(), // would like to use presence of move damage class but Fairy doesn't have it...
                EfficacyMap = efficacyMap,
                Generation = new Generation
                {
                    Id = generation.GenerationId,
                    Name = generation.Name
                }
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all types.
        /// </summary>
        public async Task<TypeEntry[]> GetAll()
        {
            var allTypes = await UpsertAll();
            return allTypes.OrderBy(t => t.TypeId).ToArray();
        }

        /// <summary>
        /// Returns all concrete types.
        /// </summary>
        public async Task<TypeEntry[]> GetConcrete()
        {
            var allTypes = await GetAll();
            return allTypes.Where(t => t.IsConcrete).ToArray();
        }

        /// <summary>
        /// Returns the efficacy of the type with the given ID in the version group with the given
        /// ID from the data store.
        /// </summary>
        public async Task<EfficacySet> GetTypesEfficacySet(IEnumerable<int> typeIds, int versionGroupId)
        {
            var entries = await UpsertMany(typeIds);
            var efficacySets = entries.Select(e => e.GetEfficacySet(versionGroupId));
            return efficacySets.Aggregate((e1, e2) => e1.Product(e2));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns true if the given generation uses the type with the given ID.
        /// </summary>
        private bool HasType(Generation generation, TypeEntry typeId)
        {
            return typeId.Generation.Id <= generation.Id;
        }

        /// <summary>
        /// Returns the efficacy of the given type in all version groups.
        /// </summary>
        private async Task<EfficacyMap> GetEfficacyMap(Type type)
        {
            var efficacy = new EfficacyMap();

            var versionGroups = await VersionGroupsService.GetAll();
            foreach (var vg in versionGroups)
            {
                var efficacySet = new EfficacySet();

                // populate damage relations - we can do this with the 'from' relations alone
                var damageRelations = await GetDamageRelations(type, vg.Key);

                foreach (var typeFrom in damageRelations.DoubleDamageFrom)
                {
                    var o = await CacheService.Upsert(typeFrom);
                    efficacySet.Add(o.Id, 2);
                }

                foreach (var typeFrom in damageRelations.HalfDamageFrom)
                {
                    var o = await CacheService.Upsert(typeFrom);
                    efficacySet.Add(o.Id, 0.5);
                }

                foreach (var typeFrom in damageRelations.NoDamageFrom)
                {
                    var o = await CacheService.Upsert(typeFrom);
                    efficacySet.Add(o.Id, 0);
                }

                efficacy.SetEfficacySet(vg.Key, efficacySet);
            }

            return efficacy;
        }

        /// <summary>
        /// Returns this type's damage relations in the version group with the given ID.
        /// </summary>
        private async Task<TypeRelations> GetDamageRelations(Type type, int versionGroupId)
        {
            if (versionGroupId == await VersionGroupsService.GetNewestVersionGroupId())
            {
                return type.DamageRelations;
            }

            var versionGroup = await VersionGroupsService.Upsert(versionGroupId);
            var pastDamageRelations = await GetPastDamageRelations(type, versionGroup.Generation);
            return pastDamageRelations ?? type.DamageRelations;
        }

        /// <summary>
        /// Returns this type's damage relations data for the given generation, if any.
        /// </summary>
        private async Task<TypeRelations> GetPastDamageRelations(Type type, Generation generation)
        {
            var pastDamageRelations = type.PastDamageRelations;
            if (pastDamageRelations.Any())
            {
                // use the earliest generation after the given one with past damage relation data,
                // if it exists
                var pastGenerations = await GenerationsService.UpsertMany(pastDamageRelations.Select(t => t.Generation));
                var laterGens = pastGenerations.Where(g => g.GenerationId >= generation.Id).ToList();
                if (laterGens.Any())
                {
                    var genToUse = laterGens.Aggregate((g, h) => g.GenerationId < h.GenerationId ? g : h);
                    return pastDamageRelations.Single(p => p.Generation.Name == genToUse.Name)
                                              .DamageRelations;
                }
            }

            return null;
        }

        #endregion
    }
}
