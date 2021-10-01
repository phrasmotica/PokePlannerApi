using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveConverter : IResourceConverter<Move, MoveEntry>
    {
        private readonly MachineService _machineService;
        private readonly MoveCategoryService _moveCategoryService;
        private readonly MoveDamageClassService _moveDamageClassService;
        private readonly MoveTargetService _moveTargetService;
        private readonly TypeService _typeService;
        private readonly VersionGroupService _versionGroupService;

        public MoveConverter(
            MachineService machineService,
            MoveCategoryService moveCategoryService,
            MoveDamageClassService moveDamageClassService,
            MoveTargetService moveTargetService,
            TypeService typeService,
            VersionGroupService versionGroupService)
        {
            _machineService = machineService;
            _moveCategoryService = moveCategoryService;
            _moveDamageClassService = moveDamageClassService;
            _moveTargetService = moveTargetService;
            _typeService = typeService;
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<MoveEntry> Convert(Move resource)
        {
            var displayNames = resource.Names.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(resource);
            var type = await _typeService.Get(resource.Type);
            var category = await _moveCategoryService.Get(resource.Meta.Category);
            var damageClass = await _moveDamageClassService.Get(resource.DamageClass);
            var target = await _moveTargetService.Get(resource.Target);
            var machines = await GetMachines(resource);

            return new MoveEntry
            {
                MoveId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList(),
                Type = type,
                Category = category,
                Power = resource.Power,
                DamageClass = damageClass,
                Accuracy = resource.Accuracy,
                PP = resource.Pp,
                Priority = resource.Priority,
                Target = target,
                Machines = machines.ToList(),
            };
        }

        /// <summary>
        /// Returns flavour text entries for the given ability, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<List<LocalString>>>> GetFlavourTextEntries(Move move)
        {
            var descriptionsList = new List<WithId<List<LocalString>>>();

            if (move.FlavorTextEntries.Any())
            {
                foreach (var vg in await _versionGroupService.GetAll())
                {
                    var relevantDescriptions = move.FlavorTextEntries.Where(f => f.VersionGroup.Name == vg.Name);
                    if (relevantDescriptions.Any())
                    {
                        var descriptions = relevantDescriptions.Select(d => new LocalString
                        {
                            Language = d.Language.Name,
                            Value = d.FlavorText
                        }).ToList();

                        descriptionsList.Add(new WithId<List<LocalString>>(vg.VersionGroupId, descriptions));
                    }
                }
            }

            return descriptionsList;
        }

        /// <summary>
        /// Returns machines for the given move, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<List<MachineEntry>>>> GetMachines(Move move)
        {
            var entries = new List<WithId<List<MachineEntry>>>();

            if (move.Machines.Any())
            {
                foreach (var vg in await _versionGroupService.GetAll())
                {
                    var relevantMachines = move.Machines.Where(m => m.VersionGroup.Name == vg.Name);
                    if (relevantMachines.Any())
                    {
                        var machines = new List<MachineEntry>();
                        foreach (var m in relevantMachines)
                        {
                            var machine = await _machineService.Get(m.Machine);
                            machines.Add(machine);
                        }

                        entries.Add(new WithId<List<MachineEntry>>(vg.VersionGroupId, machines));
                    }
                }
            }

            return entries;
        }
    }
}
