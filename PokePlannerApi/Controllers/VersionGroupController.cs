using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting version groups.
    /// </summary>
    public class VersionGroupController : ResourceControllerBase
    {
        private readonly VersionGroupService _versionGroupService;

        public VersionGroupController(
            VersionGroupService versionGroupsService,
            ILogger<VersionGroupController> logger) : base(logger)
        {
            _versionGroupService = versionGroupsService;
        }

        /// <summary>
        /// Returns all version groups.
        /// </summary>
        [HttpGet("all")]
        public async Task<VersionGroupEntry[]> GetVersionGroups()
        {
            _logger.LogInformation("VersionGroupController: getting version groups...");
            return await _versionGroupService.GetAll();
        }
    }
}
