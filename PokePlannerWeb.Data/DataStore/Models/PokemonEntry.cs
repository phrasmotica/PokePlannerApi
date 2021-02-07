using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a Pokemon in the data store.
    /// </summary>
    public class PokemonEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the Pokemon.
        /// </summary>
        public int PokemonId => Key;

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
        public List<PokemonForm> Forms { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's types indexed by version group ID.
        /// </summary>
        public List<WithId<Type[]>> Types { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's abilities.
        /// </summary>
        public List<Ability> Abilities { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's base stats indexed by version group ID.
        /// </summary>
        public List<WithId<int[]>> BaseStats { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon's moves indexed by version group ID.
        /// </summary>
        public List<WithId<Move[]>> Moves { get; set; }

        /// <summary>
        /// Gets or sets the held items this Pokemon may bear in a wild encounter, indexed by version ID.
        /// </summary>
        public List<WithId<VersionHeldItemContext[]>> HeldItems { get; set; }
    }
}
