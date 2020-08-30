using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckHound.Data.Repositories
{
    /// <summary>
    /// A quick proof of concept functional repository to store the data in memory.
    /// </summary>
    public class FoodTruckLookupInMemoryRepository : _baseRepository<FoodTruckLookupInMemoryRepository>, IFoodTruckLookupRepository
    {
        private static readonly object _lock = new object();

        // in memory representation of food trucks
        private static List<FoodTruckInfo> _foodTrucks;

        /// <summary>
        /// Keeps the latest refresh time for the data from the service.
        /// </summary>
        private static DateTime _lastRefreshedOnUtc;

        /// <summary>
        /// The recommended number of minutes after which the provider recommends refreshing data.
        /// </summary>
        private static readonly int _recommendedRefreshInMinutes = 300;

        // ---------

        private readonly IFoodTruckDataService _foodTruckDataService;


        public FoodTruckLookupInMemoryRepository(IFoodTruckDataService foodTruckDataService, ILogger<FoodTruckLookupInMemoryRepository> logger) : base(logger)
        {
            _foodTruckDataService = foodTruckDataService;
        }

        public async Task<List<FoodTruckInfo>> FindByLocationAsync(decimal longitude, decimal latitude, int maxCount = 5)
        {

            // TODO: Verify if the data service needs to run or not. If the data is stale, consider returning an error asking the client to try back in a few minutes.

            bool attemptedRefresh = false;

            if (_foodTrucks == null || !_foodTrucks.Any() || _lastRefreshedOnUtc.AddMinutes(_recommendedRefreshInMinutes) < DateTime.UtcNow)
            {
                _logger.LogTrace($"{nameof(FoodTruckLookupInMemoryRepository)}.{nameof(FindByLocationAsync)} - Refreshing via service");

                attemptedRefresh = true;
                var freshData = await _foodTruckDataService.RefreshRemoteDataAsync();
     
                if (freshData is null || !freshData.Any())
                {
                    // an example of throwing an exception but capturing and logging it silently; logging layer can handle alerts and notices to the team
                    try
                    {
                        // throw to get context
                        throw new Exception($"No data was found in the {nameof(FoodTruckInfo)} array returned from {nameof(IFoodTruckDataService)}.{nameof(_foodTruckDataService.RefreshRemoteDataAsync)}() method.");
                    }
                    catch (Exception ex)
                    {
                        // log silently and continue; stale data might be ok
                        _logger.LogError(ex, ex.Message);
                    }
                }
                if (freshData != null)
                {
                    lock (_lock)
                    {
                        _foodTrucks = freshData; // set the data to the new set
                        _lastRefreshedOnUtc = DateTime.UtcNow;
                    }
                }
            }

            // recheck for any issues
            if (_foodTrucks is null || !_foodTrucks.Any())
            {
                // an example of more robust exceptions
                if (attemptedRefresh)
                {
                    throw new Exception($"No data was found in the {nameof(FoodTruckInfo)} array even after attempting to refresh the data set.");
                }
                else
                {
                    throw new Exception($"No data was found in the {nameof(FoodTruckInfo)} array.");
                }
            }

            // return valid data
            return _foodTrucks;
        }
    }
}
