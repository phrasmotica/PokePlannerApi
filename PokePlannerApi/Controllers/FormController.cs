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
        /// <summary>
        /// THe Pokemon forms service.
        /// </summary>
        private readonly PokemonFormService PokemonFormsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormController(PokemonFormService pokemonFormsService, ILogger<FormController> logger) : base(logger)
        {
            PokemonFormsService = pokemonFormsService;
        }

        /// <summary>
        /// Returns the Pokemon form with the given ID.
        /// </summary>
        [HttpGet("{formId:int}")]
        public async Task<PokemonFormEntry> GetFormById(int formId)
        {
            Logger.LogInformation($"Getting Pokemon form {formId}...");
            return await PokemonFormsService.GetPokemonForm(formId);
        }
    }
}
