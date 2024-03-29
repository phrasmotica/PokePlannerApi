﻿using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move damage class in the data store.
    /// </summary>
    public class MoveDamageClassEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move damage class.
        /// </summary>
        public int MoveDamageClassId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move damage class.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
