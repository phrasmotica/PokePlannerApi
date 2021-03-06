﻿using System.Collections.Generic;
using System.Linq;
using PokeApiNet;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a Pokemon form in the data store.
    /// </summary>
    public class PokemonFormEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the Pokemon form.
        /// </summary>
        public int FormId => Key;

        /// <summary>
        /// Gets or sets the form name of the Pokemon form.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets whether the Pokemon form is a mega evolution.
        /// </summary>
        public bool IsMega { get; set; }

        /// <summary>
        /// Gets or sets the version group in which this form was introduced.
        /// </summary>
        public VersionGroup VersionGroup { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon form's display names.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon form's front default sprite URL.
        /// </summary>
        public string SpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon form's front shiny sprite URL.
        /// </summary>
        public string ShinySpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon form's types indexed by version group ID.
        /// </summary>
        public List<WithId<Type[]>> Types { get; set; }

        /// <summary>
        /// Gets or sets the IDs of the version groups where this Pokemon form is valid.
        /// </summary>
        public List<int> Validity { get; set; }
    }
}
