using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move in the data store.
    /// </summary>
    public class MoveEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the move.
        /// </summary>
        public int MoveId => Key;

        /// <summary>
        /// Gets or sets the display names of the move.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the flavour text entries of the move, indexed by version group ID.
        /// </summary>
        public List<WithId<LocalString[]>> FlavourTextEntries { get; set; }

        /// <summary>
        /// Gets or sets the type of the move.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the category of the move.
        /// </summary>
        public MoveCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the move's base power.
        /// </summary>
        public int? Power { get; set; }

        /// <summary>
        /// Gets or sets the damage class of the move.
        /// </summary>
        public MoveDamageClass DamageClass { get; set; }

        /// <summary>
        /// Gets or sets the move's accuracy.
        /// </summary>
        public int? Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the move's max number of power points.
        /// </summary>
        public int? PP { get; set; }

        /// <summary>
        /// Gets or sets the move's priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the move's target.
        /// </summary>
        public MoveTarget Target { get; set; }

        /// <summary>
        /// Gets or sets the machines that teach the move, indexed by version group ID.
        /// </summary>
        public List<WithId<Machine[]>> Machines { get; set; }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public MoveEntry ForEvolutionChain()
        {
            return new MoveEntry
            {
                Key = Key,
                Name = Name,
                DisplayNames = DisplayNames
            };
        }
    }

    /// <summary>
    /// Move info plus learn methods for some Pokemon.
    /// </summary>
    public class PokemonMoveContext : MoveEntry
    {
        /// <summary>
        /// Gets or sets the level at which the move is learnt, if applicable.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the machines that teach the move, if applicable.
        /// </summary>
        public List<ItemEntry> LearnMachines { get; set; }

        /// <summary>
        /// Gets or sets the methods by which the move is learnt.
        /// </summary>
        public List<MoveLearnMethodEntry> Methods { get; set; }

        /// <summary>
        /// Converts an move entry into an move context instance.
        /// </summary>
        public static PokemonMoveContext From(MoveEntry e)
        {
            return new PokemonMoveContext
            {
                Key = e.Key,
                Name = e.Name,
                DisplayNames = e.DisplayNames,
                FlavourTextEntries = e.FlavourTextEntries,
                Type = e.Type,
                Category = e.Category,
                Power = e.Power,
                DamageClass = e.DamageClass,
                Accuracy = e.Accuracy,
                PP = e.PP,
                Priority = e.Priority,
                Target = e.Target
            };
        }
    }
}
