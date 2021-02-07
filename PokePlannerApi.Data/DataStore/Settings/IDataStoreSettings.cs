namespace PokePlannerApi.Data.DataStore.Settings
{
    /// <summary>
    /// Interface for PokePlannerApi data store settings.
    /// </summary>
    public interface IDataStoreSettings
    {
        /// <summary>
        /// The name of the collection of abilities.
        /// </summary>
        string AbilityCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of encounter conditions.
        /// </summary>
        string EncounterConditionCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of encounter condition values.
        /// </summary>
        string EncounterConditionValueCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of encounters.
        /// </summary>
        string EncounterCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of encounter methods.
        /// </summary>
        string EncounterMethodCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of evolution chains.
        /// </summary>
        string EvolutionChainCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of evolution triggers.
        /// </summary>
        string EvolutionTriggerCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of generations.
        /// </summary>
        string GenerationCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of items.
        /// </summary>
        string ItemCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of locations.
        /// </summary>
        string LocationCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of location areas.
        /// </summary>
        string LocationAreaCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of machines.
        /// </summary>
        string MachineCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of move categories.
        /// </summary>
        string MoveCategoryCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of moves.
        /// </summary>
        string MoveCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of move damage classes.
        /// </summary>
        string MoveDamageClassCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of move learn methods.
        /// </summary>
        string MoveLearnMethodCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of move targets.
        /// </summary>
        string MoveTargetCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of pokedexes.
        /// </summary>
        string PokedexCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of Pokemon.
        /// </summary>
        string PokemonCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of Pokemon forms.
        /// </summary>
        string PokemonFormCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of Pokemon species.
        /// </summary>
        string PokemonSpeciesCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of stats.
        /// </summary>
        string StatCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of types.
        /// </summary>
        string TypeCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of versions.
        /// </summary>
        string VersionCollectionName { get; set; }

        /// <summary>
        /// The name of the collection of version groups.
        /// </summary>
        string VersionGroupCollectionName { get; set; }
    }
}
