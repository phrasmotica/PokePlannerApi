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
    /// Service for managing the move entries in the data store.
    /// </summary>
    public class MoveService : NamedApiResourceServiceBase<Move, MoveEntry>
    {
        /// <summary>
        /// The move category service.
        /// </summary>
        private readonly MoveCategoryService MoveCategoryService;

        /// <summary>
        /// The move damage class service.
        /// </summary>
        private readonly MoveDamageClassService MoveDamageClassService;

        /// <summary>
        /// The move target service.
        /// </summary>
        private readonly MoveTargetService MoveTargetService;

        /// <summary>
        /// The type service.
        /// </summary>
        private readonly TypeService TypeService;

        /// <summary>
        /// The version group service.
        /// </summary>
        private readonly VersionGroupService VersionGroupService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveService(
            IDataStoreSource<MoveEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MoveCategoryService moveCategoryService,
            MoveDamageClassService moveDamageClassService,
            MoveTargetService moveTargetService,
            TypeService typeService,
            VersionGroupService versionGroupService,
            ILogger<MoveService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            MoveCategoryService = moveCategoryService;
            MoveDamageClassService = moveDamageClassService;
            MoveTargetService = moveTargetService;
            TypeService = typeService;
            VersionGroupService = versionGroupService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a move entry for the given move.
        /// </summary>
        protected override async Task<MoveEntry> ConvertToEntry(Move move)
        {
            var displayNames = move.Names.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(move);
            var type = await TypeService.Upsert(move.Type);
            var category = await MoveCategoryService.Upsert(move.Meta.Category);
            var damageClass = await MoveDamageClassService.Upsert(move.DamageClass);
            var target = await MoveTargetService.Upsert(move.Target);
            var machines = await GetMachines(move);

            return new MoveEntry
            {
                Key = move.Id,
                Name = move.Name,
                DisplayNames = displayNames.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList(),
                Type = new Type
                {
                    Id = type.TypeId,
                    Name = type.Name
                },
                Category = new MoveCategory
                {
                    Id = category.MoveCategoryId,
                    Name = category.Name
                },
                Power = move.Power,
                DamageClass = new MoveDamageClass
                {
                    Id = damageClass.MoveDamageClassId,
                    Name = damageClass.Name
                },
                Accuracy = move.Accuracy,
                PP = move.Pp,
                Priority = move.Priority,
                Target = new MoveTarget
                {
                    Id = target.MoveTargetId,
                    Name = target.Name
                },
                Machines = machines.ToList()
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Returns flavour text entries for the given move, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<LocalString[]>>> GetFlavourTextEntries(Move move)
        {
            var descriptionsList = new List<WithId<LocalString[]>>();

            if (move.FlavorTextEntries.Any())
            {
                foreach (var vg in await VersionGroupService.GetAll())
                {
                    var relevantDescriptions = move.FlavorTextEntries.Where(f => f.VersionGroup.Name == vg.Name);
                    if (relevantDescriptions.Any())
                    {
                        var descriptions = relevantDescriptions.Select(d => new LocalString
                        {
                            Language = d.Language.Name,
                            Value = d.FlavorText
                        });

                        descriptionsList.Add(new WithId<LocalString[]>(vg.VersionGroupId, descriptions.ToArray()));
                    }
                }
            }

            return descriptionsList;
        }

        /// <summary>
        /// Returns machines for the given move, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<Machine[]>>> GetMachines(Move move)
        {
            var machinesList = new List<WithId<Machine[]>>();

            if (move.Machines.Any())
            {
                foreach (var vg in await VersionGroupService.GetAll())
                {
                    var relevantMachines = move.Machines.Where(m => m.VersionGroup.Name == vg.Name);
                    if (relevantMachines.Any())
                    {
                        var machines = new List<Machine>();
                        foreach (var m in relevantMachines)
                        {
                            var machine = await _pokeApi.Get(m.Machine);
                            machines.Add(machine);
                        }

                        machinesList.Add(new WithId<Machine[]>(vg.VersionGroupId, machines.ToArray()));
                    }
                }
            }

            return machinesList;
        }

        #endregion
    }
}
