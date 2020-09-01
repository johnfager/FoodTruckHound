using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodTruckHound.Api.Controllers
{
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

        [HttpGet]
        public async Task<IEnumerable<FoodTruckResult>> Get(double latitude, double longitude)
        {
           

            return await _foodTruckSpatialService.GetFoodTrucksByDistanceAsync(latitude, longitude, 5); // hard coding 5 per the requirements, but devs have flexibility on count here
        }

    }
}
