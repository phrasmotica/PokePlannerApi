using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class PokemonConverter : IResourceConverter<Pokemon, PokemonEntry>
    {
        private readonly AbilityService _abilityService;
        private readonly ItemService _itemService;
        private readonly MoveService _moveService;
        private readonly PokemonFormService _pokemonFormService;
        private readonly TypeService _typeService;
        private readonly VersionGroupService _versionGroupService;
        private readonly VersionService _versionService;
        private readonly ILogger<PokemonConverter> _logger;

        public PokemonConverter(
            AbilityService abilityService,
            ItemService itemService,
            MoveService moveService,
            PokemonFormService pokemonFormService,
            TypeService typeService,
            VersionGroupService versionGroupService,
            VersionService versionService,
            ILogger<PokemonConverter> logger)
        {
            _abilityService = abilityService;
            _itemService = itemService;
            _moveService = moveService;
            _pokemonFormService = pokemonFormService;
            _typeService = typeService;
            _versionGroupService = versionGroupService;
            _versionService = versionService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<PokemonEntry> Convert(Pokemon resource)
        {
            var displayNames = await GetDisplayNames(resource);
            var forms = await _pokemonFormService.Get(resource.Forms);
            var types = await GetTypes(resource);
            var abilities = await _abilityService.Get(resource.Abilities.Select(a => a.Ability));
            var baseStats = await GetBaseStats(resource);
            var moves = await GetMoves(resource);
            var heldItems = await GetHeldItems(resource);

            return new PokemonEntry
            {
                PokemonId = resource.Id,
                Name = resource.Name,
                SpriteUrl = GetSpriteUrl(resource),
                ShinySpriteUrl = GetShinySpriteUrl(resource),
                DisplayNames = displayNames.ToList(),
                Forms = forms.Select(e => e.ToRef()).ToList(),
                Types = types.ToList(),
                Abilities = abilities.Select(e => e.ToRef()).ToList(),
                BaseStats = baseStats,
                Moves = moves.ToList(),
                HeldItems = heldItems.ToList()
            };
        }

        /// <summary>
        /// Returns the display names of the given Pokemon.
        /// </summary>
        private async Task<IEnumerable<LocalString>> GetDisplayNames(Pokemon pokemon)
        {
            // take display names from primary form, if any
            var primaryForm = await _pokemonFormService.Get(pokemon.Forms[0]);
            return primaryForm.DisplayNames;
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of the given Pokemon.
        /// </summary>
        private string GetSpriteUrl(Pokemon pokemon)
        {
            var frontDefaultUrl = pokemon.Sprites.FrontDefault;
            if (frontDefaultUrl == null)
            {
                _logger.LogWarning($"Pokemon {pokemon.Id} is missing front default sprite");
            }

            return frontDefaultUrl;
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of the given Pokemon.
        /// </summary>
        private string GetShinySpriteUrl(Pokemon pokemon)
        {
            var frontShinyUrl = pokemon.Sprites.FrontShiny;
            if (frontShinyUrl == null)
            {
                _logger.LogWarning($"Pokemon {pokemon.Id} is missing front default shiny sprite");
            }

            return frontShinyUrl;
        }

        /// <summary>
        /// Returns the types of the given Pokemon in past version groups, if any.
        /// </summary>
        private async Task<List<WithId<List<EntryRef<TypeEntry>>>>> GetTypes(Pokemon pokemon)
        {
            var typesList = new List<WithId<List<EntryRef<TypeEntry>>>>();

            var newestId = await _versionGroupService.GetNewestVersionGroupId();
            var newestTypeEntries = await _typeService.Get(pokemon.Types.Select(t => t.Type));

            typesList.Add(
                new WithId<List<EntryRef<TypeEntry>>>(
                    newestId,
                    newestTypeEntries.Select(e => e.ToRef()).ToList()
                )
            );

            return typesList;
        }

        /// <summary>
        /// Returns the base stats of the given Pokemon.
        /// </summary>
        private async Task<List<WithId<List<int>>>> GetBaseStats(Pokemon pokemon)
        {
            // FUTURE: anticipating a generation-based base stats changelog
            // in which case this method will need to look like GetTypes()
            var newestId = await _versionGroupService.GetNewestVersionGroupId();
            var currentBaseStats = pokemon.GetBaseStats(newestId);

            var versionGroups = await _versionGroupService.GetAll();
            var statsList = versionGroups.Select(
                vg => new WithId<List<int>>(vg.VersionGroupId, currentBaseStats)
            );

            return statsList.ToList();
        }

        /// <summary>
        /// Returns the moves of the given Pokemon.
        /// </summary>
        private async Task<IEnumerable<WithId<List<EntryRef<MoveEntry>>>>> GetMoves(Pokemon pokemon)
        {
            var movesList = new List<WithId<List<EntryRef<MoveEntry>>>>();

            var versionGroups = await _versionGroupService.GetAll();
            foreach (var vg in versionGroups)
            {
                var moves = await GetMoves(pokemon, vg);
                var movesEntry = new WithId<List<EntryRef<MoveEntry>>>(
                    vg.VersionGroupId,
                    moves.Select(e => e.ToRef()).ToList()
                );

                movesList.Add(movesEntry);
            }

            return movesList;
        }

        /// <summary>
        /// Returns the given Pokemon's moves in the given version group.
        /// </summary>
        private async Task<IEnumerable<MoveEntry>> GetMoves(Pokemon pokemon, VersionGroupEntry versionGroup)
        {
            var allMoves = pokemon.Moves;
            var relevantMoves = allMoves.Where(m =>
            {
                var versionGroupNames = m.VersionGroupDetails.Select(vgd => vgd.VersionGroup.Name);
                return versionGroupNames.Contains(versionGroup.Name);
            });

            return await _moveService.Get(relevantMoves.Select(m => m.Move));
        }

        /// <summary>
        /// Returns the held items of the given Pokemon, indexed by version ID.
        /// </summary>
        private async Task<IEnumerable<WithId<List<VersionHeldItemContext>>>> GetHeldItems(Pokemon pokemon)
        {
            var itemsList = new List<WithId<List<VersionHeldItemContext>>>();

            var versions = await _versionService.GetAll();
            foreach (var v in versions)
            {
                var items = await GetHeldItems(pokemon, v);
                if (items.Any())
                {
                    var itemsEntry = new WithId<List<VersionHeldItemContext>>(
                        v.VersionId,
                        items.ToList()
                    );

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

            var itemEntries = await _itemService.Get(relevantHeldItems.Select(m => m.Item));
            return itemEntries.Select((item, index) =>
            {
                var context = VersionHeldItemContext.From(item);

                var detail = relevantHeldItems[index].VersionDetails.Single(d => d.Version.Name == version.Name);
                context.Rarity = detail.Rarity;

                return context;
            });
        }
    }
}
