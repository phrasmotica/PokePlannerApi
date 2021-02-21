using System.Collections.Generic;

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
        public string SpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this species' front shiny sprite URL.
        /// </summary>
        public string ShinySpriteUrl { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon species' display names.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets this Pokemon species' genera.
        /// </summary>
        public List<LocalString> Genera { get; set; }

        /// <summary>
        /// Gets or sets the flavour text entries of the species, indexed by version ID.
        /// </summary>
        public List<WithId<List<LocalString>>> FlavourTextEntries { get; set; }

        /// <summary>
        /// Gets or sets the types of this species' primary variety, indexed by version group ID.
        /// </summary>
        public List<WithId<List<NamedEntryRef<TypeEntry>>>> Types { get; set; }

        /// <summary>
        /// Gets or sets the base stats of this species' primary variety, indexed by version group ID.
        /// </summary>
        public List<WithId<List<int>>> BaseStats { get; set; }

        /// <summary>
        /// Gets or sets the Pokemon this species represents.
        /// </summary>
        public List<NamedEntryRef<PokemonEntry>> Varieties { get; set; }

        /// <summary>
        /// Gets or sets the generation in which this species was introduced.
        /// </summary>
        public NamedEntryRef<GenerationEntry> Generation { get; set; }

        /// <summary>
        /// Gets or sets the species' evolution chain.
        /// </summary>
        public EntryRef<EvolutionChainEntry> EvolutionChain { get; set; }

        /// <summary>
        /// Gets or sets the IDs of the version groups where this Pokemon species is valid.
        /// TODO: change to NamedEntryRef
        /// </summary>
        public List<int> Validity { get; set; }

        /// <summary>
        /// Gets or sets the catch rate of the species.
        /// </summary>
        public int CatchRate { get; set; }

        /// <summary>
        /// Returns a reference to the Pokemon species entry.
        /// </summary>
        public NamedEntryRef<PokemonSpeciesEntry> ToRef()
        {
            return new NamedEntryRef<PokemonSpeciesEntry>
            {
                Key = PokemonSpeciesId,
                Name = Name,
            };
        }
    }
}
