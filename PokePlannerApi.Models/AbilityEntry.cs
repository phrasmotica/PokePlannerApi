using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an ability in the data store.
    /// </summary>
    public class AbilityEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the ability.
        /// </summary>
        [Required]
        public int AbilityId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the ability.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the flavour text entries of the ability, indexed by version group ID.
        /// </summary>
        [Required]
        public List<WithId<List<LocalString>>> FlavourTextEntries { get; set; }
    }

    /// <summary>
    /// Ability info plus whether it's a hidden ability for some Pokemon.
    /// </summary>
    public class PokemonAbilityContext : AbilityEntry
    {
        /// <summary>
        /// Gets or sets whether the ability is hidden.
        /// </summary>
        [Required]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Converts an ability entry into an ability context instance.
        /// </summary>
        public static PokemonAbilityContext From(AbilityEntry e)
        {
            return new PokemonAbilityContext
            {
                AbilityId = e.AbilityId,
                DisplayNames = e.DisplayNames,
                FlavourTextEntries = e.FlavourTextEntries
            };
        }
    }
}
