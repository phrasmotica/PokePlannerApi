using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeApiClient = PokePlannerApi.Clients.REST.PokeApiClient;

namespace PokePlannerApi.Clients
{
    /// <summary>
    /// Class for fetching PokeAPI resources.
    /// </summary>
    public class PokeAPI : IPokeApi
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<PokeAPI> _logger;

        /// <summary>
        /// The client for PokeAPI.
        /// </summary>
        private readonly PokeApiClient _pokeApiClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PokeAPI(PokeApiClient pokeApiClient, ILogger<PokeAPI> logger)
        {
            _pokeApiClient = pokeApiClient ?? throw new ArgumentNullException(nameof(pokeApiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Resource Get() methods

        public Task<T> Get<T>(int id) where T : ResourceBase => _pokeApiClient.GetResourceAsync<T>(id);

        public Task<T> Get<T>(UrlNavigation<T> nav) where T : ResourceBase => _pokeApiClient.GetResourceAsync(nav);

        public Task<List<T>> Get<T>(IEnumerable<UrlNavigation<T>> nav) where T : ResourceBase => _pokeApiClient.GetResourceAsync(nav);

        public async Task<PokemonSprites> GetSpritesOfVariety(int varietyId)
        {
            var pokemon = await Get<Pokemon>(varietyId);
            return pokemon.Sprites;
        }

        public async Task<PokemonFormSprites> GetSpritesOfForm(int formId)
        {
            var form = await Get<PokemonForm>(formId);
            return form.Sprites;
        }

        /// <summary>
        /// Returns the encounters for the given Pokemon.
        /// </summary>
        public async Task<IEnumerable<LocationAreaEncounter>> GetEncounters(Pokemon pokemon)
        {
            var url = pokemon.LocationAreaEncounters;
            return await _pokeApiClient.GetFromUrl<IEnumerable<LocationAreaEncounter>>(url);
        }

        #endregion

        #region API resources

        /// <summary>
        /// Returns a page with all resources of the given type.
        /// </summary>
        public async Task<ApiResourceList<T>> GetFullPage<T>() where T : ApiResource
        {
            var call = $"GetFullPage<{typeof(T)}>()";
            ApiResourceList<T> res;
            try
            {
                _logger.LogInformation($"{call} started...");

                var page = await GetPage<T>(1, 1);
                res = await GetPage<T>(page.Count, 0);

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return res;
        }

        public async Task<ApiResourceList<T>> GetPage<T>() where T : ApiResource => await GetPage<T>(20, 0);

        public Task<ApiResourceList<T>> GetPage<T>(int limit, int offset) where T : ApiResource => _pokeApiClient.GetApiResourcePageAsync<T>(limit, offset);

        /// <summary>
        /// Returns many named API resources of the given type.
        /// </summary>
        public async Task<IEnumerable<T>> GetMany<T>(int limit = 20, int offset = 0) where T : ApiResource
        {
            var call = $"GetMany<{typeof(T)}>(limit={limit}, offset={offset})";
            IEnumerable<T> res;
            try
            {
                _logger.LogInformation($"{call} started...");

                var page = await GetPage<T>(limit, offset);
                res = await Get(page.Results);

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return res;
        }

        /// <summary>
        /// Returns the last named API resource of the given type.
        /// </summary>
        public async Task<T> GetLast<T>() where T : ApiResource
        {
            var call = $"GetLast<{typeof(T)}>()";
            T res;
            try
            {
                _logger.LogInformation($"{call} started...");

                // get a page to find the resource count
                var page = await GetPage<T>();
                if (!string.IsNullOrEmpty(page.Next))
                {
                    // get the last page if that wasn't it
                    page = await GetPage<T>(page.Count - 1, 1);
                }

                res = await Get(page.Results.Last());

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return res;
        }

        #endregion

        #region Named API resources

        public Task<T> Get<T>(string name) where T : NamedApiResource => _pokeApiClient.GetResourceAsync<T>(name);

        /// <summary>
        /// Returns a page with all resources of the given type.
        /// </summary>
        public async Task<NamedApiResourceList<T>> GetNamedFullPage<T>() where T : NamedApiResource
        {
            var call = $"GetFullPage<{typeof(T)}>()";
            NamedApiResourceList<T> res;
            try
            {
                _logger.LogInformation($"{call} started...");

                var page = await GetNamedPage<T>(1, 1);
                res = await GetNamedPage<T>(page.Count, 0);

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return res;
        }

        public Task<NamedApiResourceList<T>> GetNamedPage<T>() where T : NamedApiResource => GetNamedPage<T>(20, 0);

        /// <summary>
        /// Wrapper for <see cref="PokeApiClient.GetNamedResourcePageAsync{T}()"/> with exception logging.
        /// </summary>
        public Task<NamedApiResourceList<T>> GetNamedPage<T>(int limit, int offset) where T : NamedApiResource => _pokeApiClient.GetNamedResourcePageAsync<T>(limit, offset);

        #endregion
    }
}
