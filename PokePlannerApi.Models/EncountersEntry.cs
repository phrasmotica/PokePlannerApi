﻿using System.Collections.Generic;

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
        public int PokemonId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the encounters indexed by version group ID.
        /// </summary>
        public List<WithId<List<EncounterEntry>>> Encounters { get; set; } = new List<WithId<List<EncounterEntry>>>();
    }

    /// <summary>
    /// Represents an encounter in the data store.
    /// </summary>
    public class EncounterEntry
    {
        /// <summary>
        /// Gets or sets the ID of the location area of the encounter.
        /// </summary>
        public int LocationAreaId { get; set; }

        /// <summary>
        /// Gets or sets the display names of the encounter.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets the details of the encounter indexed by version ID.
        /// </summary>
        public List<WithId<List<EncounterMethodDetails>>> Details { get; set; } = new List<WithId<List<EncounterMethodDetails>>>();
    }

    /// <summary>
    /// Represents details of an encounter method.
    /// </summary>
    public class EncounterMethodDetails
    {
        /// <summary>
        /// Gets or sets the encounter method.
        /// </summary>
        public EncounterMethodEntry Method { get; set; } = default!;

        /// <summary>
        /// Gets or sets the condition values details.
        /// </summary>
        public List<ConditionValuesDetail> ConditionValuesDetails { get; set; } = new List<ConditionValuesDetail>();
    }

    /// <summary>
    /// Model for a list of encounter details under a given set of conditions.
    /// </summary>
    public class ConditionValuesDetail
    {
        /// <summary>
        /// Gets or sets the condition values required for the encounters to occur.
        /// </summary>
        public List<EncounterConditionValueEntry> ConditionValues { get; set; } = new List<EncounterConditionValueEntry>();

        /// <summary>
        /// Gets or sets the encounter details.
        /// </summary>
        public List<EncounterDetailEntry> EncounterDetails { get; set; } = new List<EncounterDetailEntry>();
    }

    /// <summary>
    /// Model for an encounter detail.
    /// </summary>
    public class EncounterDetailEntry
    {
        /// <summary>
        /// The lowest level of the encounter detail.
        /// </summary>
        public int MinLevel { get; set; }

        /// <summary>
        /// The highest level of the encounter detail.
        /// </summary>
        public int MaxLevel { get; set; }

        /// <summary>
        /// The percent chance that this encounter will occur.
        /// </summary>
        public int Chance { get; set; }
    }
}
