using FoodTruckHound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckHound.Core.Repositories
{
    public interface IFoodTruckLookupRepository : IRepository
    {
        /// <summary>
        /// Locates the closest food trucks.
        /// </summary>
        /// <param name="longitude">Starting longitude</param>
        /// <param name="latitude">Starting latitude</param>
        /// <param name="maxCount">The maximum count of food trucks to return</param>
        /// <returns></returns>
        public Task<List<FoodTruckInfo>> FindByLocationAsync(decimal longitude, decimal latitude, int maxCount = 5);
    }
}
