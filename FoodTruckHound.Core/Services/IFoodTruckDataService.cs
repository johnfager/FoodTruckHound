using FoodTruckHound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckHound.Core.Services
{
    public interface IFoodTruckDataService : IService // implements the base service type
    {

        /// <summary>
        /// Calls the remote provider for fresh data.
        /// </summary>
        /// <returns></returns>
        Task<List<FoodTruckInfo>> RefreshRemoteDataAsync();
    }
}
