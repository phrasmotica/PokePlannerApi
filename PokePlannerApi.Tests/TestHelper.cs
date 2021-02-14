using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Tests
{
    /// <summary>
    /// Helper class for writing tests.
    /// </summary>
    public class TestHelper
    {
        /// <summary>
        /// Creates a service provider for tests.
        /// </summary>
        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // configure PokeAPI services
            services.AddSingleton<PokeApiClient>();
            services.AddSingleton<ILogger<PokeAPI>, NullLogger<PokeAPI>>();
            services.AddSingleton<IPokeAPI, PokeAPI>();

            ConfigureDataStore(services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures services for accessing the data store.
        /// </summary>
        private static void ConfigureDataStore(IServiceCollection services)
        {
          
            services.AddSingleton<ILogger<StatService>, NullLogger<StatService>>();
            services.AddSingleton<StatService>();

            services.AddSingleton<ILogger<GenerationService>, NullLogger<GenerationService>>();
            services.AddSingleton<GenerationService>();

            services.AddSingleton<ILogger<PokedexService>, NullLogger<PokedexService>>();
            services.AddSingleton<PokedexService>();

            services.AddSingleton<ILogger<VersionService>, NullLogger<VersionService>>();
            services.AddSingleton<VersionService>();

            services.AddSingleton<ILogger<VersionGroupService>, NullLogger<VersionGroupService>>();
            services.AddSingleton<VersionGroupService>();

            services.AddSingleton<ILogger<TypeService>, NullLogger<TypeService>>();
            services.AddSingleton<TypeService>();

            services.AddSingleton<ILogger<MoveCategoryService>, NullLogger<MoveCategoryService>>();
            services.AddSingleton<MoveCategoryService>();

            services.AddSingleton<ILogger<MoveDamageClassService>, NullLogger<MoveDamageClassService>>();
            services.AddSingleton<MoveDamageClassService>();

            services.AddSingleton<ILogger<MoveService>, NullLogger<MoveService>>();
            services.AddSingleton<MoveService>();

            services.AddSingleton<ILogger<MoveTargetService>, NullLogger<MoveTargetService>>();
            services.AddSingleton<MoveTargetService>();

            services.AddSingleton<ILogger<EvolutionChainService>, NullLogger<EvolutionChainService>>();
            services.AddSingleton<EvolutionChainService>();

            services.AddSingleton<ILogger<EvolutionTriggerService>, NullLogger<EvolutionTriggerService>>();
            services.AddSingleton<EvolutionTriggerService>();

            services.AddSingleton<ILogger<ItemService>, NullLogger<ItemService>>();
            services.AddSingleton<ItemService>();

            services.AddSingleton<ILogger<AbilityService>, NullLogger<AbilityService>>();
            services.AddSingleton<AbilityService>();

            services.AddSingleton<ILogger<MoveLearnMethodService>, NullLogger<MoveLearnMethodService>>();
            services.AddSingleton<MoveLearnMethodService>();

            services.AddSingleton<ILogger<MachineService>, NullLogger<MachineService>>();
            services.AddSingleton<MachineService>();

            services.AddSingleton<ILogger<EncounterConditionService>, NullLogger<EncounterConditionService>>();
            services.AddSingleton<EncounterConditionService>();

            services.AddSingleton<ILogger<EncounterConditionValueService>, NullLogger<EncounterConditionValueService>>();
            services.AddSingleton<EncounterConditionValueService>();

            services.AddSingleton<ILogger<EncounterMethodService>, NullLogger<EncounterMethodService>>();
            services.AddSingleton<EncounterMethodService>();
        }
    }
}
