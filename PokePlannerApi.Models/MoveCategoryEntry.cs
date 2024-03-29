﻿using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move category in the data store.
    /// </summary>
    public class MoveCategoryEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move category.
        /// </summary>
        public int MoveCategoryId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the descriptions of the move category.
        /// </summary>
        public List<LocalString> Descriptions { get; set; }
    }
}
