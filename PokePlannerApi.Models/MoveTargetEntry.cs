﻿using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move target in the data store.
    /// </summary>
    public class MoveTargetEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the move target.
        /// </summary>
        public int MoveTargetId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move target.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the move target entry.
        /// </summary>
        public NamedEntryRef<MoveTargetEntry> ToRef()
        {
            return new NamedEntryRef<MoveTargetEntry>
            {
                Key = MoveTargetId,
                Name = Name,
            };
        }
    }
}
