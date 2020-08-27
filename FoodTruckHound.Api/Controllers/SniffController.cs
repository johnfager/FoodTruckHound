using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodTruckHound.Core.Repositories;
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




        public SniffController(IFoodTruckLookupRepository foodTruckLookupRepository, ILogger<SniffController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<FoodTruckInfo> Get()
        {
            throw new NotImplementedException();
        }
    }
}
