using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Form resource endpoints.
    /// </summary>
    public class FormController : ResourceControllerBase
    {
        private readonly PokemonFormService _pokemonFormService;

        public FormController(
            PokemonFormService pokemonFormService,
            ILogger<FormController> logger) : base(logger)
        {
            _pokemonFormService = pokemonFormService;
        }

        /// <summary>
        /// Returns the Pokemon form with the given ID.
        /// </summary>
        [HttpGet("{formId:int}")]
        public async Task<PokemonFormEntry> GetFormById(int formId)
        {
            _logger.LogInformation($"Getting Pokemon form {formId}...");
            return await _pokemonFormService.Get(formId);
        }
    }
}
