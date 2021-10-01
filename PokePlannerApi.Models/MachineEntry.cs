using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a machine in the data store.
    /// </summary>
    public class MachineEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the machine.
        /// </summary>
        public int MachineId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the item this machine represents.
        /// </summary>
        [Required]
        public ItemEntry Item { get; set; }
    }
}
