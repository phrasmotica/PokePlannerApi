namespace PokePlannerApi.Data.DataStore
{
    /// <summary>
    /// Represents a localised string.
    /// </summary>
    public class LocalString
    {
        /// <summary>
        /// The language of the string.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The string to display.
        /// </summary>
        public string Value { get; set; }
    }
}
