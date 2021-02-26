using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a Pokemon's encounters in the data store.
    /// </summary>
    public class EncountersEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the Pokemon.
        /// </summary>
        [Required]
        public int PokemonId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the encounters indexed by version group ID.
        /// </summary>
        [Required]
        public List<WithId<List<EncounterEntry>>> Encounters { get; set; }
    }

    /// <summary>
    /// Represents an encounter in the data store.
    /// </summary>
    public class EncounterEntry
    {
        /// <summary>
        /// Gets or sets the ID of the location area of the encounter.
        /// </summary>
        [Required]
        public int LocationAreaId { get; set; }

        /// <summary>
        /// Gets or sets the display names of the encounter.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the details of the encounter indexed by version ID.
        /// </summary>
        [Required]
        public List<WithId<List<EncounterMethodDetails>>> Details { get; set; }
    }

    /// <summary>
    /// Represents details of an encounter method.
    /// </summary>
    public class EncounterMethodDetails
    {
        /// <summary>
        /// Gets or sets the encounter method.
        /// </summary>
        [Required]
        public EncounterMethodEntry Method { get; set; }

        /// <summary>
        /// Gets or sets the condition values details.
        /// </summary>
        [Required]
        public List<ConditionValuesDetail> ConditionValuesDetails { get; set; }
    }

    /// <summary>
    /// Model for a list of encounter details under a given set of conditions.
    /// </summary>
    public class ConditionValuesDetail
    {
        /// <summary>
        /// Gets or sets the condition values required for the encounters to occur.
        /// </summary>
        [Required]
        public List<EncounterConditionValueEntry> ConditionValues { get; set; }

        /// <summary>
        /// Gets or sets the encounter details.
        /// </summary>
        [Required]
        public List<EncounterDetailEntry> EncounterDetails { get; set; }
    }

    /// <summary>
    /// Model for an encounter detail.
    /// </summary>
    public class EncounterDetailEntry
    {
        /// <summary>
        /// The lowest level of the encounter detail.
        /// </summary>
        [Required]
        public int MinLevel { get; set; }

        /// <summary>
        /// The highest level of the encounter detail.
        /// </summary>
        [Required]
        public int MaxLevel { get; set; }

        /// <summary>
        /// The percent chance that this encounter will occur.
        /// </summary>
        [Required]
        public int Chance { get; set; }
    }
}
