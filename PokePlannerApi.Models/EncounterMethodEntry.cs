﻿using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an encounter method in the data store.
    /// </summary>
    public class EncounterMethodEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the encounter method.
        /// </summary>
        public int EncounterMethodId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the order of the encounter method.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display names of the encounter method.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the encounter method entry.
        /// </summary>
        public NamedEntryRef<EncounterMethodEntry> ToRef()
        {
            return new NamedEntryRef<EncounterMethodEntry>
            {
                Key = EncounterMethodId,
                Name = Name,
            };
        }
    }
}
