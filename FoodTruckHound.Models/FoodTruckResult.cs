using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FoodTruckHound.Models
{
    public class FoodTruckResult : FoodTruckInfo

    {
        /// <summary>
        /// The internal unique key used by Food Truck Hound to identify unique food trucks.
        /// </summary>
        [Key]
        public string HoundKey { get; set; }

        public double DistanceInMiles { get; set; }

        //// TODO: Add in potentially in the future with more programming
        // public HoursOfOperation[] Schedule { get; set; }

    }
}
