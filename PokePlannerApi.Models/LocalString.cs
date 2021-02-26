using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a localised string.
    /// </summary>
    public class LocalString
    {
        /// <summary>
        /// The language of the string.
        /// </summary>
        [Required]
        public string Language { get; set; }

        /// <summary>
        /// The string to display.
        /// </summary>
        [Required]
        public string Value { get; set; }
    }
}
