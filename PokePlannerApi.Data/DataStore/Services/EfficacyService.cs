using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing efficacy information.
    /// </summary>
    public class EfficacyService
    {
        private readonly PokemonService _pokemonService;
        private readonly TypeService _typeService;
        private readonly ILogger<EfficacyService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EfficacyService(
            PokemonService pokemonService,
            TypeService typesService,
            ILogger<EfficacyService> logger)
        {
            _pokemonService = pokemonService;
            _typeService = typesService;
            _logger = logger;
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetTypeEfficacyByTypeId(int typeId, int versionGroupId)
        {
            var entry = await _typeService.Get(typeId);
            return await _typeService.GetTypesEfficacySet(new[] { entry.TypeId }, versionGroupId);
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
                _logger.LogWarning($"Attempted to get type efficacy for {invalidTypes.Length} invalid type(s): {invalidTypesStr}");
            }

            return await _typeService.GetTypesEfficacySet(validTypeIds, versionGroupId);
        }

        /// <summary>
        /// Returns the efficacy of the Pokemon with the given ID in the version group with the
        /// given ID.
        /// </summary>
        public async Task<EfficacySet> GetTypeEfficacyByPokemonId(int pokemonId, int versionGroupId)
        {
            var pokemon = await _pokemonService.Get(pokemonId);
            return await _typeService.GetTypesEfficacySet(pokemon.Types.Select(t => t.Id), versionGroupId);
        }

        /// <summary>
        /// Returns all valid type IDs from the given list.
        /// </summary>
        private static IEnumerable<int> ValidateTypeIds(IEnumerable<int> typeIds)
        {
            return typeIds.Where(id => id != 0);
        }
    }
}
