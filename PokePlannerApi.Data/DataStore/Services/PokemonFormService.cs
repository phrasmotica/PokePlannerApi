using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the Pokemon forms entries in the data store.
    /// </summary>
    public class PokemonFormService : NamedApiResourceServiceBase<PokemonForm, PokemonFormEntry>
    {
        /// <summary>
        /// The version group service.
        /// </summary>
        private readonly VersionGroupService VersionGroupService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonFormService(
            IDataStoreSource<PokemonFormEntry> dataStoreSource,
            IPokeAPI pokeApi,
            VersionGroupService versionGroupService,
            ILogger<PokemonFormService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            VersionGroupService = versionGroupService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a Pokemon form entry for the given Pokemon form.
        /// </summary>
        protected override async Task<PokemonFormEntry> ConvertToEntry(PokemonForm pokemonForm)
        {
            var versionGroup = await VersionGroupService.Upsert(pokemonForm.VersionGroup);
            var types = await GetTypes(pokemonForm);
            var validity = await GetValidity(pokemonForm);

            return new PokemonFormEntry
            {
                Key = pokemonForm.Id,
                Name = pokemonForm.Name,
                FormName = pokemonForm.FormName,
                IsMega = pokemonForm.IsMega,
                VersionGroup = new VersionGroup
                {
                    Id = versionGroup.VersionGroupId,
                    Name = versionGroup.Name
                },
                DisplayNames = pokemonForm.Names.Localise().ToList(),
                SpriteUrl = GetSpriteUrl(pokemonForm),
                ShinySpriteUrl = GetShinySpriteUrl(pokemonForm),
                Types = types.ToList(),
                Validity = validity.ToList()
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the Pokemon form with the given ID from the data store.
        /// </summary>
        public async Task<PokemonFormEntry> GetPokemonForm(int formId)
        {
            return await Upsert(formId);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the URL of the shiny sprite of this Pokemon.
        /// </summary>
        private string GetSpriteUrl(PokemonForm pokemonForm)
        {
            var frontDefaultUrl = pokemonForm.Sprites.FrontDefault;
            if (frontDefaultUrl == null)
            {
                Logger.LogWarning($"Pokemon form {pokemonForm.Id} is missing front default sprite");
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
                Logger.LogWarning($"Pokemon form {pokemonForm.Id} is missing front shiny sprite");
            }

            return frontShinyUrl;
        }

        /// <summary>
        /// Returns the given Pokemon form's types.
        /// </summary>
        private async Task<IEnumerable<WithId<Type[]>>> GetTypes(PokemonForm form)
        {
            var typesList = new List<WithId<Type[]>>();

            // always include the newest types
            var newestId = await VersionGroupService.GetNewestVersionGroupId();
            var newestTypeEntries = await MinimiseTypes(form.Types);
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
        /// Returns the given Pokemon form's validity in all version groups.
        /// </summary>
        private async Task<IEnumerable<int>> GetValidity(PokemonForm pokemonForm)
        {
            var validityList = new List<int>();

            foreach (var vg in await VersionGroupService.GetAll())
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
                var formVersionGroup = await VersionGroupService.Upsert(pokemonForm.VersionGroup);
                return formVersionGroup.Order <= versionGroup.Order;
            }

            return false;
        }

        #endregion
    }
}
