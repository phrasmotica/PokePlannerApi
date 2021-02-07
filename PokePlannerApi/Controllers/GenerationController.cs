using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting version groups.
    /// </summary>
    public class GenerationController : ResourceControllerBase
    {
        /// <summary>
        /// The generation service.
        /// </summary>
        private readonly GenerationService GenerationService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GenerationController(GenerationService versionGroupsService, ILogger<GenerationController> logger) : base(logger)
        {
            GenerationService = versionGroupsService;
        }

        /// <summary>
        /// Returns all generations.
        /// </summary>
        public async Task<GenerationEntry[]> GetGenerations()
        {
            Logger.LogInformation("Getting generations...");
            return await GenerationService.GetAll();
        }
    }
}
