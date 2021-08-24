namespace PokePlannerApi.Settings
{
    /// <summary>
    /// Class for PokeAPI settings.
    /// </summary>
    public class PokeApiSettings
    {
        public string BaseUri { get; set; }
        public string GraphQlUri { get; set; }
        public int GraphQlClientCacheEntryLifetimeDays { get; set; }
    }
}
