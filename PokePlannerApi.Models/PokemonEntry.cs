using System.Collections.Generic;

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
        public string SpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's front shiny sprite URL.
        /// </summary>
        public string ShinySpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets the display names of this Pokemon's primary form.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's forms.
        /// </summary>
        public List<NamedEntryRef<PokemonFormEntry>> Forms { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's types, indexed by version group ID.
        /// </summary>
        public List<WithId<List<NamedEntryRef<TypeEntry>>>> Types { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's abilities.
        /// </summary>
        public List<NamedEntryRef<AbilityEntry>> Abilities { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's base stats indexed by version group ID.
        /// </summary>
        public List<WithId<List<int>>> BaseStats { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's moves indexed by version group ID.
        /// </summary>
        public List<WithId<List<NamedEntryRef<MoveEntry>>>> Moves { get; set; }

        /// <summary>
        /// Gets or sets the held items this Pokemon may bear in a wild encounter, indexed by version ID.
        /// </summary>
        public List<WithId<List<VersionHeldItemContext>>> HeldItems { get; set; }

        /// <summary>
        /// Returns a reference to the Pokemon entry.
        /// </summary>
        public NamedEntryRef<PokemonEntry> ToRef()
        {
            return new NamedEntryRef<PokemonEntry>
            {
                Key = PokemonId,
                Name = Name,
            };
        }
    }
}
