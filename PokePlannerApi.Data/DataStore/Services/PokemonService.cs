using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;
using Pokemon = PokeApiNet.Pokemon;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing Pokemon entries.
    /// </summary>
    public class PokemonService : INamedEntryService<Pokemon, PokemonEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Pokemon, PokemonEntry> _converter;
        private readonly IDataStoreSource<PokemonEntry> _dataSource;
        private readonly AbilityService _abilityService;
        private readonly MoveLearnMethodService _moveLearnMethodService;
        private readonly MoveService _moveService;
        private readonly VersionGroupService _versionGroupService;

        public PokemonService(
            IPokeApi pokeApi,
            IResourceConverter<Pokemon, PokemonEntry> converter,
            IDataStoreSource<PokemonEntry> dataSource,
            AbilityService abilityService,
            MoveLearnMethodService moveLearnMethodService,
            MoveService moveService,
            VersionGroupService versionGroupService)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
            _abilityService = abilityService;
            _moveLearnMethodService = moveLearnMethodService;
            _moveService = moveService;
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<PokemonEntry> Get(NamedApiResource<Pokemon> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<PokemonEntry[]> Get(IEnumerable<NamedApiResource<Pokemon>> resources)
        {
            var entries = new List<PokemonEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the entry for the Pokemon with the given ID.
        /// </summary>
        /// <param name="pokemonId">The Pokemon's ID.</param>
        public async Task<PokemonEntry> Get(int pokemonId)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonId == pokemonId);
            if (hasEntry)
            {
                return entry;
            }

            var pokemon = await _pokeApi.Get<Pokemon>(pokemonId);
            var newEntry = await _converter.Convert(pokemon);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns the abilities of the Pokemon with the given ID.
        /// </summary>
        public async Task<PokemonAbilityContext[]> GetPokemonAbilities(int pokemonId)
        {
            var resource = await _pokeApi.Get<Pokemon>(pokemonId);
            var orderedAbilities = resource.Abilities.OrderBy(a => a.Slot).ToArray();
            var abilityEntries = await _abilityService.Get(orderedAbilities.Select(a => a.Ability));

            var abilityContexts = abilityEntries.Select((e, i) =>
            {
                var context = PokemonAbilityContext.From(e);
                context.IsHidden = orderedAbilities[i].IsHidden;
                return context;
            });

            return abilityContexts.ToArray();
        }

        public async Task<List<PokemonFormEntry>> GetPokemonForms(int pokemonId, int versionGroupId)
        {
            var entry = await Get(pokemonId);
            return entry.Forms.OrderBy(f => f.PokemonFormId).ToList();
        }

        public async Task<PokemonMoveContext[]> GetPokemonMoves(int pokemonId, int versionGroupId)
        {
            var resource = await _pokeApi.Get<Pokemon>(pokemonId);
            var versionGroup = await _versionGroupService.Get(versionGroupId);

            var relevantMoves = resource.Moves.Where(m =>
            {
                var versionGroupNames = m.VersionGroupDetails.Select(d => d.VersionGroup.Name);
                return versionGroupNames.Contains(versionGroup.Name);
            }).ToArray();

            var moveEntries = await _moveService.Get(relevantMoves.Select(m => m.Move));
            var entryList = moveEntries.ToList();

            var moveContexts = new List<PokemonMoveContext>();

            for (int i = 0; i < entryList.Count; i++)
            {
                var moveEntry = entryList[i];
                var context = PokemonMoveContext.From(moveEntry);

                var relevantDetails = relevantMoves[i].VersionGroupDetails
                                                      .Where(d => d.VersionGroup.Name == versionGroup.Name);

                var methodList = new List<MoveLearnMethodEntry>();
                foreach (var detail in relevantDetails)
                {
                    var method = await _moveLearnMethodService.Get(detail.MoveLearnMethod);
                    if (method.Name == "level-up")
                    {
                        context.Level = detail.LevelLearnedAt;
                    }

                    if (method.Name == "machine")
                    {
                        var machines = moveEntry.Machines.SingleOrDefault(m => m.Id == versionGroupId)?.Data;
                        if (machines.Any())
                        {
                            var machineItems = machines.Select(mr => mr.Item).ToList();
                            context.LearnMachines = machineItems;
                        }
                    }

                    methodList.Add(method);
                }

                context.Methods = methodList;

                moveContexts.Add(context);
            }

            return moveContexts.ToArray();
        }

        /// <summary>
        /// Returns the Pokemon with the given name.
        /// </summary>
        /// <param name="name">The Pokemon's name.</param>
        private async Task<PokemonEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Pokemon>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
