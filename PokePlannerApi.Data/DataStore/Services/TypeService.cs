using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the type entries in the data store.
    /// </summary>
    public class TypeService : NamedApiResourceServiceBase<Type, TypeEntry>
    {
        private readonly GenerationService _generationService;
        private readonly VersionGroupService _versionGroupService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeService(
            IDataStoreSource<TypeEntry> dataStoreSource,
            IPokeAPI pokeApi,
            GenerationService generationService,
            VersionGroupService versionGroupService,
            ILogger<TypeService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            _generationService = generationService;
            _versionGroupService = versionGroupService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a type entry for the given type.
        /// </summary>
        protected override async Task<TypeEntry> ConvertToEntry(Type type)
        {
            var displayNames = type.Names.Localise();
            var efficacyMap = await GetEfficacyMap(type);
            var generation = await _generationService.Upsert(type.Generation);

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

                efficacy.SetEfficacySet(vg.Key, efficacySet);
            }

            return efficacy;
        }

        #endregion
    }
}
