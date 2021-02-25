﻿using System.Collections.Generic;
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
        public string SpriteUrl { get; set; } = default!;

        /// <summary>
        /// Gets or sets this Pokemon's front shiny sprite URL.
        /// </summary>
        public string ShinySpriteUrl { get; set; } = default!;

        /// <summary>
        /// Gets or sets the display names of this Pokemon's primary form.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets this Pokemon's forms.
        /// </summary>
        public List<PokemonFormEntry> Forms { get; set; } = new List<PokemonFormEntry>();

        /// <summary>
        /// Gets or sets this Pokemon's types, indexed by version group ID.
        /// </summary>
        public List<WithId<List<TypeEntry>>> Types { get; set; } = new List<WithId<List<TypeEntry>>>();

        /// <summary>
        /// Gets or sets this Pokemon's abilities.
        /// </summary>
        public List<AbilityEntry> Abilities { get; set; } = new List<AbilityEntry>();

        /// <summary>
        /// Gets or sets this Pokemon's base stats indexed by version group ID.
        /// </summary>
        public List<WithId<List<int>>> BaseStats { get; set; } = new List<WithId<List<int>>>();

        /// <summary>
        /// Gets or sets this Pokemon's moves indexed by version group ID.
        /// </summary>
        public List<WithId<List<MoveEntry>>> Moves { get; set; } = new List<WithId<List<MoveEntry>>>();

        /// <summary>
        /// Gets or sets the held items this Pokemon may bear in a wild encounter, indexed by version ID.
        /// </summary>
        public List<WithId<List<VersionHeldItemContext>>> HeldItems { get; set; } = new List<WithId<List<VersionHeldItemContext>>>();

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
