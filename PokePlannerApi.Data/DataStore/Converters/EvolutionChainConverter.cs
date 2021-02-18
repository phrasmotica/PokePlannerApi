using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    // TODO: figure out the circular dependency between this and PokemonSpeciesService
    public class EvolutionChainConverter : IResourceConverter<EvolutionChain, EvolutionChainEntry>
    {
        private readonly EvolutionTriggerService _evolutionTriggerService;
        private readonly ItemService _itemService;
        private readonly LocationService _locationService;
        private readonly MoveService _moveService;
        private readonly PokemonSpeciesService _pokemonSpeciesService;
        private readonly TypeService _typeService;

        public EvolutionChainConverter(
            EvolutionTriggerService evolutionTriggerService,
            ItemService itemService,
            LocationService locationService,
            MoveService moveService,
            PokemonSpeciesService pokemonSpeciesService,
            TypeService typeService)
        {
            _evolutionTriggerService = evolutionTriggerService;
            _itemService = itemService;
            _locationService = locationService;
            _moveService = moveService;
            _pokemonSpeciesService = pokemonSpeciesService;
            _typeService = typeService;
        }

        /// <inheritdoc />
        public async Task<EvolutionChainEntry> Convert(EvolutionChain resource)
        {
            var chain = await CreateChainLinkEntry(resource.Chain);

            return new EvolutionChainEntry
            {
                EvolutionChainId = resource.Id,
                Chain = chain
            };
        }

        /// <summary>
        /// Returns a chain link entry for the given chain link.
        /// </summary>
        private async Task<ChainLinkEntry> CreateChainLinkEntry(ChainLink chainLink)
        {
            var species = await _pokemonSpeciesService.Get(chainLink.Species);
            var evolutionDetailEntries = await CreateEvolutionDetailEntries(chainLink.EvolutionDetails);

            var evolvesTo = new List<ChainLinkEntry>();

            foreach (var to in chainLink.EvolvesTo)
            {
                // create successive links recursively
                var entry = await CreateChainLinkEntry(to);
                evolvesTo.Add(entry);
            }

            return new ChainLinkEntry
            {
                IsBaby = chainLink.IsBaby,
                Species = species.ForEvolutionChain(),
                EvolutionDetails = evolutionDetailEntries.ToList(),
                EvolvesTo = evolvesTo
            };
        }

        /// <summary>
        /// Returns a list of evolution detail entries for the given list of evolution details.
        /// </summary>
        private async Task<IEnumerable<EvolutionDetailEntry>> CreateEvolutionDetailEntries(IEnumerable<EvolutionDetail> evolutionDetails)
        {
            var entryList = new List<EvolutionDetailEntry>();

            foreach (var detail in evolutionDetails)
            {
                var entry = await CreateEvolutionDetailEntry(detail);
                entryList.Add(entry);
            }

            return entryList;
        }

        /// <summary>
        /// Returns an evolution detail entry for the given evolution detail.
        /// </summary>
        private async Task<EvolutionDetailEntry> CreateEvolutionDetailEntry(EvolutionDetail evolutionDetail)
        {
            var item = await _itemService.Get(evolutionDetail.Item);
            var trigger = await _evolutionTriggerService.Get(evolutionDetail.Trigger);
            var heldItem = await _itemService.Get(evolutionDetail.HeldItem);
            var knownMove = await _moveService.Get(evolutionDetail.KnownMove);
            var knownMoveType = await _typeService.Get(evolutionDetail.KnownMoveType);
            var location = await _locationService.Get(evolutionDetail.Location);
            var partySpecies = await _pokemonSpeciesService.Get(evolutionDetail.PartySpecies);
            var partyType = await _typeService.Get(evolutionDetail.PartyType);
            var tradeSpecies = await _pokemonSpeciesService.Get(evolutionDetail.TradeSpecies);

            return new EvolutionDetailEntry
            {
                Item = item?.ForEvolutionChain(),
                Trigger = trigger?.ForEvolutionChain(),
                Gender = evolutionDetail.Gender,
                HeldItem = heldItem?.ForEvolutionChain(),
                KnownMove = knownMove?.ForEvolutionChain(),
                KnownMoveType = knownMoveType?.ForEvolutionChain(),
                Location = location?.ForEvolutionChain(),
                MinLevel = evolutionDetail.MinLevel,
                MinHappiness = evolutionDetail.MinHappiness,
                MinBeauty = evolutionDetail.MinBeauty,
                MinAffection = evolutionDetail.MinAffection,
                NeedsOverworldRain = evolutionDetail.NeedsOverworldRain,
                PartySpecies = partySpecies?.ForEvolutionChain(),
                PartyType = partyType?.ForEvolutionChain(),
                RelativePhysicalStats = evolutionDetail.RelativePhysicalStats,
                TimeOfDay = !string.IsNullOrEmpty(evolutionDetail.TimeOfDay) ? evolutionDetail.TimeOfDay : null,
                TradeSpecies = tradeSpecies?.ForEvolutionChain(),
                TurnUpsideDown = evolutionDetail.TurnUpsideDown
            };
        }
    }
}
