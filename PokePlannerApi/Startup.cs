using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PokeApiNet;
using PokePlannerApi.Data;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.DataStore.Settings;
using PokePlannerApi.Models;
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
                dataStoreSourceFactory.Create<AbilityEntry>(dataStoreSettings.AbilityCollectionName)
            );
            services.AddSingleton<AbilityService>();

            services.AddSingleton<EfficacyService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionEntry>(dataStoreSettings.EncounterConditionCollectionName)
            );
            services.AddSingleton<EncounterConditionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterConditionValueEntry>(dataStoreSettings.EncounterConditionValueCollectionName)
            );
            services.AddSingleton<EncounterConditionValueService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncounterMethodEntry>(dataStoreSettings.EncounterMethodCollectionName)
            );
            services.AddSingleton<EncounterMethodService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EncountersEntry>(dataStoreSettings.EncounterCollectionName)
            );
            services.AddSingleton<EncountersService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionChainEntry>(dataStoreSettings.EvolutionChainCollectionName)
            );
            services.AddSingleton<EvolutionChainService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<EvolutionTriggerEntry>(dataStoreSettings.EvolutionTriggerCollectionName)
            );
            services.AddSingleton<EvolutionTriggerService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<ItemEntry>(dataStoreSettings.ItemCollectionName)
            );
            services.AddSingleton<ItemService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<GenerationEntry>(dataStoreSettings.GenerationCollectionName)
            );
            services.AddSingleton<GenerationService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<LocationEntry>(dataStoreSettings.LocationCollectionName)
            );
            services.AddSingleton<LocationService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<LocationAreaEntry>(dataStoreSettings.LocationAreaCollectionName)
            );
            services.AddSingleton<LocationAreaService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MachineEntry>(dataStoreSettings.MachineCollectionName)
            );
            services.AddSingleton<MachineService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveCategoryEntry>(dataStoreSettings.MoveCategoryCollectionName)
            );
            services.AddSingleton<MoveCategoryService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveDamageClassEntry>(dataStoreSettings.MoveDamageClassCollectionName)
            );
            services.AddSingleton<MoveDamageClassService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveLearnMethodEntry>(dataStoreSettings.MoveLearnMethodCollectionName)
            );
            services.AddSingleton<MoveLearnMethodService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveEntry>(dataStoreSettings.MoveCollectionName)
            );
            services.AddSingleton<MoveService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<MoveTargetEntry>(dataStoreSettings.MoveTargetCollectionName)
            );
            services.AddSingleton<MoveTargetService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokedexEntry>(dataStoreSettings.PokedexCollectionName)
            );
            services.AddSingleton<PokedexService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonEntry>(dataStoreSettings.PokemonCollectionName)
            );
            services.AddSingleton<PokemonService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonFormEntry>(dataStoreSettings.PokemonFormCollectionName)
            );
            services.AddSingleton<PokemonFormService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<PokemonSpeciesEntry>(dataStoreSettings.PokemonSpeciesCollectionName)
            );
            services.AddSingleton<PokemonSpeciesService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<StatEntry>(dataStoreSettings.StatCollectionName)
            );
            services.AddSingleton<StatService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<TypeEntry>(dataStoreSettings.TypeCollectionName)
            );
            services.AddSingleton<TypeService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionEntry>(dataStoreSettings.VersionCollectionName)
            );
            services.AddSingleton<VersionService>();

            services.AddSingleton(sp =>
                dataStoreSourceFactory.Create<VersionGroupEntry>(dataStoreSettings.VersionGroupCollectionName)
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
