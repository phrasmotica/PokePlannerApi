using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing efficacy information.
    /// </summary>
    public class EfficacyService
    {
        // TODO: create cache service
        /// <summary>
        /// The PokeAPI data fetcher.
        /// </summary>
        protected IPokeAPI PokeApi;

        /// <summary>
        /// The Pokemon service.
        /// </summary>
        private readonly PokemonService PokemonService;

        /// <summary>
        /// The types service.
        /// </summary>
        private readonly TypeService TypesService;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<EfficacyService> Logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EfficacyService(
            IPokeAPI pokeApi,
            PokemonService pokemonService,
            TypeService typesService,
            ILogger<EfficacyService> logger)
        {
            PokeApi = pokeApi;
            PokemonService = pokemonService;
            TypesService = typesService;
            Logger = logger;
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetTypeEfficacyByTypeId(int typeId, int versionGroupId)
        {
            var entry = await TypesService.Upsert(typeId);
            return await TypesService.GetTypesEfficacySet(new[] { entry.TypeId }, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetTypeEfficacyByTypeIds(IEnumerable<int> typeIds, int versionGroupId)
        {
            var validTypeIds = ValidateTypeIds(typeIds).ToArray();
            if (validTypeIds.Length != typeIds.ToArray().Length)
            {
                var invalidTypes = typeIds.Except(validTypeIds).ToArray();
                var invalidTypesStr = string.Join(", ", invalidTypes);
                Logger.LogWarning($"Attempted to get type efficacy for {invalidTypes.Length} invalid type(s): {invalidTypesStr}");
            }

            return await TypesService.GetTypesEfficacySet(validTypeIds, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetTypeEfficacyByPokemonId(int pokemonId, int versionGroupId)
        {
            var pokemon = await PokemonService.Upsert(pokemonId);
            return await TypesService.GetTypesEfficacySet(pokemon.Types.Select(t => t.Id), versionGroupId);
        }

        /// <summary>
        /// Returns all valid type IDs from the given list.
        /// </summary>
        private IEnumerable<int> ValidateTypeIds(IEnumerable<int> typeIds)
        {
            return typeIds.Where(id => id != 0);
        }
    }
}
