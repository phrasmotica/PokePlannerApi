using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PokeApiNet;
using PokePlannerApi.Clients;

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
            services.AddSingleton<IPokeApi, PokeAPI>();

            return services.BuildServiceProvider();
        }
    }
}
