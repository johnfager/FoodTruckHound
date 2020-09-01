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
        /// <returns></returns>
        public Task<List<FoodTruckInfo>> GetFoodTrucksAsync();
    }
}
