using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTruckHound.Data.Repositories
{
    /// <summary>
    /// Created to show that repositories can be replaced and registered as needed. SQL, NoSQL, Azure Tables, etc.
    /// </summary>
    public class FoodTruckSomeOtherStoreRepository : IFoodTruckLookupRepository
    {

        public Task<List<FoodTruckInfo>> GetFoodTrucksAsync()
        {
            throw new NotImplementedException("You stopped here and remember to go back...");
        }
    }
}
