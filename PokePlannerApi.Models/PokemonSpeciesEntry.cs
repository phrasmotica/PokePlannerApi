using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a Pokemon species in the data store.
    /// </summary>
    public class PokemonSpeciesEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the Pokemon species.
        /// </summary>
        public int PokemonSpeciesId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets this species' front default sprite URL.
        /// </summary>
        public string SpriteUrl { get; set; } = default!;

        /// <summary>
        /// Gets or sets this species' front shiny sprite URL.
        /// </summary>
        public string ShinySpriteUrl { get; set; } = default!;

        /// <summary>
        /// Gets or sets this Pokemon species' display names.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets this Pokemon species' genera.
        /// </summary>
        public List<LocalString> Genera { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets the flavour text entries of the species, indexed by version ID.
        /// </summary>
        public List<WithId<List<LocalString>>> FlavourTextEntries { get; set; } = new List<WithId<List<LocalString>>>();

        /// <summary>
        /// Gets or sets the types of this species' primary variety, indexed by version group ID.
        /// </summary>
        public List<WithId<List<TypeEntry>>> Types { get; set; } = new List<WithId<List<TypeEntry>>>();

        /// <summary>
        /// Gets or sets the base stats of this species' primary variety, indexed by version group ID.
        /// </summary>
        public List<WithId<List<int>>> BaseStats { get; set; } = new List<WithId<List<int>>>();

        /// <summary>
        /// Gets or sets the Pokemon this species represents.
        /// </summary>
        public List<PokemonEntry> Varieties { get; set; } = new List<PokemonEntry>();

        /// <summary>
        /// Gets or sets the generation in which this species was introduced.
        /// </summary>
        public GenerationEntry Generation { get; set; } = default!;

        /// <summary>
        /// Gets or sets the species' evolution chain.
        /// </summary>
        public EvolutionChain EvolutionChain { get; set; } = default!;

        /// <summary>
        /// Gets or sets the IDs of the version groups where this Pokemon species is valid.
        /// </summary>
        public List<int> Validity { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the catch rate of the species.
        /// </summary>
        public int CatchRate { get; set; }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public PokemonSpeciesEntry ForEvolutionChain()
        {
            return new PokemonSpeciesEntry
            {
                Key = Key,
                Name = Name,
                SpriteUrl = SpriteUrl,
                ShinySpriteUrl = ShinySpriteUrl,
                DisplayNames = DisplayNames,
                Generation = Generation,
                Validity = Validity
            };
        }
    }
}
