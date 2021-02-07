using PokeApiNet;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a machine in the data store.
    /// </summary>
    public class MachineEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the machine.
        /// </summary>
        public int MachineId => Key;

        /// <summary>
        /// Gets or sets the item this machine represents.
        /// </summary>
        public Item Item { get; set; }
    }
}
