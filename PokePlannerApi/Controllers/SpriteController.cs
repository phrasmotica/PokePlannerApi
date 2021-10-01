using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for accessing sprite data.
    /// </summary>
    public class SpriteController : ResourceControllerBase
    {
        private readonly IPokeApi _pokeApiClient;

        public SpriteController(
            IPokeApi pokeApiClient,
            ILogger<SpriteController> logger) : base(logger)
        {
            _pokeApiClient = pokeApiClient;
        }

        /// <summary>
        /// Returns sprite data for the Pokemon with the given ID.
        /// </summary>
        [HttpGet("variety/{varietyId:int}")]
        public async Task<PokemonSprites> GetSpritesOfVariety(int varietyId)
        {
            _logger.LogInformation($"Getting sprite info for Pokemon {varietyId}...");
            return await _pokeApiClient.GetSpritesOfVariety(varietyId);
        }

        /// <summary>
        /// Returns sprite data for the Pokemon form with the given ID.
        /// </summary>
        [HttpGet("form/{formId:int}")]
        public async Task<PokemonFormSprites> GetSpritesOfForm(int formId)
        {
            _logger.LogInformation($"Getting sprite info for Pokemon form {formId}...");
            return await _pokeApiClient.GetSpritesOfForm(formId);
        }
    }
}
