﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for calling Pokemon resource endpoints.
    /// </summary>
    public class PokemonController : ResourceControllerBase
    {
        private readonly PokemonService _pokemonService;

        public PokemonController(
            PokemonService pokemonService,
            ILogger<PokemonController> logger) : base(logger)
        {
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Returns the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}")]
        public async Task<PokemonEntry> GetPokemonById(int pokemonId)
        {
            _logger.LogInformation($"Getting Pokemon {pokemonId}...");
            return await _pokemonService.Get(pokemonId);
        }

        /// <summary>
        /// Returns the forms of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/forms/{versionGroupId:int}")]
        public async Task<List<PokemonFormEntry>> GetPokemonFormsById(int pokemonId, int versionGroupId)
        {
            _logger.LogInformation($"Getting forms of Pokemon {pokemonId} in version group {versionGroupId}...");
            return await _pokemonService.GetPokemonForms(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the moves of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/moves/{versionGroupId:int}")]
        public async Task<PokemonMoveContext[]> GetPokemonMovesById(int pokemonId, int versionGroupId)
        {
            _logger.LogInformation($"Getting moves of Pokemon {pokemonId} in version group {versionGroupId}...");
            return await _pokemonService.GetPokemonMoves(pokemonId, versionGroupId);
        }

        /// <summary>
        /// Returns the abilities of the Pokemon with the given ID.
        /// </summary>
        [HttpGet("{pokemonId:int}/abilities")]
        public async Task<PokemonAbilityContext[]> GetPokemonAbilitiesById(int pokemonId)
        {
            _logger.LogInformation($"Getting abilities of Pokemon {pokemonId}...");
            return await _pokemonService.GetPokemonAbilities(pokemonId);
        }
    }
}
