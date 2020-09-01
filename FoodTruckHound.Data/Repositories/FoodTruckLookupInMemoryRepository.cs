using FoodTruckHound.Models;
using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

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

        public async Task<List<FoodTruckInfo>> GetFoodTrucksAsync()
        {
            // TODO: Verify if the data service needs to run or not. If the data is stale, consider returning an error asking the client to try back in a few minutes.

            bool attemptedRefresh = false;

            if (_foodTrucks == null || !_foodTrucks.Any() || _lastRefreshedOnUtc.AddMinutes(_recommendedRefreshInMinutes) < DateTime.UtcNow)
            {
                _logger.LogTrace($"{nameof(FoodTruckLookupInMemoryRepository)}.{nameof(GetFoodTrucksAsync)} - Refreshing via service");

                attemptedRefresh = true;
                var freshData = await _foodTruckDataService.RefreshRemoteDataAsync();

                if (freshData is null || !freshData.Any())
                {
                    // an example of throwing an exception but capturing and logging it silently; logging layer can handle alerts and notices to the team
                    try
                    {
                        // throw to get context
                        throw new Exception($"No data was found in the {nameof(GetFoodTrucksAsync)} array returned from {nameof(IFoodTruckDataService)}.{nameof(_foodTruckDataService.RefreshRemoteDataAsync)}() method.");
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
                        // TODO: Verify with the city that REQUESTED and APPROVED ARE ACTIVE
                        // TODO: Log and verify issues with valid records that do not have lat or long set
                        _foodTrucks = freshData.Where(x => (x.Status == "REQUESTED" || x.Status == "APPROVED") && x.Latitude != 0 && x.Longitude != 0).ToList(); // set the data to the new set of active trucks
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
                    throw new Exception($"No data was found in the {nameof(_foodTrucks)} array even after attempting to refresh the data set.");
                }
                else
                {
                    throw new Exception($"No data was found in the {nameof(_foodTrucks)} array.");
                }
            }

            // return valid data
            return _foodTrucks;
        }
    }
}
