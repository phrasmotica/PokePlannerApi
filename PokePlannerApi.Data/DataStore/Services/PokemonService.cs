using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;
using Pokemon = PokeApiNet.Pokemon;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the Pokemon entries in the data store.
    /// </summary>
    public class PokemonService : NamedApiResourceServiceBase<Pokemon, PokemonEntry>
    {
        private readonly AbilityService _abilityService;
        private readonly ItemService _itemService;
        private readonly MachineService _machineService;
        private readonly MoveLearnMethodService _moveLearnMethodService;
        private readonly MoveService _moveService;
        private readonly PokemonFormService _pokemonFormService;
        private readonly VersionGroupService _versionGroupService;
        private readonly VersionService _versionService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonService(
            IDataStoreSource<PokemonEntry> dataStoreSource,
            IPokeAPI pokeApi,
            AbilityService abilityService,
            ItemService itemService,
            MachineService machineService,
            MoveLearnMethodService moveLearnMethodService,
            MoveService moveService,
            PokemonFormService pokemonFormService,
            VersionGroupService versionGroupService,
            VersionService versionService,
            ILogger<PokemonService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            _abilityService = abilityService;
            _itemService = itemService;
            _machineService = machineService;
            _moveLearnMethodService = moveLearnMethodService;
            _moveService = moveService;
            _pokemonFormService = pokemonFormService;
            _versionGroupService = versionGroupService;
            _versionService = versionService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a Pokemon entry for the given Pokemon.
        /// </summary>
        protected override async Task<PokemonEntry> ConvertToEntry(Pokemon pokemon)
        {
            var displayNames = await GetDisplayNames(pokemon);
            var forms = await GetForms(pokemon);
            var types = await GetTypes(pokemon);
            var abilities = await GetAbilities(pokemon);
            var baseStats = await GetBaseStats(pokemon);
            var moves = await GetMoves(pokemon);
            var heldItems = await GetHeldItems(pokemon);

            return new PokemonEntry
            {
                Key = pokemon.Id,
                Name = pokemon.Name,
                SpriteUrl = GetSpriteUrl(pokemon),
                ShinySpriteUrl = GetShinySpriteUrl(pokemon),
                DisplayNames = displayNames.ToList(),
                Forms = forms.ToList(),
                Types = types,
                Abilities = abilities.ToList(),
                BaseStats = baseStats,
                Moves = moves.ToList(),
                HeldItems = heldItems.ToList()
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the Pokemon with the given ID from the data store.
        /// </summary>
        public async Task<PokemonEntry> GetPokemon(int pokemonId)
        {
            return await Upsert(pokemonId);
        }

        /// <summary>
        /// Returns the Pokemon forms of the Pokemon with the given ID in the version group with the
        /// given ID from the data store.
        /// </summary>
        public async Task<PokemonFormEntry[]> GetPokemonForms(int pokemonId, int versionGroupId)
        {
            var entry = await Upsert(pokemonId);
            var formEntries = await _pokemonFormService.UpsertMany(entry.Forms.Select(f => f.Id));
            return formEntries.OrderBy(f => f.FormId).ToArray();
        }

        /// <summary>
        /// Returns the moves of the Pokemon with the given ID in the version group with the
        /// given ID from the data store.
        /// </summary>
        public async Task<PokemonMoveContext[]> GetPokemonMoves(int pokemonId, int versionGroupId)
        {
            var resource = await _pokeApi.Get<Pokemon>(pokemonId);
            var versionGroup = await _versionGroupService.Upsert(versionGroupId);

            var relevantMoves = resource.Moves.Where(m =>
            {
                var versionGroupNames = m.VersionGroupDetails.Select(d => d.VersionGroup.Name);
                return versionGroupNames.Contains(versionGroup.Name);
            }).ToArray();

            var moveEntries = await _moveService.UpsertMany(relevantMoves.Select(m => m.Move));
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
                    var method = await _moveLearnMethodService.Upsert(detail.MoveLearnMethod);
                    if (method.Name == "level-up")
                    {
                        context.Level = detail.LevelLearnedAt;
                    }

                    if (method.Name == "machine")
                    {
                        var machineRefs = moveEntry.Machines.SingleOrDefault(m => m.Id == versionGroupId)?.Data;
                        if (machineRefs.Any())
                        {
                            var machineItems = new List<ItemEntry>();

                            foreach (var m in machineRefs)
                            {
                                var machineEntry = await _machineService.Upsert(m.Id);
                                var machineItem = await _itemService.Upsert(machineEntry.Item.Id);
                                machineItems.Add(machineItem);
                            }

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
        /// Returns the abilities of the Pokemon with the given ID from the data store.
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

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the display names of the given Pokemon.
        /// </summary>
        private async Task<IEnumerable<LocalString>> GetDisplayNames(Pokemon pokemon)
        {
            // take display names from primary form, if any
            var primaryForm = await _pokemonFormService.Upsert(pokemon.Forms[0]);
            return primaryForm.DisplayNames;
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of this Pokemon.
        /// </summary>
        private string GetSpriteUrl(Pokemon pokemon)
        {
            var frontDefaultUrl = pokemon.Sprites.FrontDefault;
            if (frontDefaultUrl == null)
            {
                Logger.LogWarning($"Pokemon {pokemon.Id} is missing front default sprite");
            }

            return frontDefaultUrl;
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of this Pokemon.
        /// </summary>
        private string GetShinySpriteUrl(Pokemon pokemon)
        {
            var frontShinyUrl = pokemon.Sprites.FrontShiny;
            if (frontShinyUrl == null)
            {
                Logger.LogWarning($"Pokemon {pokemon.Id} is missing front default shiny sprite");
            }

            return frontShinyUrl;
        }

        /// <summary>
        /// Returns the Pokemon that this Pokemon species represents.
        /// </summary>
        private async Task<IEnumerable<PokemonForm>> GetForms(Pokemon pokemon)
        {
            var formsList = new List<PokemonForm>();

            foreach (var form in pokemon.Forms)
            {
                var source = await _pokemonFormService.Upsert(form);
                formsList.Add(new PokemonForm
                {
                    Id = source.FormId,
                    Name = source.Name
                });
            }

            return formsList;
        }

        /// <summary>
        /// Returns the types of the given Pokemon in past version groups, if any.
        /// </summary>
        private async Task<List<WithId<Type[]>>> GetTypes(Pokemon pokemon)
        {
            var typesList = new List<WithId<Type[]>>();

            var newestId = await _versionGroupService.GetNewestVersionGroupId();
            var newestTypeEntries = await MinimiseTypes(pokemon.Types);
            typesList.Add(new WithId<Type[]>(newestId, newestTypeEntries.ToArray()));

            return typesList;
        }

        /// <summary>
        /// Minimises a set of Pokemon types.
        /// </summary>
        private async Task<IEnumerable<Type>> MinimiseTypes(IEnumerable<PokemonType> types)
        {
            var newestTypeObjs = await _pokeApi.Get(types.Select(t => t.Type));
            return newestTypeObjs.Select(t => t.Minimise());
        }

        /// <summary>
        /// Returns references to this Pokemon's abilities.
        /// </summary>
        private async Task<IEnumerable<Ability>> GetAbilities(Pokemon pokemon)
        {
            var abilities = new List<Ability>();

            foreach (var ability in pokemon.Abilities.Select(a => a.Ability))
            {
                var abilityRef = await _pokeApi.Get(ability);
                abilities.Add(abilityRef);
            }

            return abilities;
        }

        /// <summary>
        /// Returns the base stats of the given Pokemon.
        /// </summary>
        private async Task<List<WithId<int[]>>> GetBaseStats(Pokemon pokemon)
        {
            // FUTURE: anticipating a generation-based base stats changelog
            // in which case this method will need to look like GetTypes()
            var newestId = await _versionGroupService.GetNewestVersionGroupId();
            var currentBaseStats = pokemon.GetBaseStats(newestId);

            var versionGroups = await _versionGroupService.GetAll();
            var statsList = versionGroups.Select(
                vg => new WithId<int[]>(vg.VersionGroupId, currentBaseStats)
            );

            return statsList.ToList();
        }

        /// <summary>
        /// Returns the moves of the given Pokemon.
        /// </summary>
        private async Task<IEnumerable<WithId<Move[]>>> GetMoves(Pokemon pokemon)
        {
            var movesList = new List<WithId<Move[]>>();

            var versionGroups = await _versionGroupService.GetAll();
            foreach (var vg in versionGroups)
            {
                var moves = await GetMoves(pokemon, vg);
                var movesEntry = new WithId<Move[]>(vg.VersionGroupId, moves.ToArray());
                movesList.Add(movesEntry);
            }

            return movesList;
        }

        /// <summary>
        /// Returns the given Pokemon's moves in the given version group.
        /// </summary>
        private async Task<IEnumerable<Move>> GetMoves(Pokemon pokemon, VersionGroupEntry versionGroup)
        {
            var allMoves = pokemon.Moves;
            var relevantMoves = allMoves.Where(m =>
            {
                var versionGroupNames = m.VersionGroupDetails.Select(vgd => vgd.VersionGroup.Name);
                return versionGroupNames.Contains(versionGroup.Name);
            });

            var moveEntries = await _moveService.UpsertMany(relevantMoves.Select(m => m.Move));
            return moveEntries.Select(m => new Move
            {
                Id = m.MoveId,
                Name = m.Name
            });
        }

        /// <summary>
        /// Returns the held items of the given Pokemon, indexed by version ID.
        /// </summary>
        private async Task<IEnumerable<WithId<VersionHeldItemContext[]>>> GetHeldItems(Pokemon pokemon)
        {
            var itemsList = new List<WithId<VersionHeldItemContext[]>>();

            var versions = await _versionService.GetAll();
            foreach (var v in versions)
            {
                var items = await GetHeldItems(pokemon, v);
                if (items.Any())
                {
                    var itemsEntry = new WithId<VersionHeldItemContext[]>(v.VersionId, items.ToArray());
                    itemsList.Add(itemsEntry);
                }
            }

            return itemsList;
        }

        /// <summary>
        /// Returns the given Pokemon's held items in the given version.
        /// </summary>
        private async Task<IEnumerable<VersionHeldItemContext>> GetHeldItems(Pokemon pokemon, VersionEntry version)
        {
            var allHeldItems = pokemon.HeldItems;
            var relevantHeldItems = allHeldItems.Where(h =>
            {
                var versionGroupNames = h.VersionDetails.Select(vd => vd.Version.Name);
                return versionGroupNames.Contains(version.Name);
            }).ToArray();

            var itemEntries = await _itemService.UpsertMany(relevantHeldItems.Select(m => m.Item));
            return itemEntries.Select((item, index) =>
            {
                var context = VersionHeldItemContext.From(item);

                var detail = relevantHeldItems[index].VersionDetails.Single(d => d.Version.Name == version.Name);
                context.Rarity = detail.Rarity;

                return context;
            });
        }

        #endregion
    }
}
