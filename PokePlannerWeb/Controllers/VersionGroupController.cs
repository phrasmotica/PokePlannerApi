using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;

namespace PokePlannerWeb.Controllers
{
    /// <summary>
    /// Controller for getting version groups.
    /// </summary>
    public class VersionGroupController : ResourceControllerBase
    {
        /// <summary>
        /// The version groups service.
        /// </summary>
        private readonly VersionGroupService VersionGroupsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VersionGroupController(VersionGroupService versionGroupsService, ILogger<VersionGroupController> logger) : base(logger)
        {
            VersionGroupsService = versionGroupsService;
        }

        /// <summary>
        /// Returns all version groups.
        /// </summary>
        [HttpGet("all")]
        public async Task<VersionGroupEntry[]> GetVersionGroups()
        {
            Logger.LogInformation("VersionGroupController: getting version groups...");
            return await VersionGroupsService.GetAll();
        }
    }
}
