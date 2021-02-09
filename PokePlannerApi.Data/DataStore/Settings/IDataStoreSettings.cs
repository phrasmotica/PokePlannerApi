namespace PokePlannerApi.Data.DataStore.Settings
{
    /// <summary>
    /// Interface for PokePlannerApi data store settings.
    /// </summary>
    public interface IDataStoreSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PrivateKey { get; set; }
    }
}