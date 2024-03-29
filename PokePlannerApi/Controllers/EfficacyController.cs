﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Controllers
{
    /// <summary>
    /// Controller for getting type efficacies.
    /// </summary>
    public class EfficacyController : ResourceControllerBase
    {
        private readonly EfficacyService _efficacyService;

        public EfficacyController(
            EfficacyService efficacyService,
            ILogger<EfficacyController> logger) : base(logger)
        {
            _efficacyService = efficacyService;
        }

        /// <summary>
        /// Returns the efficacy of the type with the given ID.
        /// </summary>
        [HttpGet("{typeId:int}")]
        public async Task<EfficacyEntry> Get(int typeId)
        {
            return await _efficacyService.Get(typeId);
        }

        /// <summary>
        /// Returns the efficacy of the types with the given IDs in the version
        /// group with the given ID.
        /// </summary>
        [HttpGet]
        public async Task<EfficacySet> GetEfficacySet(
            [FromQuery(Name = "versionGroup")] int versionGroupId,
            [FromQuery(Name = "type")] int[] types)
        {
            return await _efficacyService.GetEfficacySet(types, versionGroupId);
        }
    }
}
