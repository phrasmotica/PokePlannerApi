using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokeApiNet;

namespace PokePlannerApi.Clients
{
    /// <summary>
    /// Class for reading PokeAPI resources from a file system.
    /// </summary>
    public class FileSystemPokeApiClient : IPokeAPI
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<FileSystemPokeApiClient> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSystemPokeApiClient(
            IFileSystem fileSystem,
            ILogger<FileSystemPokeApiClient> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        #region Resource Get() methods

        /// <summary>
        /// Returns the resource of the given type with the given ID.
        /// </summary>
        public Task<T> Get<T>(int id) where T : ResourceBase
        {
            var call = $"Get<{typeof(T)}>({id})";
            T res;
            try
            {
                _logger.LogInformation($"{call} started...");

                var directory = GetDirectory<T>();
                res = GetFromPath<T>($"{directory}/{id}/index.json");

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return Task.FromResult(res);
        }

        /// <summary>
        /// Returns the resource of the given type from the given navigation URL.
        /// </summary>
        public Task<T> Get<T>(UrlNavigation<T> nav) where T : ResourceBase
        {
            var call = $"Get<{typeof(T)}>(\"{nav.Url}\")";
            T res;
            try
            {
                _logger.LogInformation($"{call} started...");

                res = GetFromPath<T>(nav.Url);

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} from UrlNavigation object failed.");
                throw;
            }

            return Task.FromResult(res);
        }

        /// <summary>
        /// Returns the resources of the given type from the given navigation URLs.
        /// </summary>
        public Task<IEnumerable<T>> Get<T>(IEnumerable<UrlNavigation<T>> nav) where T : ResourceBase
        {
            var call = $"Get<{typeof(T)}>(urlList)";
            IEnumerable<T> resList;
            try
            {
                _logger.LogInformation($"{call} started...");

                resList = nav.Select(n => GetFromPath<T>(n.Url));

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} from UrlNavigation objects failed.");
                throw;
            }

            return Task.FromResult(resList);
        }

        /// <summary>
        /// Returns the encounters for the given Pokemon.
        /// </summary>
        public Task<IEnumerable<LocationAreaEncounter>> GetEncounters(Pokemon pokemon)
        {
            var call = $"GetLocationAreaEncounters({pokemon.Id})";
            IEnumerable<LocationAreaEncounter> res;
            try
            {
                _logger.LogInformation($"{call} started...");

                var url = pokemon.LocationAreaEncounters;
                res = GetFromPath<IEnumerable<LocationAreaEncounter>>(url);

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return Task.FromResult(res);
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

        /// <summary>
        /// Wrapper for <see cref="PokeApiClient.GetApiResourcePageAsync{T}"/> with exception logging.
        /// </summary>
        public async Task<ApiResourceList<T>> GetPage<T>() where T : ApiResource
        {
            return await GetPage<T>(20, 0);
        }

        /// <summary>
        /// Wrapper for <see cref="PokeApiClient.GetApiResourcePageAsync{T}()"/> with exception logging.
        /// </summary>
        public Task<ApiResourceList<T>> GetPage<T>(int limit, int offset) where T : ApiResource
        {
            var call = $"GetPage<{typeof(T)}>(limit={limit}, offset={offset})";
            ApiResourceList<T> resList;
            try
            {
                _logger.LogInformation($"{call} started...");

                var directory = GetDirectory<T>();
                var navList = Enumerable.Range(offset, limit)
                                        .Select(n => new ApiResource<T>
                                        {
                                            Url = $"{directory}/{n}/index.json"
                                        })
                                        .ToList();

                resList = new ApiResourceList<T>()
                {
                    Results = navList
                };

                _logger.LogInformation($"{call} finished.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{call} failed.");
                throw;
            }

            return Task.FromResult(resList);
        }

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

        #region Helpers

        /// <summary>
        /// Returns the name of the directory in which resources of the given
        /// type are stored.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        private static string GetDirectory<TResource>() where TResource : ResourceBase
        {
            return typeof(TResource).Name.ToLowerInvariant();
        }

        /// <summary>
        /// Returns the object of the given type from the file with the given path.
        /// </summary>
        private T GetFromPath<T>(string path)
        {
            // lowercase as files have lowercase named
            var sanitisedPath = path.ToLowerInvariant();

            var content = _fileSystem.File.ReadAllText(sanitisedPath);
            return JsonConvert.DeserializeObject<T>(content);
        }

        #endregion
    }
}
