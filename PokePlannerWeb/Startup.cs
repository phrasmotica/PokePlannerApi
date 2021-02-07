using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using PokemonEntry = PokePlannerWeb.Data.DataStore.Models.PokemonEntry;

namespace PokePlannerWeb
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
            // configure PokeAPI services
            services.AddSingleton<PokeApiClient>();
            services.AddSingleton<IPokeAPI, PokeAPI>();

            ConfigureDataStore(services);

            ConfigureCache(services);

            services.AddCors();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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

        /// <summary>
        /// Configures services for accessing the cache.
        /// </summary>
        private void ConfigureCache(IServiceCollection services)
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
            services.AddSingleton<AbilityCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterCondition>(cacheSettings.EncounterConditionCollectionName)
            );
            services.AddSingleton<EncounterConditionCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterConditionValue>(cacheSettings.EncounterConditionValueCollectionName)
            );
            services.AddSingleton<EncounterConditionValueCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EncounterMethod>(cacheSettings.EncounterMethodCollectionName)
            );
            services.AddSingleton<EncounterMethodCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.Create<EvolutionChain>(cacheSettings.EvolutionChainCollectionName)
            );
            services.AddSingleton<EvolutionChainCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<EvolutionTrigger>(cacheSettings.EvolutionTriggerCollectionName)
            );
            services.AddSingleton<EvolutionTriggerCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Generation>(cacheSettings.GenerationCollectionName)
            );
            services.AddSingleton<GenerationCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Item>(cacheSettings.ItemCollectionName)
            );
            services.AddSingleton<ItemCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<LocationArea>(cacheSettings.LocationAreaCollectionName)
            );
            services.AddSingleton<LocationAreaCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Location>(cacheSettings.LocationCollectionName)
            );
            services.AddSingleton<LocationCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.Create<Machine>(cacheSettings.MachineCollectionName)
            );
            services.AddSingleton<MachineCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Move>(cacheSettings.MoveCollectionName)
            );
            services.AddSingleton<MoveCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveCategory>(cacheSettings.MoveCategoryCollectionName)
            );
            services.AddSingleton<MoveCategoryCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveDamageClass>(cacheSettings.MoveDamageClassCollectionName)
            );
            services.AddSingleton<MoveDamageClassCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveLearnMethod>(cacheSettings.MoveLearnMethodCollectionName)
            );
            services.AddSingleton<MoveLearnMethodCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<MoveTarget>(cacheSettings.MoveTargetCollectionName)
            );
            services.AddSingleton<MoveTargetCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Pokedex>(cacheSettings.PokedexCollectionName)
            );
            services.AddSingleton<PokedexCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Pokemon>(cacheSettings.PokemonCollectionName)
            );
            services.AddSingleton<PokemonCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokemonForm>(cacheSettings.PokemonFormCollectionName)
            );
            services.AddSingleton<PokemonFormCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<PokemonSpecies>(cacheSettings.PokemonSpeciesCollectionName)
            );
            services.AddSingleton<PokemonSpeciesCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Stat>(cacheSettings.StatCollectionName)
            );
            services.AddSingleton<StatCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Type>(cacheSettings.TypeCollectionName)
            );
            services.AddSingleton<TypeCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<Version>(cacheSettings.VersionCollectionName)
            );
            services.AddSingleton<VersionCacheService>();

            services.AddSingleton(sp =>
                cacheSourceFactory.CreateNamed<VersionGroup>(cacheSettings.VersionGroupCollectionName)
            );
            services.AddSingleton<VersionGroupCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowCredentials();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
