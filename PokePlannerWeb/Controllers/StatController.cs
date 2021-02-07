using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.DataStore.Services;

namespace PokePlannerWeb.Controllers
{
    /// <summary>
    /// Controller for getting stats.
    /// </summary>
    public class StatController : ResourceControllerBase
    {
        /// <summary>
        /// The stats service.
        /// </summary>
        private readonly StatService StatsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatController(StatService statsService, ILogger<StatController> logger) : base(logger)
        {
            StatsService = statsService;
        }

        /// <summary>
        /// Returns the base stats in the version group with the given ID.
        /// </summary>
        [HttpGet("{versionGroupId:int}")]
        public async Task<StatEntry[]> GetBaseStatsInVersionGroup(int versionGroupId)
        {
            Logger.LogInformation($"Getting base stats in version group {versionGroupId}...");
            var allStats = await StatsService.GetBaseStats(versionGroupId);
            return allStats.ToArray();
        }
    }
}
