﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a Pokemon in the data store.
    /// </summary>
    public class PokemonEntry : EntryBase
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
        /// Gets or sets this Pokemon's front default sprite URL.
        /// </summary>
        [Required]
        public string SpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's front shiny sprite URL.
        /// </summary>
        [Required]
        public string ShinySpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets the display names of this Pokemon's primary form.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's forms.
        /// TODO: convert to list of int IDs
        /// </summary>
        public List<PokemonFormEntry> Forms { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's types, indexed by version group ID.
        /// </summary>
        public List<WithId<List<TypeEntry>>> Types { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's abilities.
        /// TODO: convert to list of int IDs
        /// </summary>
        public List<AbilityEntry> Abilities { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's base stats indexed by version group ID.
        /// </summary>
        public List<WithId<List<int>>> BaseStats { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's moves indexed by version group ID.
        /// TODO: convert to list of int IDs
        /// </summary>
        public List<WithId<List<MoveEntry>>> Moves { get; set; }

        /// <summary>
        /// Gets or sets the held items this Pokemon may bear in a wild encounter, indexed by version ID.
        /// </summary>
        public List<WithId<List<VersionHeldItemContext>>> HeldItems { get; set; }

        /// <summary>
        /// Returns the Pokemon's types in the version group with the given ID.
        /// </summary>
        public List<TypeEntry> GetTypes(int versionGroupId)
        {
            return Types.OrderBy(e => e.Id)
                        .Where(e => e.Id >= versionGroupId)
                        .First()
                        .Data;
        }
    }
}
