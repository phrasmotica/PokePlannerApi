using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PokeApiNet;
using PokePlannerWeb.Data;
using PokePlannerWeb.Data.Cache.Abstractions;
using PokePlannerWeb.Data.Cache.Services;
using PokePlannerWeb.Data.Cache.Settings;
using PokePlannerWeb.Data.DataStore.Abstractions;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;
using PokePlannerWeb.Data.DataStore.Settings;

namespace PokePlannerWeb.Tests
{
    /// <summary>
    /// Helper class for writing tests.
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// The Configuration.
        /// </summary>
        private static IConfiguration Configuration;

        /// <summary>
        /// Creates a service provider for tests.
        /// </summary>
        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

            // configure PokeAPI services
            services.AddSingleton<PokeApiClient>();
            services.AddSingleton<ILogger<PokeAPI>, NullLogger<PokeAPI>>();
            services.AddSingleton<IPokeAPI, PokeAPI>();

            ConfigureDataStore(services);

            ConfigureCache(services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures services for accessing the data store.
        /// </summary>
        private static void ConfigureDataStore(IServiceCollection services)
        {
            // bind data store settings
            services.Configure<DataStoreSettings>(
                Configuration.GetSection(nameof(DataStoreSettings))
            );

            // create singleton for data store settings
            services.AddSingleton<IDataStoreSettings>(sp =>
                sp.GetRequiredService<IOptions<DataStoreSettings>>().Value
            );

            // create data store services
            var dataStoreSourceFactory = new DataStoreSourceFactory();
            var dataStoreSettings = Configuration.GetSection(nameof(DataStoreSettings)).Get<DataStoreSettings>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<StatEntry>(dataStoreSettings.StatCollectionName)
            );
            services.AddSingleton<ILogger<StatService>, NullLogger<StatService>>();
            services.AddSingleton<StatService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<GenerationEntry>(dataStoreSettings.GenerationCollectionName)
            );
            services.AddSingleton<ILogger<GenerationService>, NullLogger<GenerationService>>();
            services.AddSingleton<GenerationService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokedexEntry>(dataStoreSettings.PokedexCollectionName)
            );
            services.AddSingleton<ILogger<PokedexService>, NullLogger<PokedexService>>();
            services.AddSingleton<PokedexService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionEntry>(dataStoreSettings.VersionCollectionName)
            );
            services.AddSingleton<ILogger<VersionService>, NullLogger<VersionService>>();
            services.AddSingleton<VersionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionGroupEntry>(dataStoreSettings.VersionGroupCollectionName)
            );
            services.AddSingleton<ILogger<VersionGroupService>, NullLogger<VersionGroupService>>();
            services.AddSingleton<VersionGroupService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<TypeEntry>(dataStoreSettings.TypeCollectionName)
            );
            services.AddSingleton<ILogger<TypeService>, NullLogger<TypeService>>();
            services.AddSingleton<TypeService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveCategoryEntry>(dataStoreSettings.MoveCategoryCollectionName)
            );
            services.AddSingleton<ILogger<MoveCategoryService>, NullLogger<MoveCategoryService>>();
            services.AddSingleton<MoveCategoryService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveDamageClassEntry>(dataStoreSettings.MoveDamageClassCollectionName)
            );
            services.AddSingleton<ILogger<MoveDamageClassService>, NullLogger<MoveDamageClassService>>();
            services.AddSingleton<MoveDamageClassService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveEntry>(dataStoreSettings.MoveCollectionName)
            );
            services.AddSingleton<ILogger<MoveService>, NullLogger<MoveService>>();
            services.AddSingleton<MoveService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveTargetEntry>(dataStoreSettings.MoveTargetCollectionName)
            );
            services.AddSingleton<ILogger<MoveTargetService>, NullLogger<MoveTargetService>>();
            services.AddSingleton<MoveTargetService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionChainEntry>(dataStoreSettings.EvolutionChainCollectionName)
            );
            services.AddSingleton<ILogger<EvolutionChainService>, NullLogger<EvolutionChainService>>();
            services.AddSingleton<EvolutionChainService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionTriggerEntry>(dataStoreSettings.EvolutionTriggerCollectionName)
            );
            services.AddSingleton<ILogger<EvolutionTriggerService>, NullLogger<EvolutionTriggerService>>();
            services.AddSingleton<EvolutionTriggerService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<ItemEntry>(dataStoreSettings.ItemCollectionName)
            );
            services.AddSingleton<ILogger<ItemService>, NullLogger<ItemService>>();
            services.AddSingleton<ItemService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<AbilityEntry>(dataStoreSettings.AbilityCollectionName)
            );
            services.AddSingleton<ILogger<AbilityService>, NullLogger<AbilityService>>();
            services.AddSingleton<AbilityService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveLearnMethodEntry>(dataStoreSettings.MoveLearnMethodCollectionName)
            );
            services.AddSingleton<ILogger<MoveLearnMethodService>, NullLogger<MoveLearnMethodService>>();
            services.AddSingleton<MoveLearnMethodService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MachineEntry>(dataStoreSettings.MachineCollectionName)
            );
            services.AddSingleton<ILogger<MachineService>, NullLogger<MachineService>>();
            services.AddSingleton<MachineService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionEntry>(dataStoreSettings.EncounterConditionCollectionName)
            );
            services.AddSingleton<ILogger<EncounterConditionService>, NullLogger<EncounterConditionService>>();
            services.AddSingleton<EncounterConditionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionValueEntry>(dataStoreSettings.EncounterConditionValueCollectionName)
            );
            services.AddSingleton<ILogger<EncounterConditionValueService>, NullLogger<EncounterConditionValueService>>();
            services.AddSingleton<EncounterConditionValueService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterMethodEntry>(dataStoreSettings.EncounterMethodCollectionName)
            );
            services.AddSingleton<ILogger<EncounterMethodService>, NullLogger<EncounterMethodService>>();
            services.AddSingleton<EncounterMethodService>();
        }

        /// <summary>
        /// Configures services for accessing the cache.
        /// </summary>
        private static void ConfigureCache(IServiceCollection services)
        {
            // bind cache settings
            services.Configure<CacheSettings>(
                Configuration.GetSection(nameof(CacheSettings))
            );

            // create singleton for cache settings
            services.AddSingleton<ICacheSettings>(sp =>
                sp.GetRequiredService<IOptions<CacheSettings>>().Value
            );

            // create cache services
            var cacheSourceFactory = new CacheSourceFactory();
            var cacheSettings = Configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Ability>(cacheSettings.AbilityCollectionName)
            );
            services.AddSingleton<ILogger<AbilityCacheService>, NullLogger<AbilityCacheService>>();
            services.AddSingleton<AbilityCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterCondition>(cacheSettings.EncounterConditionCollectionName)
            );
            services.AddSingleton<ILogger<EncounterConditionCacheService>, NullLogger<EncounterConditionCacheService>>();
            services.AddSingleton<EncounterConditionCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterConditionValue>(cacheSettings.EncounterConditionValueCollectionName)
            );
            services.AddSingleton<ILogger<EncounterConditionValueCacheService>, NullLogger<EncounterConditionValueCacheService>>();
            services.AddSingleton<EncounterConditionValueCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterMethod>(cacheSettings.EncounterMethodCollectionName)
            );
            services.AddSingleton<ILogger<EncounterMethodCacheService>, NullLogger<EncounterMethodCacheService>>();
            services.AddSingleton<EncounterMethodCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.Create<EvolutionChain>(cacheSettings.EvolutionChainCollectionName)
            );
            services.AddSingleton<ILogger<EvolutionChainCacheService>, NullLogger<EvolutionChainCacheService>>();
            services.AddSingleton<EvolutionChainCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EvolutionTrigger>(cacheSettings.EvolutionTriggerCollectionName)
            );
            services.AddSingleton<ILogger<EvolutionTriggerCacheService>, NullLogger<EvolutionTriggerCacheService>>();
            services.AddSingleton<EvolutionTriggerCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Generation>(cacheSettings.GenerationCollectionName)
            );
            services.AddSingleton<ILogger<GenerationCacheService>, NullLogger<GenerationCacheService>>();
            services.AddSingleton<GenerationCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Item>(cacheSettings.ItemCollectionName)
            );
            services.AddSingleton<ILogger<ItemCacheService>, NullLogger<ItemCacheService>>();
            services.AddSingleton<ItemCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<LocationArea>(cacheSettings.LocationAreaCollectionName)
            );
            services.AddSingleton<ILogger<LocationAreaCacheService>, NullLogger<LocationAreaCacheService>>();
            services.AddSingleton<LocationAreaCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Location>(cacheSettings.LocationCollectionName)
            );
            services.AddSingleton<ILogger<LocationCacheService>, NullLogger<LocationCacheService>>();
            services.AddSingleton<LocationCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.Create<Machine>(cacheSettings.MachineCollectionName)
            );
            services.AddSingleton<ILogger<MachineCacheService>, NullLogger<MachineCacheService>>();
            services.AddSingleton<MachineCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Move>(cacheSettings.MoveCollectionName)
            );
            services.AddSingleton<ILogger<MoveCacheService>, NullLogger<MoveCacheService>>();
            services.AddSingleton<MoveCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveCategory>(cacheSettings.MoveCategoryCollectionName)
            );
            services.AddSingleton<ILogger<MoveCategoryCacheService>, NullLogger<MoveCategoryCacheService>>();
            services.AddSingleton<MoveCategoryCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveDamageClass>(cacheSettings.MoveDamageClassCollectionName)
            );
            services.AddSingleton<ILogger<MoveDamageClassCacheService>, NullLogger<MoveDamageClassCacheService>>();
            services.AddSingleton<MoveDamageClassCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveLearnMethod>(cacheSettings.MoveLearnMethodCollectionName)
            );
            services.AddSingleton<ILogger<MoveLearnMethodCacheService>, NullLogger<MoveLearnMethodCacheService>>();
            services.AddSingleton<MoveLearnMethodCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveTarget>(cacheSettings.MoveTargetCollectionName)
            );
            services.AddSingleton<ILogger<MoveTargetCacheService>, NullLogger<MoveTargetCacheService>>();
            services.AddSingleton<MoveTargetCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Pokedex>(cacheSettings.PokedexCollectionName)
            );
            services.AddSingleton<ILogger<PokedexCacheService>, NullLogger<PokedexCacheService>>();
            services.AddSingleton<PokedexCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Pokemon>(cacheSettings.PokemonCollectionName)
            );
            services.AddSingleton<ILogger<PokemonCacheService>, NullLogger<PokemonCacheService>>();
            services.AddSingleton<PokemonCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokemonForm>(cacheSettings.PokemonFormCollectionName)
            );
            services.AddSingleton<ILogger<PokemonFormCacheService>, NullLogger<PokemonFormCacheService>>();
            services.AddSingleton<PokemonFormCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokemonSpecies>(cacheSettings.PokemonSpeciesCollectionName)
            );
            services.AddSingleton<ILogger<PokemonSpeciesCacheService>, NullLogger<PokemonSpeciesCacheService>>();
            services.AddSingleton<PokemonSpeciesCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Stat>(cacheSettings.StatCollectionName)
            );
            services.AddSingleton<ILogger<StatCacheService>, NullLogger<StatCacheService>>();
            services.AddSingleton<StatCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokeApiNet.Type>(cacheSettings.TypeCollectionName)
            );
            services.AddSingleton<ILogger<TypeCacheService>, NullLogger<TypeCacheService>>();
            services.AddSingleton<TypeCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokeApiNet.Version>(cacheSettings.VersionCollectionName)
            );
            services.AddSingleton<ILogger<VersionCacheService>, NullLogger<VersionCacheService>>();
            services.AddSingleton<VersionCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<VersionGroup>(cacheSettings.VersionGroupCollectionName)
            );
            services.AddSingleton<ILogger<VersionGroupCacheService>, NullLogger<VersionGroupCacheService>>();
            services.AddSingleton<VersionGroupCacheService>();
        }
    }
}
