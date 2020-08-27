using FoodTruckHound.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckHound.Core.Services
{
    public interface IFoodTruckDataService : IService // implements the base service type
    {
        /// <summary>
        /// The last time that <see cref="RefreshRemoteDataAsync"/> was called.
        /// </summary>
        DateTime LastRefreshedOnUtc { get; }

        /// <summary>
        /// The recommended number of minutes after which the provider recommends refreshing data using the <see cref="RefreshRemoteDataAsync"/>.
        /// </summary>
        int RecommendedRefreshInMinutes { get; set; }

        /// <summary>
        /// Calls the remote provider for fresh data.
        /// </summary>
        /// <returns></returns>
        Task<FoodTruckInfo[]> RefreshRemoteDataAsync();
    }
}
