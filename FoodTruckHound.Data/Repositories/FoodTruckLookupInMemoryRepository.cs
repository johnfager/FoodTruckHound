﻿using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FoodTruckHound.Data.Repositories
{
    public class FoodTruckLookupInMemoryRepository : _baseRepository, IFoodTruckLookupRepository
    {
        // in memory representation of food trucks
        private static FoodTruckInfo[] _foodTrucks;

        private static readonly string _cacheKey = $"{nameof(FoodTruckLookupInMemoryRepository)}.Data";

        private readonly IFoodTruckDataService _foodTruckDataService;

        private readonly IMemoryCache _memoryCache;


        public FoodTruckLookupInMemoryRepository(IFoodTruckDataService foodTruckDataService, IMemoryCache memoryCache, ILogger logger) : base(logger)
        {
            _foodTruckDataService = foodTruckDataService;
        }

        public async Task<FoodTruckInfo[]> FindByLocationAsync(decimal longitude, decimal latitude, int maxCount = 5)
        {

            // TODO: Verify if the data service needs to run or not. If the data is very stale, consider returning an error to try back in a few minutes.

            bool attemptedRefresh = false;

            _memoryCache.TryGetValue<FoodTruckInfo[]>(_cacheKey, out FoodTruckInfo[] data);

            if (data == null || !data.Any() || _foodTruckDataService.LastRefreshedOnUtc.AddMinutes(_foodTruckDataService.RecommendedRefreshInMinutes) < DateTime.UtcNow)
            {
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
                _memoryCache.Set(_cacheKey, freshData);
                data = freshData; // set the data to the new set
            }

            // recheck for any issues
            if (data is null || !data.Any())
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
            return data;
        }
    }
}
