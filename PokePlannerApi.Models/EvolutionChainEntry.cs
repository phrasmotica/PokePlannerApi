using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a evolution chain in the data store.
    /// </summary>
    public class EvolutionChainEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the evolution chain.
        /// </summary>
        public int EvolutionChainId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the evolution chain link.
        /// </summary>
        public ChainLinkEntry Chain { get; set; }

        /// <summary>
        /// Returns a reference to the evolution chain entry.
        /// </summary>
        public EntryRef<EvolutionChainEntry> ToRef()
        {
            return new EntryRef<EvolutionChainEntry>
            {
                Key = EvolutionChainId,
            };
        }
    }

    /// <summary>
    /// Represents a link to a species as part of an evolution chain.
    /// </summary>
    public class ChainLinkEntry
    {
        /// <summary>
        /// Whether this link is for a baby Pokemon. Only ever true on the base link.
        /// </summary>
        public bool IsBaby { get; set; }

        /// <summary>
        /// The Pokemon species at this stage of the evolution chain.
        /// </summary>
        public NamedEntryRef<PokemonSpeciesEntry> Species { get; set; }

        /// <summary>
        /// All details regarding the specific details of the referenced species evolution.
        /// </summary>
        public List<EvolutionDetailEntry> EvolutionDetails { get; set; }

        /// <summary>
        /// A list of chain objects.
        /// </summary>
        public List<ChainLinkEntry> EvolvesTo { get; set; }
    }

    /// <summary>
    /// Represents details of how a species evolves into another.
    /// </summary>
    public class EvolutionDetailEntry
    {
        /// <summary>
        /// Gets or sets the item required to cause evolution into this species.
        /// </summary>
        public NamedEntryRef<ItemEntry> Item { get; set; }

        /// <summary>
        /// Gets or sets the type of event that triggers evolution into this species.
        /// </summary>
        public NamedEntryRef<EvolutionTriggerEntry> Trigger { get; set; }

        /// <summary>
        /// Gets or sets the ID of the gender of the evolving Pokémon species must be in order to
        /// evolve into this species.
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Gets or sets the item the evolving species must be holding during the evolution trigger
        /// event to evolve into this species.
        /// </summary>
        public NamedEntryRef<ItemEntry> HeldItem { get; set; }

        /// <summary>
        /// Gets or sets the move that must be known by the evolving species during the evolution
        /// trigger event in order to evolve into this species.
        /// </summary>
        public NamedEntryRef<MoveEntry> KnownMove { get; set; }

        /// <summary>
        /// Gets or sets the evolving species must know a move with this type during the evolution
        /// trigger event in order to evolve into this species.
        /// </summary>
        public NamedEntryRef<TypeEntry> KnownMoveType { get; set; }

        /// <summary>
        /// Gets or sets the location the evolution must be triggered at.
        /// </summary>
        public NamedEntryRef<LocationEntry> Location { get; set; }

        /// <summary>
        /// Gets or sets the minimum required level of the evolving Pokémon species to evolve into
        /// this species.
        /// </summary>
        public int? MinLevel { get; set; }

        /// <summary>
        /// Gets or sets the minimum required level of happiness the evolving species to evolve into
        /// this species.
        /// </summary>
        public int? MinHappiness { get; set; }

        /// <summary>
        /// Gets or sets the minimum required level of beauty the evolving species to evolve into
        /// this species.
        /// </summary>
        public int? MinBeauty { get; set; }

        /// <summary>
        /// Gets or sets the minimum required level of affection the evolving species to evolve into
        /// this species.
        /// </summary>
        public int? MinAffection { get; set; }

        /// <summary>
        /// Gets or sets whether or not it must be raining in the overworld to cause
        /// evolution into this species.
        /// </summary>
        public bool NeedsOverworldRain { get; set; }

        /// <summary>
        /// Gets or sets the species that must be in the players party in order for the evolving
        /// species to evolve into this species.
        /// </summary>
        public NamedEntryRef<PokemonSpeciesEntry> PartySpecies { get; set; }

        /// <summary>
        /// Gets or sets the type of Pokemon the player must have in their party during the
        /// evolution trigger event in order for the evolving species to evolve into this species.
        /// </summary>
        public NamedEntryRef<TypeEntry> PartyType { get; set; }

        /// <summary>
        /// Gets or sets the required relation between the Pokémon's Attack and Defense stats:
        /// 1 means Attack > Defense
        /// 0 means Attack = Defense
        /// -1 means Attack < Defense
        /// </summary>
        public int? RelativePhysicalStats { get; set; }

        /// <summary>
        /// Gets or sets the required time of day: day or night.
        /// </summary>
        public string TimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the species for which this one must be traded.
        /// </summary>
        public NamedEntryRef<PokemonSpeciesEntry> TradeSpecies { get; set; }

        /// <summary>
        /// Gets or sets whether or not the 3DS needs to be turned upside-down as this Pokemon
        /// levels up.
        /// </summary>
        public bool TurnUpsideDown { get; set; }
    }
}
