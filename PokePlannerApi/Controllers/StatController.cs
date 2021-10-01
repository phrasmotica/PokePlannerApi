using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting stats.
    /// </summary>
    public class StatController : ResourceControllerBase
    {
        private readonly StatService _statService;

        public StatController(
            StatService statsService,
            ILogger<StatController> logger) : base(logger)
        {
            _statService = statsService;
        }

        /// <summary>
        /// Returns the base stats in the version group with the given ID.
        /// </summary>
        [HttpGet("{versionGroupId:int}")]
        public async Task<StatEntry[]> GetBaseStatsInVersionGroup(int versionGroupId)
        {
            _logger.LogInformation($"Getting base stats in version group {versionGroupId}...");
            var allStats = await _statService.GetBaseStats(versionGroupId);
            return allStats.ToArray();
        }
    }
}
