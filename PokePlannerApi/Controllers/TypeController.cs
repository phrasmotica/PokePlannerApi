using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.DataStore.Services;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Type resource endpoints.
    /// </summary>
    public class TypeController : ResourceControllerBase
    {
        /// <summary>
        /// The type service.
        /// </summary>
        private readonly TypeService TypeService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeController(TypeService typeService, ILogger<TypeController> logger) : base(logger)
        {
            TypeService = typeService;
        }

        /// <summary>
        /// Returns all concrete types.
        /// </summary>
        public async Task<TypeEntry[]> GetConcreteTypes()
        {
            Logger.LogInformation($"Getting concrete types...");
            return await TypeService.GetConcrete();
        }
    }
}
