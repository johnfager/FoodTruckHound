using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTruckHound.Models
{
    public class FoodTruckInfo
    {
        /// <summary>
        /// The internal unique key used by Food Truck Hound to identify unique food trucks.
        /// </summary>
        public string HoundKey { get; set; }

        public int LocationId { get; set; }

        public string DisplayName { get; set; }

        public string FacilityType { get; set; }

        public string LocationDescription { get; set; }

        public string Address { get; set; }

        public string Permit { get; set; }

        public string Status { get; set; }

        public string FoodItems { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        /// <summary>
        /// The URL of the food truck's schedule.
        /// </summary>
        /// <remarks>This URL is populated but is very slow responding and troubling. Looking for better schedule information.</remarks>
        public string ScheduleUrl { get; set; }

        public HoursOfOperation[] Schedule { get; set; }

    }
}
