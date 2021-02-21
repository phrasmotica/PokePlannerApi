﻿using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a version in the data store.
    /// </summary>
    public class VersionEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the version.
        /// </summary>
        public int VersionId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the version.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the version entry.
        /// </summary>
        public EntryRef<VersionEntry> ToRef()
        {
            return new EntryRef<VersionEntry>
            {
                Key = VersionId,
                Name = Name,
            };
        }
    }
}
