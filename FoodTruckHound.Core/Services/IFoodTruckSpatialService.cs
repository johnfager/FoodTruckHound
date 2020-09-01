using FoodTruckHound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckHound.Core.Services
{
    public interface IFoodTruckSpatialService : IService
    {
        /// <summary>
        /// Gets food trucks based on the provided longitude and latitude with a max count.
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        Task<List<FoodTruckResult>> GetFoodTrucksByDistanceAsync(double latitude, double longitude, int maxCount = 5);
    }
}
