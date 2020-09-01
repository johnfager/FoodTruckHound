using FoodTruckHound.Core.Repositories;
using FoodTruckHound.Core.Services;
using FoodTruckHound.Data.Utilities;
using FoodTruckHound.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodTruckHound.Data.Services
{

    // NOTE: Code credit from https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates/6366657 with lots of up votes.

    public class FoodTruckSpatialService : _baseService<FoodTruckSpatialService>, IFoodTruckSpatialService
    {
        private readonly IFoodTruckLookupRepository _foodTruckLookupRepository;

        public FoodTruckSpatialService(IFoodTruckLookupRepository foodTruckLookupRepository, ILogger<FoodTruckSpatialService> logger) : base(logger)
        {
            _foodTruckLookupRepository = foodTruckLookupRepository;
        }

        /// <summary>
        /// Works with repositories and services to go through
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public async Task<List<FoodTruckResult>> GetFoodTrucksByDistanceAsync(double latitude, double longitude, int maxCount = 5)
        {
            // NOTE: A standard filter could handle these ArgumentException errors, ideally with a custom error with client safe content; consider ErrorDetail

            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException($"{nameof(latitude)} must be betwee -90 and 90.");
            }

            if (longitude == -180 || longitude == 180)
            {
                throw new ArgumentException($"{nameof(longitude)} must be betwee -180 and 180.");
            }

            // use the repo to get our items
            var foodTrucks = await _foodTruckLookupRepository.GetFoodTrucksAsync();

            // convert the lat and long into a coordinate once for efficency
            var startingCoordinates = new Coordinates(latitude, longitude);

            // translate our data into a model with distance and a unique key better suited for our application
            var foodTrucksWithDistance = foodTrucks.Select(x => Convert(x, startingCoordinates: new Coordinates(latitude, longitude))); // a parameterized ctor with a colon for an example of clarity

            // get the closest and then by name
            // TODO: condsider the betst way to sort if name isn't ideal and there are multiple tied records for distance
            var closestFoodTrucks = foodTrucksWithDistance.OrderBy(x => x.DistanceInMiles).ThenBy(x => x.DisplayName).Take(maxCount).ToList();

            // return our model
            return closestFoodTrucks;
        }

        #region helpers

        private FoodTruckResult Convert(FoodTruckInfo foodTruckInfo, Coordinates startingCoordinates)
        {
            var value = new FoodTruckResult()
            {
                HoundKey = $"{foodTruckInfo.Permit}.{foodTruckInfo.DisplayName}", // vet this out in a real world scenario 
                LocationId = foodTruckInfo.LocationId,
                DisplayName = foodTruckInfo.DisplayName,
                FacilityType = foodTruckInfo.FacilityType,
                LocationDescription = foodTruckInfo.LocationDescription,
                Address = foodTruckInfo.Address,
                Permit = foodTruckInfo.Permit,
                Status = foodTruckInfo.Status,
                FoodItems = foodTruckInfo.FoodItems,
                Latitude = foodTruckInfo.Latitude,
                Longitude = foodTruckInfo.Longitude
            };

            value.DistanceInMiles = Math.Round((new Coordinates(foodTruckInfo.Latitude, foodTruckInfo.Longitude)).DistanceTo(startingCoordinates, UnitOfLength.Miles), 2);

            return value;
        }



        #endregion

        #region classes



        #endregion
    }

}
