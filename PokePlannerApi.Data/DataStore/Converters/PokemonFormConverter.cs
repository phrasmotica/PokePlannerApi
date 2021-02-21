using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class PokemonFormConverter : IResourceConverter<PokemonForm, PokemonFormEntry>
    {
        private readonly VersionGroupService _versionGroupService;
        private readonly ILogger<PokemonFormConverter> _logger;

        public PokemonFormConverter(
            VersionGroupService versionGroupService,
            ILogger<PokemonFormConverter> logger)
        {
            _versionGroupService = versionGroupService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<PokemonFormEntry> Convert(PokemonForm resource)
        {
            var versionGroup = await _versionGroupService.Get(resource.VersionGroup);
            var validity = await GetValidity(resource);

            return new PokemonFormEntry
            {
                PokemonFormId = resource.Id,
                Name = resource.Name,
                FormName = resource.FormName,
                IsMega = resource.IsMega,
                VersionGroup = versionGroup.ToRef(),
                DisplayNames = resource.Names.Localise().ToList(),
                SpriteUrl = GetSpriteUrl(resource),
                ShinySpriteUrl = GetShinySpriteUrl(resource),
                Types = new List<WithId<List<EntryRef<TypeEntry>>>>(),
                Validity = validity.ToList()
            };
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of this Pokemon.
        /// </summary>
        private string GetSpriteUrl(PokemonForm pokemonForm)
        {
            var frontDefaultUrl = pokemonForm.Sprites.FrontDefault;
            if (frontDefaultUrl == null)
            {
                _logger.LogWarning($"Pokemon form {pokemonForm.Id} is missing front default sprite");
            }

            return frontDefaultUrl;
        }

        /// <summary>
        /// Returns the URL of the shiny sprite of this Pokemon.
        /// </summary>
        private string GetShinySpriteUrl(PokemonForm pokemonForm)
        {
            var frontShinyUrl = pokemonForm.Sprites.FrontShiny;
            if (frontShinyUrl == null)
            {
                _logger.LogWarning($"Pokemon form {pokemonForm.Id} is missing front shiny sprite");
            }

            return frontShinyUrl;
        }

        /// <summary>
        /// Returns the given Pokemon form's validity in all version groups.
        /// </summary>
        private async Task<IEnumerable<int>> GetValidity(PokemonForm pokemonForm)
        {
            var validityList = new List<int>();

            foreach (var vg in await _versionGroupService.GetAll())
            {
                var isValid = await IsValid(pokemonForm, vg);
                if (isValid)
                {
                    // form is only valid if the version group's ID is in the list
                    validityList.Add(vg.VersionGroupId);
                }
            }

            return validityList;
        }

        /// <summary>
        /// Returns true if the given Pokemon form can be obtained in the given version group.
        /// </summary>
        private async Task<bool> IsValid(PokemonForm pokemonForm, VersionGroupEntry versionGroup)
        {
            if (pokemonForm.IsMega)
            {
                // decide based on version group in which it was introduced
                var formVersionGroup = await _versionGroupService.Get(pokemonForm.VersionGroup);
                return formVersionGroup.Order <= versionGroup.Order;
            }

            return false;
        }
    }
}
