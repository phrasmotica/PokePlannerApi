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
    /// Service for managing the evolution chain entries in the data store.
    /// </summary>
    public class EvolutionChainService : ServiceBase<EvolutionChain, EvolutionChainEntry>
    {
        /// <summary>
        /// The evolution trigger service.
        /// </summary>
        private readonly EvolutionTriggerService EvolutionTriggerService;

        /// <summary>
        /// The item service.
        /// </summary>
        private readonly ItemService ItemService;

        /// <summary>
        /// The location service.
        /// </summary>
        private readonly LocationService LocationService;

        /// <summary>
        /// The move service.
        /// </summary>
        private readonly MoveService MoveService;

        private readonly PokemonSpeciesService _pokemonSpeciesService;

        /// <summary>
        /// The type service.
        /// </summary>
        private readonly TypeService TypeService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionChainService(
            IDataStoreSource<EvolutionChainEntry> dataStoreSource,
            IPokeAPI pokeApi,
            EvolutionTriggerService evolutionTriggerService,
            ItemService itemService,
            LocationService locationService,
            MoveService moveService,
            PokemonSpeciesService pokemonSpeciesService,
            TypeService typeCacheService,
            ILogger<EvolutionChainService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            EvolutionTriggerService = evolutionTriggerService;
            ItemService = itemService;
            LocationService = locationService;
            MoveService = moveService;
            TypeService = typeCacheService;
            _pokemonSpeciesService = pokemonSpeciesService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a generation entry for the given generation.
        /// </summary>
        protected override async Task<EvolutionChainEntry> ConvertToEntry(EvolutionChain evolutionChain)
        {
            var chain = await CreateChainLinkEntry(evolutionChain.Chain);

            return new EvolutionChainEntry
            {
                Key = evolutionChain.Id,
                Chain = chain
            };
        }

        /// <summary>
        /// Returns the evolution chain required to create an evolution chain entry with the given ID.
        /// </summary>
        protected override async Task<EvolutionChain> FetchSource(int key)
        {
            Logger.LogInformation($"Fetching evolution chain source object with ID {key}...");
            return await _pokeApi.Get<EvolutionChain>(key);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all evolution chains.
        /// </summary>
        public async Task<EvolutionChainEntry[]> GetAll()
        {
            var allEvolutionChains = await UpsertAll();
            return allEvolutionChains.OrderBy(g => g.Id).ToArray();
        }

        /// <summary>
        /// Returns the evolution chain for the species with the given ID.
        /// </summary>
        public async Task<EvolutionChainEntry> GetBySpeciesId(int speciesId)
        {
            var species = await _pokeApi.Get<PokemonSpecies>(speciesId);
            return await Upsert(species.EvolutionChain);
        }

        /// <summary>
        /// Upserts the evolution chain from the given navigation property and returns it.
        /// </summary>
        public override async Task<EvolutionChainEntry> Upsert(UrlNavigation<EvolutionChain> evolutionChain)
        {
            var chainRes = await _pokeApi.Get(evolutionChain);
            if (chainRes.IsEmpty())
            {
                // don't bother converting if the chain is empty
                return null;
            }

            return await base.Upsert(chainRes);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns a chain link entry for the given chain link.
        /// </summary>
        private async Task<ChainLinkEntry> CreateChainLinkEntry(ChainLink chainLink)
        {
            var species = await _pokemonSpeciesService.Upsert(chainLink.Species);
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
            var item = await ItemService.Upsert(evolutionDetail.Item);
            var trigger = await EvolutionTriggerService.Upsert(evolutionDetail.Trigger);
            var heldItem = await ItemService.Upsert(evolutionDetail.HeldItem);
            var knownMove = await MoveService.Upsert(evolutionDetail.KnownMove);
            var knownMoveType = await TypeService.Upsert(evolutionDetail.KnownMoveType);
            var location = await LocationService.Upsert(evolutionDetail.Location);
            var partySpecies = await _pokemonSpeciesService.Upsert(evolutionDetail.PartySpecies);
            var partyType = await TypeService.Upsert(evolutionDetail.PartyType);
            var tradeSpecies = await _pokemonSpeciesService.Upsert(evolutionDetail.TradeSpecies);

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

        #endregion
    }
}
