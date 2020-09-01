using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Api.Controllers
{
    /// <summary>
    /// Searches for nearby food trucks.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SniffController : ControllerBase
    {

        private readonly ILogger<SniffController> _logger;

        private readonly IFoodTruckSpatialService _foodTruckSpatialService;


        public SniffController(IFoodTruckSpatialService foodTruckSpatialService, ILogger<SniffController> logger)
        {
            _foodTruckSpatialService = foodTruckSpatialService;
            _logger = logger;
        }

        /// <summary>
        /// Finds the 5 closest food trucks based on the provided client's location.
        /// </summary>
        /// <param name="latitude">Latitude of the client</param>
        /// <param name="longitude">Longitude of the client</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<FoodTruckResult>> Get(double latitude, double longitude)
        {
            _logger.LogTrace($"{nameof(SniffController)}.{nameof(Get)} - Finding food trucks for ({latitude}:{longitude})");

            var model = await _foodTruckSpatialService.GetFoodTrucksByDistanceAsync(latitude, longitude, 5); // hard coding 5 per the requirements, but devs have flexibility on count here

            if(model == null || model.Count == 0)
            {
                // TODO: Wire up a more proper error handling filter and friends error responses with ErrorDetail pattern and appropriate logging
                throw new HttpRequestException("No food trucks could be found.");
            }

            _logger.LogTrace($"{nameof(SniffController)}.{nameof(Get)} - Found '{model.Count}' food trucks for ({latitude}:{longitude})");

            return model;
        }

    }
}
