using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.OpenApi.Models;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Clients.GraphQL;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.DataStore.Settings;
using PokePlannerApi.Models;
using PokePlannerApi.OpenAPI;
using PokePlannerApi.Resilience;
using PokePlannerApi.Settings;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;
using Type = PokeApiNet.Type;
using Version = PokeApiNet.Version;
using PokeApiClient = PokePlannerApi.Clients.REST.PokeApiClient;

namespace PokePlannerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigurePokeApiClient(services);
            ConfigureDataStore(services);

            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    });

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<NullablePrimitiveTypesSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokePlannerApi", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                });
            });
        }

        /// <summary>
        /// Configures the PokeAPI client.
        /// </summary>
        private void ConfigurePokeApiClient(IServiceCollection services)
        {
            var pokeApiSettings = Configuration.GetSection(nameof(PokeApiSettings)).Get<PokeApiSettings>();

            var pokeApiUrl = Environment.GetEnvironmentVariable("POKEAPI_URL");

            services.AddSingleton(sp => new PokeApiClient(new HttpClient
            {
                BaseAddress = new Uri(pokeApiUrl),
            }));

            services.AddSingleton<IPokeApi, PokeAPI>();

            var resiliencePolicy = ResiliencePolicy.CreateResiliencePolicy(pokeApiSettings, NullLogger.Instance);

            services.AddSingleton(sp => new PokeAPIGraphQLClient(new Uri(pokeApiSettings.GraphQlUri), resiliencePolicy));
        }

        /// <summary>
        /// Configures services for accessing the data store.
        /// </summary>
        private void ConfigureDataStore(IServiceCollection services)
        {
            // create data store services
            var dataStoreSourceFactory = new DataStoreSourceFactory();
            var dataStoreSettings = Configuration.GetSection(nameof(DataStoreSettings)).Get<DataStoreSettings>();
            var collectionSettings = dataStoreSettings.CollectionSettings;

            AddNamedService<Ability, AbilityEntry, AbilityConverter, AbilityService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.AbilityCollectionName);

            AddNamedService<Type, EfficacyEntry, EfficacyConverter, EfficacyService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.EfficacyCollectionName);

            //AddNamedService<EncounterCondition, EncounterConditionEntry, EncounterConditionConverter, EncounterConditionService>(
            //    services,
            //    dataStoreSourceFactory,
            //    dataStoreSettings,
            //    collectionSettings.EncounterConditionCollectionName);

            AddNamedService<EncounterConditionValue, EncounterConditionValueEntry, EncounterConditionValueConverter, EncounterConditionValueService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.EncounterConditionValueCollectionName);

            AddNamedService<EncounterMethod, EncounterMethodEntry, EncounterMethodConverter, EncounterMethodService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.EncounterMethodCollectionName);

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncountersEntry>(
                    collectionSettings.EncounterCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<IResourceConverter<Pokemon, EncountersEntry>, EncountersConverter>();
            services.AddSingleton<EncountersService>();

            AddService<EvolutionChain, EvolutionChainEntry, EvolutionChainConverter, EvolutionChainService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.EvolutionChainCollectionName);

            AddNamedService<EvolutionTrigger, EvolutionTriggerEntry, EvolutionTriggerConverter, EvolutionTriggerService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.EvolutionTriggerCollectionName);

            AddNamedService<Generation, GenerationEntry, GenerationConverter, GenerationService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.GenerationCollectionName);

            AddNamedService<Item, ItemEntry, ItemConverter, ItemService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.ItemCollectionName);

            AddNamedService<Location, LocationEntry, LocationConverter, LocationService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.LocationCollectionName);

            AddNamedService<LocationArea, LocationAreaEntry, LocationAreaConverter, LocationAreaService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.LocationAreaCollectionName);

            AddService<Machine, MachineEntry, MachineConverter, MachineService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MachineCollectionName);

            AddNamedService<MoveCategory, MoveCategoryEntry, MoveCategoryConverter, MoveCategoryService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MoveCategoryCollectionName);

            AddNamedService<MoveDamageClass, MoveDamageClassEntry, MoveDamageClassConverter, MoveDamageClassService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MoveDamageClassCollectionName);

            AddNamedService<MoveLearnMethod, MoveLearnMethodEntry, MoveLearnMethodConverter, MoveLearnMethodService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MoveLearnMethodCollectionName);

            AddNamedService<Move, MoveEntry, MoveConverter, MoveService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MoveCollectionName);

            AddNamedService<MoveTarget, MoveTargetEntry, MoveTargetConverter, MoveTargetService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.MoveTargetCollectionName);

            AddNamedService<Pokedex, PokedexEntry, PokedexConverter, PokedexService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.PokedexCollectionName);

            AddNamedService<Pokemon, PokemonEntry, PokemonConverter, PokemonService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.PokemonCollectionName);

            AddNamedService<PokemonForm, PokemonFormEntry, PokemonFormConverter, PokemonFormService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.PokemonFormCollectionName);

            AddNamedService<PokemonSpecies, PokemonSpeciesEntry, PokemonSpeciesConverter, PokemonSpeciesService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.PokemonSpeciesCollectionName);

            AddNamedService<Type, TypeEntry, TypeConverter, TypeService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.TypeCollectionName);

            AddNamedService<Version, VersionEntry, VersionConverter, VersionService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.VersionCollectionName);

            AddNamedService<VersionGroup, VersionGroupEntry, VersionGroupConverter, VersionGroupService>(
                services,
                dataStoreSourceFactory,
                dataStoreSettings,
                collectionSettings.VersionGroupCollectionName);
        }

        private static void AddNamedService<TResource, TEntry, TConverter, TService>(
            IServiceCollection services,
            DataStoreSourceFactory dataStoreSourceFactory,
            DataStoreSettings dataStoreSettings,
            string collectionName)
            where TResource : NamedApiResource
            where TEntry : EntryBase
            where TConverter : class, IResourceConverter<TResource, TEntry>
            where TService : class, INamedEntryService<TResource, TEntry>
        {
            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<TEntry>(
                    collectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );

            services.AddSingleton<IResourceConverter<TResource, TEntry>, TConverter>();
            services.AddSingleton<TService>();
        }

        private static void AddService<TResource, TEntry, TConverter, TService>(
            IServiceCollection services,
            DataStoreSourceFactory dataStoreSourceFactory,
            DataStoreSettings dataStoreSettings,
            string collectionName)
            where TResource : ApiResource
            where TEntry : EntryBase
            where TConverter : class, IResourceConverter<TResource, TEntry>
            where TService : class, IEntryService<TResource, TEntry>
        {
            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<TEntry>(
                    collectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );

            services.AddSingleton<IResourceConverter<TResource, TEntry>, TConverter>();
            services.AddSingleton<TService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokePlannerApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
