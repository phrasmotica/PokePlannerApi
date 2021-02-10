using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PokeApiNet;
using PokePlannerApi.Data;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.DataStore.Settings;
using PokePlannerApi.Models;
using PokePlannerApi.Settings;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
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

            services.AddSingleton(sp => new PokeApiClient(new Uri(pokeApiSettings.BaseUri)));
            services.AddSingleton<IPokeAPI, PokeAPI>();
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

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<AbilityEntry>(
                    collectionSettings.AbilityCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<NamedApiResourceServiceBase<Ability, AbilityEntry>>();
            services.AddSingleton<AbilityService>();

            services.AddSingleton<EfficacyService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionEntry>(
                    collectionSettings.EncounterConditionCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EncounterConditionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionValueEntry>(
                    collectionSettings.EncounterConditionValueCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EncounterConditionValueService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterMethodEntry>(
                    collectionSettings.EncounterMethodCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EncounterMethodService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncountersEntry>(
                    collectionSettings.EncounterCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EncountersService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionChainEntry>(
                    collectionSettings.EvolutionChainCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EvolutionChainService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionTriggerEntry>(
                    collectionSettings.EvolutionTriggerCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<EvolutionTriggerService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<ItemEntry>(
                    collectionSettings.ItemCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<ItemService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<GenerationEntry>(
                    collectionSettings.GenerationCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<GenerationService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<LocationEntry>(
                    collectionSettings.LocationCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<LocationService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<LocationAreaEntry>(
                    collectionSettings.LocationAreaCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<LocationAreaService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MachineEntry>(
                    collectionSettings.MachineCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MachineService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveCategoryEntry>(
                    collectionSettings.MoveCategoryCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MoveCategoryService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveDamageClassEntry>(
                    collectionSettings.MoveDamageClassCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MoveDamageClassService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveLearnMethodEntry>(
                    collectionSettings.MoveLearnMethodCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MoveLearnMethodService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveEntry>(
                    collectionSettings.MoveCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MoveService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveTargetEntry>(
                    collectionSettings.MoveTargetCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<MoveTargetService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokedexEntry>(
                    collectionSettings.PokedexCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<PokedexService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonEntry>(
                    collectionSettings.PokemonCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<PokemonService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonFormEntry>(
                    collectionSettings.PokemonFormCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<PokemonFormService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonSpeciesEntry>(
                    collectionSettings.PokemonSpeciesCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<PokemonSpeciesService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<StatEntry>(
                    collectionSettings.StatCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<StatService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<TypeEntry>(
                    collectionSettings.TypeCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<TypeService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionEntry>(
                    collectionSettings.VersionCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<VersionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionGroupEntry>(
                    collectionSettings.VersionGroupCollectionName,
                    dataStoreSettings.DatabaseName,
                    dataStoreSettings.PrivateKey,
                    dataStoreSettings.ConnectionString)
            );
            services.AddSingleton<VersionGroupService>();
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
