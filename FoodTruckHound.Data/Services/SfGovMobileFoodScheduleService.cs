using FoodTruckHound.Core.Services;
using FoodTruckHound.Models;
using System;
using System.Threading.Tasks;

namespace FoodTruckHound.Data.Services.SfGov
{
    public class SfGovMobileFoodScheduleService : IFoodTruckDataService
    {
        private static DateTime _lastRefreshedOnUtc;

        private static readonly object _lock = new object();

        public DateTime LastRefreshedOnUtc => _lastRefreshedOnUtc;


        public int RecommendedRefreshInMinutes { get; set; } = 300; // default setting for refresh rate

        public async Task<FoodTruckInfo[]> RefreshRemoteDataAsync()
        {
            // TODO: Locate and parse the CSV 

            // TODO: Get better schedule data if time (feature add)

            lock(_lock)
            {
                _lastRefreshedOnUtc = DateTime.UtcNow;
            }

            throw new NotImplementedException();
        }

        #region helpers

        // TODO: Look at populating better schedule information.

        #endregion


    }
}
