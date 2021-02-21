namespace PokePlannerApi.Data.DataStore.Settings
{
    /// <summary>
    /// Class for PokePlannerApi data store settings.
    /// </summary>
    public class DataStoreSettings
    {
        public string ConnectionString { get; set; }
        public string PrivateKey { get; set; }
        public string DatabaseName { get; set; }
        public CollectionSettings CollectionSettings { get; set; }
    }
}
